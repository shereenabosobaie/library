import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';
import { Popup } from '../components/popup/popup';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, Popup],
  templateUrl: './admin-dashboard.html',
  styleUrls: ['./admin-dashboard.css']
})
export class AdminDashboard implements OnInit {
  requests: any[] = [];
  notifications: any[] = [];
  isLoading = true;
  errorMessage = '';
  @ViewChild('popup') popup!: Popup;
  private hubConnection!: signalR.HubConnection;
  private apiUrl = 'http://localhost:5114/api/Request';

  constructor(private http: HttpClient) { }

  ngOnInit() {
    const token = localStorage.getItem('token');
    if (!token) {
      this.popup.show('Please login as admin.');
      this.isLoading = false;
      return;
    }

    this.startSignalRConnection();
    this.loadRequests(token);
  }

  startSignalRConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5114/notifications')
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.hubConnection
      .start()
      .then(() => console.log(' Connected to SignalR hub'))
      .catch(err => console.error('SignalR connection error:', err));

    
    this.hubConnection.on('NewBorrowRequest', (data) => {
      console.log(' New Borrow Request received:', data);
      this.notifications.push(data);
      this.popup.show(` New borrow request! Member: ${data.memberId}, Book: ${data.bookId}`);

      this.loadRequests(localStorage.getItem('token')!);
    });
  }
 
  loadRequests(token: string) {
    this.isLoading = true;
    this.http.get<any[]>(`${this.apiUrl}/all`, {
      headers: { Authorization: `Bearer ${token}` }
    })
      .subscribe({
        next: (res) => {
          this.requests = res || [];
          console.log(' All requests:', this.requests);
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Failed to load requests:', err);
          this.errorMessage = 'Could not load borrow requests.';
          this.isLoading = false;
        }
      });
  }

  updateRequest(id: number, action: 'approve' | 'reject') {
    const token = localStorage.getItem('token');
    if (!token) return;

    const endpoint = `${this.apiUrl}/${action}/${id}`;
    this.http.put(endpoint, {}, {
      headers: { Authorization: `Bearer ${token}` },
      responseType: 'text'
    })
      .subscribe({
        next: () => {
          this.popup.show(`Request ${action}d successfully.`);
          this.requests = this.requests.map(r =>
            r.id === id ? { ...r, ended: action === 'approve' ? 'Approved' : 'Rejected' } : r
          );
        },
        error: (err) => {
          console.error(`Failed to ${action} request:`, err);
          this.popup.show(`Failed to ${action} request.`);
        }
      });
  }
  onImageError(event: Event) {
    const element = event.target as HTMLImageElement;
    element.src = 'assets/images/default-book.jpg';
  }
}

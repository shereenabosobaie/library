import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Popup } from '../components/popup/popup';

@Component({
  selector: 'app-borrow-requests',
  standalone: true,
  imports: [CommonModule,Popup],
  templateUrl: './borrow-requests.html',
  styleUrls: ['./borrow-requests.css']
})
export class BorrowRequests implements OnInit {
  requests: any[] = [];
  isLoading = true;
  errorMessage = '';
  @ViewChild('popup') popup!: Popup;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    const token = localStorage.getItem('token');

    if (!token) {
      this.errorMessage = 'Please login first.';
      this.isLoading = false;
      return;
    }

    this.loadRequests(token);
  }

  loadRequests(token: string) {
    this.http.get<any[]>('http://localhost:5114/api/Request/member', {
      headers: { Authorization: `Bearer ${token}` }
    })
      .subscribe({
        next: (res) => {
          console.log('Borrow requests:', res);
          this.requests = res || [];
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Failed to load borrow requests:', err);
          this.errorMessage = 'Could not load your borrow requests.';
          this.isLoading = false;
        }
      });
  }

  cancelRequest(id: number) {
    if (!confirm('Cancel this borrow request?')) return;

    const token = localStorage.getItem('token');
    if (!token) return;

    this.http.delete(`http://localhost:5114/api/Request/cancel/${id}`, {
      headers: { Authorization: `Bearer ${token}` },
      responseType: 'text'  
    })
      .subscribe({
        next: () => {
          this.popup.show('Request canceled successfully.');
          this.requests = this.requests.filter(r => r.id !== id);
        },
        error: (err) => {
          console.error('Cancel failed:', err);
          this.popup.show('Failed to cancel the request.');
        }
      });
  }
  getFullImageUrl(imagePath: string): string {
    if (!imagePath) return 'assets/images/default-book.jpg';
    if (imagePath.startsWith('http')) return imagePath;
    if (!imagePath.startsWith('/')) imagePath = '/' + imagePath;
    return 'http://localhost:5114' + imagePath;
  }
}

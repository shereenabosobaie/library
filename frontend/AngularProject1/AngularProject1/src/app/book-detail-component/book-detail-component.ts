import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Popup } from '../components/popup/popup';
import { jwtDecode } from 'jwt-decode';
@Component({
  selector: 'app-book-detail',
  standalone: true,
  imports: [CommonModule,Popup],
  templateUrl: './book-detail-component.html',
  styleUrls: ['./book-detail-component.css']
})
export class BookDetailComponent {
  bookId = 0;
  book: any = null;
  isLoading = true;
  errorMessage = '';
  isAdmin = false;
  @ViewChild('popup') popup!: Popup;
  constructor(private route: ActivatedRoute, private router: Router, private http: HttpClient) {}

  ngOnInit() {
    this.checkRole();
    this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.bookId = +idParam;
        this.fetchBookDetails();
      }
    });
  }
  checkRole() {
    const token = localStorage.getItem('token');
    if (!token) {
      this.isAdmin = false;
      return;
    }

    try {
      const decoded: any = jwtDecode(token);
      const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      console.log('Decoded role:', role);
      this.isAdmin = role === 'Admin';
    } catch (error) {
      console.error('Error decoding token:', error);
      this.isAdmin = false;
    }
  }
  
  fetchBookDetails() {
    this.http.get(`http://localhost:5114/api/bookcontroler/${this.bookId}`)
      .subscribe({
        next: (res: any) => {
          
          this.book = {
            Id: res.id || res.Id,
            Title: res.title || res.Title,
            Author: res.author || res.Author,
            Description: res.description || res.Description,
            ImageUrl: res.imageUrl || res.ImageUrl,
            IsAvilable: res.isAvilable || res.IsAvilable,
            Rate: res.rate || res.Rate
          };

          console.log('Book loaded:', this.book); 
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Failed to fetch book details:', err);
          this.errorMessage = 'Book not found or unavailable.';
          this.isLoading = false;
        }
      });
  }

  
  goBack() {
    this.router.navigate(['/search']);
  }

  
  borrowBook() {
    const token = localStorage.getItem('token');
    if (!token) {
      this.popup.show('Please login to borrow books.');
      this.router.navigate(['/login']);
      return;
    }

    console.log('Navigating to borrow request for book:', this.book);
    this.router.navigate(['/borrow-request', this.book.Id]);
  }
  editBook() {
    this.router.navigate([`/admin/update-book`, this.book.Id]);
  }
  deleteBook() {
    if (!confirm(`Are you sure you want to delete "${this.book.Title}"?`)) return;

    const token = localStorage.getItem('token');
    if (!token) {
      this.popup.show('Unauthorized!');
      return;
    }

    this.http.delete(`http://localhost:5114/api/bookcontroler/${this.book.Id}`, {
      headers: { Authorization: `Bearer ${token}` },
      responseType: 'text'
    })
      .subscribe({
        next: () => {
          this.popup.show('Book deleted successfully!');
          this.router.navigate(['/home']);
        },
        error: (err) => {
          console.error('Delete failed:', err);
          this.popup.show('Failed to delete book.');
        }
      });

  }
  rateBook(stars: number) {
    this.popup.show(`You rated this book ${stars} stars.`);
  }
}

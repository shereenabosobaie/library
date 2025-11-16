import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Popup } from '../components/popup/popup';

@Component({
  selector: 'app-borrow-req',
  standalone: true,
  imports: [CommonModule, FormsModule,Popup],
  templateUrl: './borrow-req.html',
  styleUrls: ['./borrow-req.css']
})
export class BorrowReq implements OnInit {
  bookId = 0;
  book: any = null;
  days = 7;
  isLoading = true;
  errorMessage = '';
  @ViewChild('popup') popup!: Popup;
  constructor(private route: ActivatedRoute, private http: HttpClient, private router: Router) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.bookId = +idParam;
        console.log('Book ID from route:', this.bookId);
        this.fetchBookDetails();
      }
    });
  }

  fetchBookDetails() {
    this.isLoading = true;
    this.http.get(`http://localhost:5114/api/bookcontroler/${this.bookId}`)
      .subscribe({
        next: (res: any) => {
          console.log('Raw API response:', res);

          const bookId = res.id || res.Id || res.bookId || this.bookId;

          this.book = {
            id: bookId,
            Id: bookId,
            Title: res.title || res.Title,
            Author: res.author || res.Author,
            Description: res.description || res.Description,
            ImageUrl: res.imageUrl || res.ImageUrl || 'assets/images/default-book.jpg',
            isAvilable: res.isAvilable ?? res.IsAvilable ?? true,
            publishYear: res.publishYear || res.PublishYear
          };

          console.log('Processed book object:', this.book);
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error loading book details:', err);
          this.errorMessage = 'Failed to load book details. Please try again.';
          this.isLoading = false;
        }
      });
  }

 
  validateDays() {
    if (this.days < 1) {
      this.days = 1;
    } else if (this.days > 30) {
      this.days = 30;
    }
  }

  confirmBorrow() {
    
    const token = localStorage.getItem('token');
    if (!token) {
      this.popup.show('Please login first.');
      this.router.navigate(['/login']);
      return;
    }

    
    if (!this.book.isAvilable) {
      this.popup.show('This book is currently unavailable for borrowing.');
      return;
    }

    
    if (this.days < 1 || this.days > 30) {
      this.popup.show('Please select a borrowing period between 1 and 30 days.');
      return;
    }

   
    const bookId = this.book.id || this.book.Id || this.bookId;

    console.log('Submitting borrow request:', {
      bookId: bookId,
      days: this.days,
      bookObject: this.book
    });

    const requestBody = {
      bookId: bookId,
      days: this.days
    };

    this.http.post(`http://localhost:5114/api/Request/create`, requestBody, {
      headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    }).subscribe({
      next: (response: any) => {
        console.log('Borrow request successful:', response);
        this.popup.show(response.message || 'Borrow request submitted successfully!');
        this.router.navigate(['/borrow-requests']);
      },
      error: (err) => {
        console.error('Borrow request failed:', err);

        
        if (err.error) {
          console.log('Error response body:', err.error);
        }

        let errorMessage = 'Failed to submit borrow request. Please try again.';

        if (err.status === 400) {
          errorMessage = err.error?.message || 'You already have a pending request for this book.';
        } else if (err.status === 401) {
          errorMessage = 'Your session has expired. Please login again.';
          this.router.navigate(['/login']);
        } else if (err.status === 404) {
          errorMessage = 'Book not found. Please try another book.';
        } else if (err.status === 500) {
          errorMessage = 'Server error. Please try again later.';
        }

        this.popup.show(errorMessage);
      }
    });
  }

  cancel() {
    this.router.navigate(['/search']);
  }

  checkAuthStatus(): boolean {
    const token = localStorage.getItem('token');
    if (!token) {
      this.popup.show('Please login to continue.');
      this.router.navigate(['/login']);
      return false;
    }
    return true;
  }

  
  validateRequest(): boolean {
    if (!this.book) {
      this.popup.show('Book information is not loaded. Please try again.');
      return false;
    }

    if (!this.book.isAvilable) {
      this.popup.show('This book is not available for borrowing.');
      return false;
    }

    if (this.days < 1 || this.days > 365) {
      this.popup.show('Please select a valid borrowing period (1-365 days).');
      return false;
    }

    return true;
  }

  
  borrowBook() {
    if (!this.checkAuthStatus() || !this.validateRequest()) {
      return;
    }

    this.confirmBorrow();
  }
  getFullImageUrl(imagePath: string): string {
    if (!imagePath) return 'assets/images/default-book.jpg';
    if (imagePath.startsWith('http')) return imagePath;
    if (!imagePath.startsWith('/')) imagePath = '/' + imagePath;
    return 'http://localhost:5114' + imagePath;
  }
}

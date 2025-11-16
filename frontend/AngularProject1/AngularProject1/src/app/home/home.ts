import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class Home implements OnInit {
  books: any[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    this.fetchRecentBooks();
  }

  fetchRecentBooks() {
    this.http.get<any[]>('http://localhost:5114/api/bookcontroler/recent').subscribe({
      next: (res) => {
        this.books = res;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load books:', err);
        this.errorMessage = 'Failed to load recent books.';
        this.isLoading = false;
      }
    });
  }

  viewBook(bookId: number) {
    this.router.navigate(['/book', bookId]);
  }

  
}

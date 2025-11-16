import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './search.html',
  styleUrls: ['./search.css']
})
export class Search implements OnInit {

  searchQuery = '';
  searchResults: any[] = [];
  books: any[] = [];          
  isLoading = true;
  errorMessage = '';

  constructor(private router: Router, private http: HttpClient) { }

  ngOnInit() {
    this.fetchRecentBooks();
  }

  searchBooks() {
    if (!this.searchQuery.trim()) {
      this.searchResults = [];   
      return;
    }

    this.http.get<any[]>(`http://localhost:5114/api/bookcontroler/search?query=${this.searchQuery}`)
      .subscribe({
        next: (res) => {
          this.searchResults = res;
          console.log('Search results:', res);
        },
        error: (err) => {
          console.error('Search error:', err);
          this.searchResults = [];
        }
      });
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

  
  getFullImageUrl(book: any): string {
    const path = book.imageUrl || book.ImageUrl;
    if (!path) return 'assets/default-book.png';
    if (path.startsWith('http')) return path;
    return 'http://localhost:5114' + (path.startsWith('/') ? path : '/' + path);
  }
}

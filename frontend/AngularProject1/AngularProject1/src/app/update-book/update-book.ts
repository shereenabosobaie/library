import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Popup } from '../components/popup/popup';

@Component({
  selector: 'app-update-book',
  standalone: true,
  imports: [CommonModule, FormsModule, Popup],
  templateUrl: './update-book.html',
  styleUrls: ['./update-book.css']
})
export class UpdateBook implements OnInit {
  bookId = 0;
  book: any = {};
  imageFile: File | null = null;
  previewUrl: string | null = null;
  isLoading = true;
  errorMessage = '';
  @ViewChild('popup') popup!: Popup;

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.bookId = +idParam;
        this.fetchBookDetails();
      }
    });
  }

  fetchBookDetails() {
    this.http.get(`http://localhost:5114/api/bookcontroler/${this.bookId}`)
      .subscribe({
        next: (res: any) => {
          this.book = res;
          this.previewUrl = 'http://localhost:5114' + (res.imageUrl || res.ImageUrl);
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Failed to fetch book details:', err);
          this.errorMessage = 'Failed to load book details.';
          this.isLoading = false;
        }
      });
  }

  onImageSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.imageFile = file;
      const reader = new FileReader();
      reader.onload = () => (this.previewUrl = reader.result as string);
      reader.readAsDataURL(file);
    }
  }

  updateBook() {
    const token = localStorage.getItem('token');
    if (!token) {
      this.popup.show('Unauthorized!');
      return;
    }

    const formData = new FormData();

    formData.append('Title', this.book.Title || this.book.title || '');
    formData.append('Author', this.book.Author || this.book.author || '');
    formData.append('PublishYear', (this.book.PublishYear || this.book.publishYear || 0).toString());
    formData.append('Description', this.book.Description || this.book.description || '');
    formData.append('IsAvilable', (this.book.IsAvilable ?? true).toString());
    formData.append('Rate', (this.book.Rate || this.book.rate || 0).toString());

    if (this.imageFile) {
      formData.append('image', this.imageFile);
    }

    this.http.put(`http://localhost:5114/api/bookcontroler/update/${this.bookId}`, formData, {
      headers: { Authorization: `Bearer ${token}` }
    })
      .subscribe({
        next: (res: any) => {
          this.popup.show(res.message || 'Book updated successfully!');
          this.router.navigate(['/book', this.bookId]);
        },
        error: (err) => {
          console.error('Update failed:', err);
          this.popup.show('Failed to update book.');
        }
      });
  }

  cancel() {
    this.router.navigate(['/book', this.bookId]);
  }
}

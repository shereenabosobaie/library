import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Popup } from '../components/popup/popup';

@Component({
  selector: 'app-add-book',
  standalone: true,
  imports: [CommonModule, FormsModule,Popup],
  templateUrl: './add-book.html',
  styleUrl: './add-book.css',
})
export class AddBook {
  book = {
    title: '',
    author: '',
    publishYear: '',
    description: ''
  };
  @ViewChild('popup') popup!: Popup;
  imageFile: File | null = null;
  message = '';
  previewUrl: string | ArrayBuffer | null = null;

  constructor(private http: HttpClient, private router: Router) { }

  onFileSelected(event: any) {
    this.imageFile = event.target.files[0];

    if (this.imageFile) { 
      const reader = new FileReader();
      reader.onload = () => this.previewUrl = reader.result;
      reader.readAsDataURL(this.imageFile); 
    }
  }
  addBook() {
    const formData = new FormData();
    formData.append('title', this.book.title);
    formData.append('author', this.book.author);
    formData.append('publishYear', this.book.publishYear.toString());
    formData.append('description', this.book.description);
    if (this.imageFile) {
      formData.append('image', this.imageFile);
    }

    this.http.post('http://localhost:5114/api/bookcontroler/create', formData, {
      headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
    }).subscribe({
      next: (res: any) => {
        this.popup.show(' Book added successfully!');
        this.router.navigate(['/search']);
      },
      error: (err) => {
        console.error(err);
        this.popup.show(' Failed to add book.');
      }
    });
  }
}

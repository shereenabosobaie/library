import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrls: ['./profile.css']
})
export class Profile implements OnInit {
  user = {
    id: 0,
    name: '',
    email: '',
    password: '',
    imageUrl: ''
  };

  selectedFile: File | null = null;
  successMessage = '';
  errorMessage = '';

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    this.loadUserFromServer();
  }

  loadUserFromServer() {
    const token = localStorage.getItem('token');
    if (!token) {
      this.router.navigate(['/login']);
      return;
    }

    this.http.get('http://localhost:5114/api/Member/current', {
      headers: { Authorization: `Bearer ${token}` }
    }).subscribe({
      next: (res: any) => {
        this.user = res; 
        localStorage.setItem('user', JSON.stringify(res));
      },
      error: (err) => {
        console.error('Failed to load user:', err);
        this.router.navigate(['/login']);
      }
    });
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  uploadImage() {
    if (!this.selectedFile || !this.user.id) return;

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    this.http.post(`http://localhost:5114/api/Member/upload-image?memberId=${this.user.id}`, formData)
      .subscribe({
        next: (res: any) => {
          this.user.imageUrl = res.imageUrl;
          localStorage.setItem('userImageUrl', res.imageUrl);
          this.successMessage = 'Image uploaded successfully!';
        },
        error: () => this.errorMessage = 'Image upload failed.'
      });
  }

  saveChanges() {
    const updateData = {
      id: this.user.id,
      name: this.user.name,
      email: this.user.email,
      password: this.user.password || null,
      imageUrl: this.user.imageUrl
    };

    this.http.put('http://localhost:5114/api/Member/update', updateData)
      .subscribe({
        next: () => {
          this.successMessage = 'Profile updated successfully!';
          localStorage.setItem('user', JSON.stringify(this.user));
        },
        error: () => this.errorMessage = 'Profile update failed.'
      });
  }
}

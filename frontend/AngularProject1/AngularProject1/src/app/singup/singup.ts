import { Component, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Popup } from '../components/popup/popup';

@Component({
  selector: 'app-singup',
  standalone: true,
  imports: [FormsModule, CommonModule, Popup],
  templateUrl: './singup.html',
  styleUrls: ['./singup.css']
})
export class Singup {
  username = '';
  email = '';
  password = '';
  confirmPassword = '';
  selectedFile: File | null = null;
  previewUrl: string | null = null;

  @ViewChild('popup') popup!: Popup;

  constructor(private router: Router, private http: HttpClient) { }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];

    if (this.selectedFile) {
      const reader = new FileReader();
      reader.onload = (e: any) => (this.previewUrl = e.target.result);
      reader.readAsDataURL(this.selectedFile);
    }
  }

  onSignup() {
    if (!this.username || !this.email || !this.password || !this.confirmPassword) {
      this.popup.show('Please fill in all fields!', 'error');
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.popup.show('Passwords do not match!', 'error');
      return;
    }

    const formData = new FormData();
    formData.append('Name', this.username);
    formData.append('Email', this.email);
    formData.append('password', this.password);

    if (this.selectedFile) {
      formData.append('file', this.selectedFile);
    }

    this.http.post('http://localhost:5114/api/Member/register', formData).subscribe({
      next: (res: any) => {
        this.popup.show('Registration successful!', 'success');
        console.log('Registered:', res);
        setTimeout(() => this.router.navigate(['/login']), 1500);
      },
      error: (err) => {
        console.error('Registration error:', err);
        const msg = err.status === 409 ? 'Email already exists!' : 'Registration failed!';
        this.popup.show(msg, 'error');
      }
    });
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }
}

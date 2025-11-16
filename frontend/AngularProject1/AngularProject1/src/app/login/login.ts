import { Component, ViewChild } from '@angular/core';
import { Header } from '../components/header/header';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../auth.service';
import { CommonModule } from '@angular/common';
import { Popup } from '../components/popup/popup';
import { logincheck } from '../../logincheck';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, Popup],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  @ViewChild('popup') popup!: Popup;

  Email: string = '';
  password: string = '';

  //onLogin() {
    
  //  console.log('Login attempted with:', this.username, this.password);
  //}
  constructor(private router: Router, private authService: AuthService, private authState: logincheck) { }

  errorMessage: string = '';

  onLogin() {
    this.authService.login(this.Email, this.password).subscribe({
      next: (res) => {
        console.log('Login response:', res); 

        localStorage.setItem('token', res.Token || res.token); 
        localStorage.setItem('isLoggedIn', 'true');
        this.authState.login();

        this.popup.show('Login successful!', 'success');
        setTimeout(() => this.router.navigate(['/home']), 1500);
      },
      error: () => {
        this.popup.show('Invalid email or password!', 'error');
      }
    });
  }

  onSignUp() {
   
    this.router.navigate(['/signup']);
  }

}

import { CommonModule } from '@angular/common';
import { Component, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { logincheck } from '../../../logincheck';
import { Subscription } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  constructor(private router: Router, private authState: logincheck, private cdr: ChangeDetectorRef) { }

  isLoggedIn = false;
  isAdmin = false;   
  userImageUrl: string | null = null;
  private sub!: Subscription;

  ngOnInit() {
    this.sub = this.authState.isLoggedIn$.subscribe((loggedIn) => {
      console.log('Header detected login state change:', loggedIn);
      this.isLoggedIn = loggedIn;

      if (loggedIn) {
        this.checkRole(); 
      } else {
        this.isAdmin = false;
      }

      this.userImageUrl = localStorage.getItem('userImageUrl');
      this.cdr.detectChanges();
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
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

  goToAddBook() {
    this.router.navigate(['/add-book']);
  }
  goToProfile() {
    this.router.navigate(['/profile']);
  }

  logout() {
    this.authState.logout();
    this.router.navigate(['/login']);
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }

  goToSignup() {
    this.router.navigate(['/signup']);
  }

  goToHome() {
    this.router.navigate(['/home']);
  }

  goToSearch() {
    this.router.navigate(['/search']);
  }

  goToRequests() {
    this.router.navigate(['/borrow-requests']);
  }

  goToAdminDashboard() {
    this.router.navigate(['/admin-dashboard']);
  }
}

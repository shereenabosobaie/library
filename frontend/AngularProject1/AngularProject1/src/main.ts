import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { Login } from './app/login/login';
import { Home } from './app/home/home';
import { provideRouter, Routes } from '@angular/router';
import { Singup } from './app/singup/singup';
import { BookDetailComponent } from './app/book-detail-component/book-detail-component';
import { Search } from './app/search/search';
import { HttpClientModule } from '@angular/common/http';
import { importProvidersFrom } from '@angular/core';
import { Profile } from './app/profile/profile';
import { authGuard } from './auth.guard';
import { BorrowRequests } from './app/borrow-requests/borrow-requests';
import { BorrowReq } from './app/borrow-req/borrow-req';
import { AdminDashboard } from './app/admin-dashboard/admin-dashboard';
import { AddBook } from './app/add-book/add-book';
import { UpdateBook } from './app/update-book/update-book';

const routes: Routes = [
  { path: '', component: Home },
  { path: 'home', component: Home },
  { path: 'login', component: Login },
  { path: 'signup', component: Singup },
  { path: 'search', component: Search },
  { path: 'profile', component: Profile, canActivate: [authGuard] },
  { path: 'add-book', component: AddBook, canActivate: [authGuard] },
  { path: 'admin/update-book/:id', component: UpdateBook, canActivate: [authGuard] },
  { path: 'borrow-requests', component: BorrowRequests },
  { path: 'borrow-request/:id', component: BorrowReq },
  { path: 'admin-dashboard', component: AdminDashboard },
  { path: 'book/:id', component: BookDetailComponent },
  { path: '**', redirectTo: 'home' }
];

bootstrapApplication(App, {
  providers: [
    provideRouter(routes),importProvidersFrom(HttpClientModule)
    
  ]
}).catch(err => console.error(err));

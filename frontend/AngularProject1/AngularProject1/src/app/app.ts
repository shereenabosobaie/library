import { Component } from '@angular/core';
import { Header } from './components/header/header';
import { Login } from './login/login';
import { RouterOutlet } from '@angular/router';
@Component({
  selector: 'app-root',
  imports: [Header, RouterOutlet],
  standalone: true,
  template: `
    <app-header></app-header>
    <router-outlet></router-outlet>
  `,
  styleUrls: ['./app.css']
})
export class App {
  title = 'e-library';
}

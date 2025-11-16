import { CommonModule } from '@angular/common';
import { Component ,Input } from '@angular/core';

@Component({
  selector: 'app-popup',
  standalone: true,
  imports: [CommonModule], 
  templateUrl: './popup.html',
  styleUrl: './popup.css',
})
export class Popup {
  @Input() message: string = '';
  @Input() type: 'success' | 'error' | 'info' = 'info';
  isVisible = false;

  show(message: string, type: 'success' | 'error' | 'info' = 'info') {
    this.message = message;
    this.type = type;
    this.isVisible = true;
    setTimeout(() => (this.isVisible = false), 3000); 
  }
}

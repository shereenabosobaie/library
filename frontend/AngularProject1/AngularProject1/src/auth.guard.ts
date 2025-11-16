import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { logincheck } from './logincheck';

export const authGuard: CanActivateFn = () => {
  const authState = inject(logincheck);
  const router = inject(Router);

  const hasToken = !!localStorage.getItem('token');

  if (hasToken) {
    authState.refreshState();
    return true; 
  }

  router.navigate(['/login']);
  return false;
};

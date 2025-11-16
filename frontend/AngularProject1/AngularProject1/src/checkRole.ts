import { Injectable } from "@angular/core";
import { jwtDecode } from "jwt-decode";

@Injectable({ providedIn: 'root' })
export class checkRole {
  isAdmin(): boolean {
    const token = localStorage.getItem('token');
    if (!token) return false;
    try {
      const decoded: any = jwtDecode(token);
      const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return role === 'Admin';
    } catch {
      return false;
    }
  }
}

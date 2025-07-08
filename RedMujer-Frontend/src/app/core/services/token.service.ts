import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor() { }

  private tokenKey = 'tokenRedMujer';

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  decodeToken(): any | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      return jwtDecode(token);
    } catch (error) {
      console.error('Error decodificando el token:', error);
      return null;
    }
  }

  getNameIdentifier(): string | null {
    const decoded = this.decodeToken();
    return decoded?.['nameidentifier'] || null;
  }

  getRole(): string | null {
    const decoded = this.decodeToken();
    return decoded?.role || null;
  }

  isTokenExpired(): boolean {
    const decoded = this.decodeToken();
    if (!decoded?.exp) return true;

    const expiry = decoded.exp * 1000;
    return Date.now() > expiry;
  }

  clearToken(): void {
    localStorage.removeItem(this.tokenKey);
  }
}

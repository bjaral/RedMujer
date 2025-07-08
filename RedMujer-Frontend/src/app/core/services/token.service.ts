import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor() {}

  private tokenKey = 'tokenRedMujer';

  // Namespaces comunes
  private claimTypes = {
    nameIdentifier: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier',
    role: 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',
    email: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress',
    name: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'
  };

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

  getNameIdentifier(): number | null {
    const decoded = this.decodeToken();
    if (!decoded) return null;

    const nameIdentifier = decoded[this.claimTypes.nameIdentifier];
    const id = Number(nameIdentifier);

    return isNaN(id) ? null : id;
  }

  getRole(): string | null {
    const decoded = this.decodeToken();
    if (!decoded) return null;

    return decoded[this.claimTypes.role] || null;
  }

  isTokenExpired(): boolean {
    const decoded = this.decodeToken();
    const exp = decoded?.exp;

    if (!exp || typeof exp !== 'number') return true;

    const expiryTime = exp * 1000;
    return Date.now() > expiryTime;
  }

  clearToken(): void {
    localStorage.removeItem(this.tokenKey);
  }
}

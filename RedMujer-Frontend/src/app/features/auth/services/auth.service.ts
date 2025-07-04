import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'http://localhost:5145/api/auth';
  private usuarioUrl = 'http://localhost:5145/api/usuario';
  private personaUrl = 'http://localhost:5145/api/persona';
  private ubicacionUrl = 'http://localhost:5145/api/ubicacion';

  constructor(private http: HttpClient) { }

  login(credentials: { email: string; password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials);
  }

  register(data: { usuario: any; persona: any; ubicacion: any }): Observable<any> {
    // Primero crea usuario, luego persona, luego ubicación
    return this.createUsuario(data.usuario).pipe(
      switchMap(usuarioRes =>
        this.createPersona({ ...data.persona, usuarioId: usuarioRes.id }).pipe(
          switchMap(personaRes =>
            this.createUbicacion({ ...data.ubicacion, personaId: personaRes.id })
          )
        )
      )
    );
  }

  private createUsuario(usuario: any): Observable<any> {
    return this.http.post(this.usuarioUrl, usuario);
  }

  private createPersona(persona: any): Observable<any> {
    return this.http.post(this.personaUrl, persona);
  }

  private createUbicacion(ubicacion: any): Observable<any> {
    return this.http.post(this.ubicacionUrl, ubicacion);
  }

  logout(): void {
    localStorage.removeItem('tokenRedMujer');
  }
}

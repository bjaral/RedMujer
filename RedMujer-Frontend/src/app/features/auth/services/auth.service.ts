import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'http://localhost:5145/api/Auth';
  private usuarioUrl = 'http://localhost:5145/api/Usuarios';
  private personaUrl = 'http://localhost:5145/api/Personas';
  private ubicacionUrl = 'http://localhost:5145/api/Ubicaciones';

  constructor(private http: HttpClient) { }

  login(credentials: { Correo: string; Password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials);
  }

  register(data: { usuario: any; persona: any; ubicacion: any }): Observable<any> {
  return this.createUsuario(data.usuario).pipe(
    switchMap(usuarioRes =>

      this.createUbicacion(data.ubicacion).pipe(

        switchMap(ubicacionRes =>
          this.createPersona({
            ...data.persona,
            usuarioId: usuarioRes.id,
            idUbicacion: ubicacionRes.id
          })
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

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { TokenService } from './token.service';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = `${environment.apiUrl}/Auth`;
  private usuarioUrl = `${environment.apiUrl}/Usuarios`;
  private personaUrl = `${environment.apiUrl}/Personas`;
  private ubicacionUrl = `${environment.apiUrl}/Ubicaciones`;

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  login(credentials: { correo: string; password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials);
  }

  register(data: { usuario: any; persona: any; ubicacion: any }): Observable<any> {
    return this.createUsuario(data.usuario).pipe(
      switchMap(usuarioRes =>
        this.createUbicacion(data.ubicacion).pipe(
          switchMap(ubicacionRes =>
            this.createPersona({
              ...data.persona,
              Id_Usuario: usuarioRes.id,
              Id_Ubicacion: ubicacionRes.id
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

  verificarCorreo(correo: string): Observable<{ existe: boolean; correo: string }> {
    return this.http.get<{ existe: boolean; correo: string }>(
      `${this.usuarioUrl}/verificar-correo?correo=${encodeURIComponent(correo)}`
    );
  }

  logout(): void {
    this.tokenService.clearToken();
  }
}
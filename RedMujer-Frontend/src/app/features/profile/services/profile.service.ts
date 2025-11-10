import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  obtenerEmprendimientosPorPersona(idPersona: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Personas/${idPersona}/emprendimientos`);
  }

  borrarEmprendimientoPorId(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/Emprendimientos/${id}`);
  }

  // Obtener usuario
  obtenerUsuario(userId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Usuarios/${userId}`);
  }

  // Obtener persona
  obtenerPersona(personaId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Personas/${personaId}`);
  }

  // Obtener ubicación
  obtenerUbicacion(ubicacionId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Ubicaciones/${ubicacionId}`);
  }

  // Obtener perfil completo (Usuario → Persona → Ubicación)
  obtenerPerfilCompleto(userId: number): Observable<any> {
    return this.obtenerUsuario(userId).pipe(
      switchMap(usuario => {
        // console.log('Usuario obtenido:', usuario);
        
        return this.http.get<any[]>(`${this.apiUrl}/Personas`).pipe(
          map(personas => {
            // Buscar la persona que tenga este id_Usuario
            const persona = personas.find(p => p.id_Usuario === userId);
            if (!persona) {
              throw new Error(`No se encontró persona para el usuario ${userId}`);
            }
            return { usuario, persona };
          }),
          switchMap(({ usuario, persona }) => {
            // console.log('Persona obtenida:', persona);
            // Con la persona obtenemos la ubicación
            return this.obtenerUbicacion(persona.id_Ubicacion).pipe(
              map(ubicacion => {
                // console.log('Ubicación obtenida:', ubicacion);
                return {
                  usuario,
                  persona,
                  ubicacion
                };
              })
            );
          })
        );
      })
    );
  }

  // Actualizar usuario (username y email)
  actualizarUsuario(userId: number, datos: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/Usuarios/${userId}`, datos);
  }

  // Actualizar persona
  actualizarPersona(personaId: number, datos: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/Personas/${personaId}`, datos);
  }

  // Actualizar ubicación
  actualizarUbicacion(ubicacionId: number, datos: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/Ubicaciones/${ubicacionId}`, datos);
  }

  // Actualizar perfil completo (Usuario → Ubicación → Persona)
  actualizarPerfilCompleto(
    userId: number, 
    personaId: number, 
    ubicacionId: number, 
    datosUsuario: any, 
    datosPersona: any, 
    datosUbicacion: any
  ): Observable<any> {
    // Primero actualizar Usuario, luego Ubicación, luego Persona
    return this.actualizarUsuario(userId, datosUsuario).pipe(
      switchMap(() => this.actualizarUbicacion(ubicacionId, datosUbicacion)),
      switchMap(() => this.actualizarPersona(personaId, datosPersona))
    );
  }

  // Cambiar contraseña
  cambiarContrasena(userId: number, contrasenaActual: string, contrasenaNueva: string): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/Usuarios/${userId}/cambiar-contrasena`, {
      contrasenaActual,
      contrasenaNueva
    });
  }

  // Contar emprendimientos
  contarEmprendimientos(personaId: number): Observable<number> {
    return this.obtenerEmprendimientosPorPersona(personaId).pipe(
      map(emprendimientos => emprendimientos.length)
    );
  }
}
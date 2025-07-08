import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PersonaService {

  private personaUrl = 'http://localhost:5145/api/Personas';
  private personaEmprendimientoUrl = 'http://localhost:5145/api/PersonaEmprendimiento';

  constructor(private http: HttpClient) { }

  getIdPersona(idUsuario: number) {
    return this.http.get<any>(`${this.personaUrl}/usuario/${idUsuario}`).pipe(
      map(response => response.id_Persona)
    );
  }
  
  postEmprendimientoToPersona(idPersona: number, idEmprendimiento: number): Observable<any> {
    const body = {
      id_Persona: idPersona,
      id_Emprendimiento: idEmprendimiento
    };
    return this.http.post<any>(this.personaEmprendimientoUrl, body);
  }

}

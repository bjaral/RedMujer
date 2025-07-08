import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PersonaService {

  private personaUrl = 'http://localhost:5145/api/Personas';

  constructor(private http: HttpClient) { }

  getPersona(idUsuario: number) {
    return this.http.get<any>(`${this.personaUrl}/persona/${idUsuario}`);
  }

}

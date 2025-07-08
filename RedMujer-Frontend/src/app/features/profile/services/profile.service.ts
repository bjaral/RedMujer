import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class ProfileService {

  constructor(private http: HttpClient) {}

  private url = 'http://localhost:5145/api';

  obtenerEmprendimientosPorPersona(idPersona: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.url}/Personas/${idPersona}/emprendimientos`);
  }

  borrarEmprendimientoPorId(id: number): Observable<any> {
    return this.http.delete<any>(`${this.url}/Emprendimientos/${id}`);
  }

}

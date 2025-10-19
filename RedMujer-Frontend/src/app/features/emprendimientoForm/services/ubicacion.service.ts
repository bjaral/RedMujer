import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UbicacionService {

  private apiUrl = 'http://localhost:5145/api';

  constructor(private http: HttpClient) { }

  regiones(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Regiones`);
  }

  comunas(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Comunas`);
  }

  comunasPorRegion(idRegion: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Comunas/region/${idRegion}`);
  }

}
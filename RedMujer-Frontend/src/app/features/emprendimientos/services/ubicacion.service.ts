import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UbicacionService {

  private apiUrl = environment.apiUrl;

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
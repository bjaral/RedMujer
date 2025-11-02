import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAll(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Emprendimientos`);
  }

  getRandom(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Emprendimientos/random/5`);
  }

  getCategoriasByEmprendimiento(idEmprendimiento: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/emprendimientos/${idEmprendimiento}/categorias`);
  }
}
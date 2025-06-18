import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor(private http: HttpClient) {}

  url = 'http://localhost:5145/api'

  getAll(): Observable<any> {
    return this.http.get<any>(`${this.url}/Emprendimientos`);
  }

  getRandom(): Observable<any> {
    return this.http.get<any>(`${this.url}/Emprendimientos/random/1`);
  }
}
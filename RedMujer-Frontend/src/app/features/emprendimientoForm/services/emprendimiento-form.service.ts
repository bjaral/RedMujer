import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EmprendimientoFormService {

  private apiUrl = 'http://localhost:5145/api/Emprendimientos';

  constructor(private http: HttpClient) {}

  //Nuevo emprendimiento

  crearEmprendimiento(formData: FormData): Observable<any> {
    return this.http.post<any>(this.apiUrl, formData);
  }

  subirMultimedia(idEmprendimiento: number, archivos: File[]): Observable<any> {
    const formData = new FormData();
    archivos.forEach((archivo) => {
      formData.append('Imagenes', archivo);
    });
    return this.http.post<any>(`${this.apiUrl}/${idEmprendimiento}/imagenes-emprendimiento`, formData);
  }

  //Editar emprendimiento

  obtenerEmprendimientoPorId(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  actualizarEmprendimiento(id: number, formData: FormData): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, formData);
  }

  borrarImagenPrincipal(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}/imagen-principal`); 
  }

  obtenerMultimediaPorId(id: number): Observable<string[]> {
    return this.http.get<{ imagenes: string[] }>(`${this.apiUrl}/${id}/imagenes-emprendimiento`)
    .pipe(map(response => response.imagenes));
  }

  actualizarMultimedia(id: number, archivos: File[]): Observable<any> {
    const formData = new FormData();
    archivos.forEach((archivo) => {
      formData.append('Imagenes', archivo);
    });
    return this.http.put<any>(`${this.apiUrl}/${id}/imagenes-emprendimiento`, formData);
  }

  borrarImagenAdicional(id: number, nombreArchivo: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}/imagenes-emprendimiento/${nombreArchivo}`);
  }

}

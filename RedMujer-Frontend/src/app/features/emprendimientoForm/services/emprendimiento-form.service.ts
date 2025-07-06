import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EmprendimientoFormService {

  private apiUrl = 'http://localhost:5145/api/Emprendimientos';

  constructor(private http: HttpClient) {}

  //Nuevo emprendimiento

  crearEmprendimiento(data: any, imagen: File | null): Observable<any> {
    const formData = new FormData();
    formData.append('RUT', data.rut);
    formData.append('Nombre', data.nombre);
    formData.append('Descripcion', data.descripcion || null);
    formData.append('Modalidad', data.modalidad);
    formData.append('Horario_Atencion', data.horario_Atencion);
    formData.append('Vigencia', 'true');
    if (imagen) {
      formData.append('Imagen', imagen);
    }

    return this.http.post<any>(this.apiUrl, formData);
  }

  subirMultimedia(idEmprendimiento: number, archivos: File[], descripcion = '', tipo = 'imagen'): Observable<any[]> {
    const requests = archivos.map((archivo) => {
      const formData = new FormData();
      formData.append('Archivo', archivo);
      formData.append('Tipo_Multimedia', tipo);
      formData.append('Descripcion', descripcion);

      return this.http.post(`${this.apiUrl}/${idEmprendimiento}/multimedia`, formData);
    });

    return forkJoin(requests);
  }

  //Editar emprendimiento

  obtenerEmprendimientoPorId(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  /*obtenerImagenPorId(id: number): Observable<string> {
    return this.http.get<{ url: string }>(`${this.apiUrl}/${id}/imagen-principal`)
    .pipe(
      map(response => response.url)
    );
  }

  obtenerMultimediaPorId(id: number): Observable<string[]> {
    return this.http.get<{ imagenes: string[] }>(`${this.apiUrl}/${id}/imagenes-emprendimiento`)
      .pipe(
        map(response => response.imagenes)
      );

  }*/

  actualizarEmprendimiento(id: number, formData: FormData): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, formData);
  }

  /*actualizarImagenPrincipal(id: number, imagen: File): Observable<any> {
    const formData = new FormData();
    formData.append('Imagen', imagen);

    return this.http.put<any>(`${this.apiUrl}/${id}/imagen-principal`, formData);
  }

  actualizarMultimedia(id: number, archivos: File[], descripcion = '', tipo = 'imagen'): Observable<any> {
    const formData = new FormData();
    archivos.forEach((archivo, index) => {
      formData.append('Imagenes', archivo, archivo.name);
    });
    formData.append('Descripcion', descripcion || '');
    return this.http.put(`${this.apiUrl}/${id}/imagenes-emprendimiento`, formData);
  }*/

}

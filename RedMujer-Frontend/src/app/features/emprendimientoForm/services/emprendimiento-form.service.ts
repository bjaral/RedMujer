import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmprendimientoFormService {

  private apiUrl = 'http://localhost:5145/api/Emprendimientos';

  constructor(private http: HttpClient) {}

  crearEmprendimiento(data: any, imagen: File): Observable<any> {
    const formData = new FormData();
    formData.append('RUT', data.rut);
    formData.append('Nombre', data.nombre);
    formData.append('Descripcion', data.descripcion || '');
    formData.append('Modalidad', data.modalidad);
    formData.append('Horario_Atencion', data.horario_Atencion);
    formData.append('Vigencia', 'true');
    formData.append('Imagen', imagen);

    return this.http.post<any>(this.apiUrl, formData);
  }

  subirMultimedia(idEmprendimiento: number, archivos: File[], descripcion = '', tipo = 'imagen'): Observable<any[]> {
    const uploadRequests = archivos.map((archivo) => {
      const formData = new FormData();
      formData.append('Archivo', archivo);
      formData.append('Tipo_Multimedia', tipo);
      formData.append('Descripcion', descripcion);

      return this.http.post(`${this.apiUrl}/${idEmprendimiento}/multimedia`, formData);
    });

    return new Observable(observer => {
      const results: any[] = [];
      let completed = 0;

      uploadRequests.forEach((req, index) => {
        req.subscribe({
          next: res => {
            results[index] = res;
            completed++;
            if (completed === uploadRequests.length) {
              observer.next(results);
              observer.complete();
            }
          },
          error: err => observer.error(err)
        });
      });
    });
  }
}

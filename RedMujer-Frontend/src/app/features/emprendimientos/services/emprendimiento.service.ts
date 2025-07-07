import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin, map, switchMap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmprendimientoService {

  constructor(private http: HttpClient) { }

  private url = 'http://localhost:5145/api'

  getByIdImg(id: number): Observable<any> {
    return this.http.get<any>(`${this.url}/Emprendimientos/${id}/imagenes-emprendimiento`);
  }

  getAll(): Observable<any> {
    return this.http.get<any>(`${this.url}/Emprendimientos`);
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.url}/Emprendimientos/${id}`);
  }

  getByCategory(category: string): Observable<any> {
    return this.http.get<any>(`${this.url}/Emprendimientos/categoria/${category}`);
  }

  // Obtener categorías de un emprendimiento específico
  getCategories(id: number): Observable<any> {
    return this.http.get<any>(`${this.url}/emprendimientos/${id}/categorias`);
  }

  // Obtener todos los emprendimientos con sus categorías
  getAllWithCategories(): Observable<any[]> {
  return this.getAll().pipe(
    switchMap((emprendimientos: any[]) => { // Use switchMap here
      // Crear un array de observables para obtener las categorías de cada emprendimiento
      const emprendimientosConCategorias = emprendimientos.map(emp => {
        return this.getCategories(emp.id).pipe(
          map(categorias => ({
            ...emp,
            categorias: categorias,
            categoriasTexto: categorias.map((cat: any) => cat.nombre).join(', ')
          }))
        );
      });

      // Ejecutar todas las peticiones en paralelo y retornar el resultado aplanado
      return forkJoin(emprendimientosConCategorias);
    })
    // No se necesita un segundo map para aplanar, switchMap ya se encarga de ello.
  );
}

  crearEmprendimiento(data: any, imagen: File | null): Observable<any> {
    const formData = new FormData();
    formData.append('RUT', data.rut);
    formData.append('Nombre', data.nombre);
    formData.append('Descripcion', data.descripcion || '');
    formData.append('Modalidad', data.modalidad);
    formData.append('Horario_Atencion', data.horario_Atencion);
    formData.append('Vigencia', 'true');
    if (imagen) {
      formData.append('Imagen', imagen);
    }

    return this.http.post<any>(this.url, formData);
  }
}
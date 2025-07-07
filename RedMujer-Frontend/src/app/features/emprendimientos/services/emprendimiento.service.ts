import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin, map, switchMap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmprendimientoService {

  constructor(private http: HttpClient) { }

  private url = 'http://localhost:5145/api'

  getAll(): Observable<any> {
    return this.http.get<any>(`${this.url}/Emprendimientos`);
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

  getUbicacionDeEmprendimiento(idEmprendimiento: number): Observable<any> {
  return this.http.get<any>(`${this.url}/EmprendimientoUbicacion`).pipe(
    map((ubicacionRelaciones: any[]) => {
      // Buscar la relación entre el emprendimiento y su ubicación
      const ubicacionRelacion = ubicacionRelaciones.find(rel => rel.idEmprendimiento === idEmprendimiento);
      if (!ubicacionRelacion) {
        throw new Error('No se encontró la relación entre emprendimiento y ubicación');
      }
      
      const idComuna = ubicacionRelacion.idUbicacion;
      return idComuna; // Devolver solo el id de la comuna para seguir el flujo
    }),
    switchMap((idComuna: number) => {
      // Obtener la comuna y luego la región usando `switchMap`
      return this.getComunaById(idComuna).pipe(
        switchMap((comuna: any) => {
          const idRegion = comuna.id_Region;
          return this.getRegionById(idRegion).pipe(
            map((region: any) => ({
              comuna: comuna.nombre,
              region: region.nombre
            }))
          );
        })
      );
    })
  );
}

  getComunaById(idComuna: number): Observable<any> {
    return this.http.get<any>(`${this.url}/Comunas/${idComuna}`);
  }

  getRegionById(idRegion: number): Observable<any> {
    return this.http.get<any>(`${this.url}/Regiones/${idRegion}`);
  }
}
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin, map, switchMap, of, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmprendimientoService {

  constructor(private http: HttpClient) { }

  private url = 'http://localhost:5145/api'

  getByIdRedes(id: number): Observable<any> {
    return this.http.get<any>(`${this.url}/Contactos/emprendimiento/${id}/`);
  }

  getContactoById(id: number): Observable<any> {
    return this.http.get<any>(`${this.url}/Emprendimientos/${id}/contactos`);
  }

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

  getCategories(id: number): Observable<any> {
    return this.http.get<any>(`${this.url}/emprendimientos/${id}/categorias`);
  }

  getAllWithCategories(): Observable<any[]> {
    return this.getAll().pipe(
      switchMap((emprendimientos: any[]) => {
        const emprendimientosConCategorias = emprendimientos.map(emp => {
          return this.getCategories(emp.id).pipe(
            map(categorias => ({
              ...emp,
              categorias: categorias,
              categoriasTexto: categorias.map((cat: any) => cat.nombre).join(', ')
            }))
          );
        });

        return forkJoin(emprendimientosConCategorias);
      })
    );
  }

  getUbicacionDeEmprendimiento(idEmprendimiento: number): Observable<any> {
    return this.http.get<any>(`http://localhost:5145/emprendimientos/${idEmprendimiento}/ubicaciones`).pipe(
      switchMap((ubicaciones: any[]) => {
        const ubicacionesVigentes = ubicaciones.filter(ubicacion => ubicacion.vigencia === true);

        if (ubicacionesVigentes.length === 0) {
          return of({
            comuna: 'No especificada',
            region: 'No especificada',
            id_Comuna: null,
            id_Region: null
          });
        }

        const ubicacion = ubicacionesVigentes[0];

        return forkJoin({
          comuna: this.getComunaById(ubicacion.id_Comuna),
          region: this.getRegionById(ubicacion.id_Region)
        }).pipe(
          map(({ comuna, region }) => {
            return {
              comuna: comuna.nombre,
              region: region.nombre,
              id_Comuna: ubicacion.id_Comuna,
              id_Region: ubicacion.id_Region,
              calle: ubicacion.calle,
              numero: ubicacion.numero
            };
          })
        );
      }),
      catchError(error => {
        console.error(`Error al obtener ubicaciones para emprendimiento ${idEmprendimiento}:`, error);

        return of({
          comuna: 'No especificada',
          region: 'No especificada',
          id_Comuna: null,
          id_Region: null
        });
      })
    );
  }

  getComunaById(idComuna: number): Observable<any> {
    return this.http.get<any>(`${this.url}/Comunas/${idComuna}`);
  }

  getRegionById(idRegion: number): Observable<any> {
    return this.http.get<any>(`${this.url}/Regiones/${idRegion}`);
  }

  getCategoriasByEmprendimiento(idEmprendimiento: number): Observable<any> {
    return this.http.get<any>(`http://localhost:5145/emprendimientos/${idEmprendimiento}/categorias`);
  }

  getPlataformasByEmprendimiento(idEmprendimiento: number): Observable<any[]> {
    return this.http.get<any[]>(`http://localhost:5145/emprendimientos/${idEmprendimiento}/plataformas`).pipe(
      switchMap((plataformas: any[]) => {
        const plataformasVigentes = plataformas.filter(plataforma => plataforma.vigencia === true);

        if (plataformasVigentes.length === 0) {
          return of([{
            ruta: 'No especificada',
            descripcion: 'No especificada',
            tipo_plataforma: 'No especificada',
          }]);
        }

        const plataformasFormateadas = plataformasVigentes.map(plataforma => ({
          ruta: plataforma.ruta,
          descripcion: plataforma.descripcion,
          tipo_plataforma: plataforma.tipo_Plataforma
        }));

        return of(plataformasFormateadas);
      }),
      catchError(error => {
        console.error(`Error al obtener plataformas para emprendimiento ${idEmprendimiento}:`, error);

        return of([{
          ruta: 'No especificada',
          descripcion: 'No especificada',
          tipo_plataforma: 'No especificada',
        }]);
      })
    );
  }

  obtenerMultimediaPorId(id: number): Observable<string[]> {
    return this.http.get<{ imagenes: string[] }>(`${this.url}/Emprendimientos/${id}/imagenes-emprendimiento`)
      .pipe(map(response => response.imagenes));
  }

}
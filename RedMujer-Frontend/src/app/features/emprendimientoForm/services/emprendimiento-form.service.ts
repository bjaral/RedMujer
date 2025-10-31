import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { forkJoin, Observable, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EmprendimientoFormService {

  private apiUrl = 'http://localhost:5145/api/Emprendimientos';
  private api = 'http://localhost:5145/api/';

  constructor(private http: HttpClient) { }

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

  // Contactos
  crearContacto(contacto: any): Observable<any> {
    return this.http.post<any>(`${this.api}Contactos`, contacto);
  }

  obtenerContactosPorEmprendimiento(idEmprendimiento: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}Contactos/emprendimiento/${idEmprendimiento}`);
  }

  actualizarContacto(id: number, contacto: any): Observable<any> {
    return this.http.put<any>(`${this.api}Contactos/${id}`, contacto);
  }

  eliminarContacto(id: number): Observable<any> {
    return this.http.delete<any>(`${this.api}Contactos/${id}`);
  }

  // Categorías
  obtenerCategorias(): Observable<any[]> {
    return this.http.get<any[]>(`http://localhost:5145/categorias`);
  }

  crearEmprendimientoCategoria(empCat: any): Observable<any> {
    return this.http.post<any>(`${this.api}EmprendimientoCategoria`, empCat);
  }

  obtenerCategoriasPorEmprendimiento(idEmprendimiento: number): Observable<any[]> {
    return this.http.get<any[]>(`http://localhost:5145/emprendimientos/${idEmprendimiento}/categorias`);
  }

  eliminarEmprendimientoCategoria(idCategoria: number, idEmprendimiento: number): Observable<any> {
    return this.http.delete<any>(`${this.api}EmprendimientoCategoria/${idCategoria}/${idEmprendimiento}`);
  }

  // Plataformas
  crearPlataforma(plataforma: any): Observable<any> {
    return this.http.post<any>(`${this.api}Plataforma`, plataforma);
  }

  obtenerPlataformasPorEmprendimiento(idEmprendimiento: number): Observable<any[]> {
    return this.http.get<any[]>(`http://localhost:5145/emprendimientos/${idEmprendimiento}/plataformas`);
  }

  actualizarPlataforma(id: number, plataforma: any): Observable<any> {
    return this.http.put<any>(`${this.api}Plataforma/${id}`, plataforma);
  }

  eliminarPlataforma(id: number): Observable<any> {
    return this.http.delete<any>(`${this.api}Plataforma/${id}`);
  }

  // Ubicaciones
  crearUbicacion(ubicacion: any): Observable<any> {
    return this.http.post<any>(`${this.api}Ubicaciones`, ubicacion);
  }

  obtenerUbicacionPorId(id: number): Observable<any> {
    return this.http.get<any>(`${this.api}Ubicaciones/${id}`);
  }

  actualizarUbicacion(id: number, ubicacion: any): Observable<any> {
    return this.http.put<any>(`${this.api}Ubicaciones/${id}`, ubicacion);
  }

  crearEmprendimientoUbicacion(empUbicacion: any): Observable<any> {
    return this.http.post<any>(`${this.api}EmprendimientoUbicacion`, empUbicacion);
  }

  getUbicacionDeEmprendimiento(idEmprendimiento: number): Observable<any> {
    return this.http.get<any>(`http://localhost:5145/emprendimientos/${idEmprendimiento}/ubicaciones`).pipe(
      switchMap((ubicaciones: any[]) => {
        const ubicacionesVigentes = ubicaciones.filter(ubicacion => ubicacion.vigencia === true);

        if (ubicacionesVigentes.length === 0) {
          return of({
            id_Ubicacion: null,  // ← Agregado
            comuna: 'No especificada',
            region: 'No especificada',
            id_Comuna: null,
            id_Region: null,
            calle: '',
            numero: '',
            referencia: ''
          });
        }

        const ubicacion = ubicacionesVigentes[0];

        return forkJoin({
          comuna: this.getComunaById(ubicacion.id_Comuna),
          region: this.getRegionById(ubicacion.id_Region)
        }).pipe(
          map(({ comuna, region }) => {
            return {
              id_Ubicacion: ubicacion.id_Ubicacion,  // ← Agregado
              comuna: comuna.nombre,
              region: region.nombre,
              id_Comuna: ubicacion.id_Comuna,
              id_Region: ubicacion.id_Region,
              calle: ubicacion.calle,
              numero: ubicacion.numero,
              referencia: ubicacion.referencia
            };
          })
        );
      }),
      catchError(error => {
        console.error(`Error al obtener ubicaciones para emprendimiento ${idEmprendimiento}:`, error);

        return of({
          id_Ubicacion: null,  // ← Agregado
          comuna: '',
          region: '',
          id_Comuna: null,
          id_Region: null,
          calle: '',
          numero: '',
          referencia: ''
        });
      })
    );
  }

  getComunaById(idComuna: number): Observable<any> {
    return this.http.get<any>(`${this.api}Comunas/${idComuna}`);
  }

  getRegionById(idRegion: number): Observable<any> {
    return this.http.get<any>(`${this.api}Regiones/${idRegion}`);
  }

}
using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.services;
using RedMujer_Backend.repositories;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace RedMujer_Backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmprendimientosController : ControllerBase
    {
        private readonly IEmprendimientoService _service;
        private readonly IWebHostEnvironment _env;
        private readonly IMultimediaRepository _multimediaRepo;

        public EmprendimientosController(
            IEmprendimientoService service,
            IWebHostEnvironment env,
            IMultimediaRepository multimediaRepo)
        {
            _service = service;
            _env = env;
            _multimediaRepo = multimediaRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lista = await _service.GetAllAsync();
            return Ok(lista);
        }

        [HttpGet("random/{cantidad}")]
        public async Task<IActionResult> GetRandom(int cantidad)
        {
            var lista = await _service.GetRandomAsync(cantidad);
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Emprendimiento>> GetById(int id)
        {
            var emprendimiento = await _service.GetByIdAsync(id);
            if (emprendimiento == null)
            {
                return NotFound();
            }
            return Ok(emprendimiento);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] EmprendimientoDto dto)
        {
            // 1. Crear emprendimiento SIN imagen
            var nuevo = await _service.CrearAsync(dto, null);

            // 2. Si viene una imagen, la subes y asocias
            if (dto.Imagen != null)
            {
                var rutaImagen = await GuardarImagenPrincipal(nuevo.Id_Emprendimiento, dto.Imagen);
                // Actualiza sólo la imagen
                await _service.ActualizarImagenAsync(nuevo.Id_Emprendimiento, rutaImagen);
                nuevo.Imagen = rutaImagen; // Opcional: para devolver la ruta en la respuesta
            }

            return CreatedAtAction(nameof(GetById), new { id = nuevo.Id_Emprendimiento }, nuevo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromForm] EmprendimientoDto dto)
        {
            var rutaImagen = await GuardarImagenPrincipal(id, dto.Imagen);
            await _service.ActualizarAsync(id, dto, rutaImagen);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/multimedia")]
        public async Task<IActionResult> SubirMultimedia(int id, [FromForm] MultimediaUploadDto dto)
        {
            if (dto.Archivo == null || dto.Archivo.Length == 0)
                return BadRequest("No se recibió ningún archivo.");

            var existe = await _service.ExisteAsync(id);
            if (!existe)
                return NotFound("Emprendimiento no encontrado.");

            string subCarpeta = "imagenes_emprendimiento";
            var ruta = await GuardarArchivoMultimedia(id, dto.Archivo, subCarpeta);

            var multimedia = new Multimedia
            {
                Id_Emprendimiento = id,
                Tipo_Multimedia = TipoMultimedia.imagen,
                Ruta = ruta,
                Descripcion = dto.Descripcion,
                Vigencia = true
            };

            await _multimediaRepo.AgregarMultimediaAsync(multimedia);

            return Ok(new { ruta });
        }

        private async Task<string?> GuardarImagenPrincipal(int idEmprendimiento, IFormFile? imagen)
        {
            if (imagen == null || imagen.Length == 0)
                return null;

            var nombreArchivo = Path.GetFileName(imagen.FileName);
            var carpetaDestino = Path.Combine(_env.ContentRootPath, "media", "emprendimientos", idEmprendimiento.ToString(), "imagen_principal");
            Directory.CreateDirectory(carpetaDestino);

            var rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }

            // Retornamos la ruta relativa para almacenar en base de datos y usar en URLs
            return Path.Combine("emprendimientos", idEmprendimiento.ToString(), "imagen_principal", nombreArchivo).Replace("\\", "/");
        }

        private async Task<string> GuardarArchivoMultimedia(int idEmprendimiento, IFormFile archivo, string subCarpeta)
        {
            var nombreArchivo = Path.GetFileName(archivo.FileName);
            var carpetaDestino = Path.Combine(_env.ContentRootPath, "media", "emprendimientos", idEmprendimiento.ToString(), subCarpeta);
            Directory.CreateDirectory(carpetaDestino);

            var rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            return Path.Combine("emprendimientos", idEmprendimiento.ToString(), subCarpeta, nombreArchivo).Replace("\\", "/");
        }
    }
}

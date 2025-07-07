using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.services;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace RedMujer_Backend.controllers
{
    public class ImagenesUploadDto
    {
        public List<IFormFile> Imagenes { get; set; } = new();
    }

    public class ImagenUploadDto
    {
        public IFormFile? Imagen { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class EmprendimientosController : ControllerBase
    {
        private readonly IEmprendimientoService _service;
        private readonly IWebHostEnvironment _env;
        private readonly ICategoriaService _categoriaService;

        public EmprendimientosController(
            IEmprendimientoService service,
            IWebHostEnvironment env,
            ICategoriaService categoriaService)
        {
            _service = service;
            _env = env;
            _categoriaService = categoriaService;
        }

        // =========== GETS ===========
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
        public async Task<ActionResult<EmprendimientoDto>> GetById(int id)
        {
            var emprendimiento = await _service.GetByIdAsync(id);
            if (emprendimiento == null)
                return NotFound();
            return Ok(emprendimiento);
        }

        [HttpGet("modalidades")]
        public ActionResult<IEnumerable<string>> GetTiposModalidades()
        {
            var modalidades = Enum.GetNames(typeof(TipoModalidad));
            return Ok(modalidades);
        }


        // =========== CREAR ===========
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Crear([FromForm] EmprendimientoCreateDto dto)
        {
            var nuevo = await _service.CrearAsync(dto, null);
            if (dto.Imagen != null)
            {
                var rutaImagen = await GuardarImagenPrincipal(nuevo.Id_Emprendimiento, dto.Imagen);
                await _service.ActualizarImagenAsync(nuevo.Id_Emprendimiento, rutaImagen);
                nuevo.Imagen = rutaImagen;
            }
            var respuesta = new EmprendimientoDto
            {
                Id_Emprendimiento = nuevo.Id_Emprendimiento,
                RUT = nuevo.RUT,
                Nombre = nuevo.Nombre,
                Descripcion = nuevo.Descripcion,
                Modalidad = nuevo.Modalidad?.ToString() ?? "",
                Horario_Atencion = nuevo.Horario_Atencion,
                Vigencia = nuevo.Vigencia,
                Imagen = nuevo.Imagen
            };
            return CreatedAtAction(nameof(GetById), new { id = nuevo.Id_Emprendimiento }, respuesta);
        }

        // =========== ACTUALIZAR ===========
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Actualizar(int id, [FromForm] EmprendimientoCreateDto dto)
        {
            // Verifica si no se recibió ni imagen ni campos actualizables
            if (dto.Imagen == null && dto.EsVacio())
                return NoContent(); // No hacer nada si no hay datos

            string? rutaImagen = null;
            if (dto.Imagen != null)
                rutaImagen = await GuardarImagenPrincipal(id, dto.Imagen);

            await _service.ActualizarAsync(id, dto, rutaImagen);
            return NoContent();
        }

        // =========== ELIMINAR ===========
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }

        // =========== IMAGEN PRINCIPAL ===========
        [HttpGet("{id}/imagen-principal")]
        public async Task<IActionResult> GetImagenPrincipal(int id)
        {
            var ruta = await _service.ObtenerRutaImagenPrincipalAsync(id);
            if (string.IsNullOrEmpty(ruta))
                return NotFound("No hay imagen principal para este emprendimiento.");

            var urlRelativa = Path.Combine("media", ruta).Replace("\\", "/");
            var urlCompleta = $"{Request.Scheme}://{Request.Host}/{urlRelativa}";

            return Ok(new { url = urlCompleta });
        }

        [HttpPut("{id}/imagen-principal")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ActualizarImagenPrincipal(int id, [FromForm] ImagenUploadDto dto)
        {
            var imagen = dto.Imagen;
            if (imagen == null || imagen.Length == 0)
                return NoContent(); // No elimina ni modifica nada si no recibe archivo

            var existe = await _service.ExisteAsync(id);
            if (!existe)
                return NotFound();

            var rutaAnterior = await _service.ObtenerRutaImagenPrincipalAsync(id);
            if (!string.IsNullOrEmpty(rutaAnterior))
            {
                var rutaFisica = Path.Combine(_env.ContentRootPath, "media", rutaAnterior);
                if (System.IO.File.Exists(rutaFisica))
                    System.IO.File.Delete(rutaFisica);
            }

            var rutaNueva = await GuardarImagenPrincipal(id, imagen);
            await _service.ActualizarImagenPrincipalAsync(id, rutaNueva);

            return Ok(new { ruta = rutaNueva });
        }

        [HttpDelete("{id}/imagen-principal")]
        public async Task<IActionResult> EliminarImagenPrincipal(int id)
        {
            var ruta = await _service.ObtenerRutaImagenPrincipalAsync(id);
            if (string.IsNullOrEmpty(ruta))
                return NotFound("No hay imagen principal para este emprendimiento.");

            var rutaFisica = Path.Combine(_env.ContentRootPath, "media", ruta);
            if (System.IO.File.Exists(rutaFisica))
                System.IO.File.Delete(rutaFisica);

            await _service.ActualizarImagenPrincipalAsync(id, null);
            return NoContent();
        }

        // =========== IMÁGENES ADICIONALES ===========

        [HttpGet("{id}/imagenes-emprendimiento")]
        public IActionResult GetImagenesEmprendimiento(int id)
        {
            var carpeta = Path.Combine(_env.ContentRootPath, "media", "emprendimientos", id.ToString(), "imagenes_emprendimiento");
            var imagenes = new List<string>();

            if (Directory.Exists(carpeta))
            {
                var archivos = Directory.GetFiles(carpeta);
                foreach (var archivo in archivos)
                {
                    var nombreArchivo = Path.GetFileName(archivo);
                    var urlRelativa = Path.Combine("media", "emprendimientos", id.ToString(), "imagenes_emprendimiento", nombreArchivo).Replace("\\", "/");
                    var urlCompleta = $"{Request.Scheme}://{Request.Host}/{urlRelativa}";
                    imagenes.Add(urlCompleta);
                }
            }

            return Ok(new { imagenes });
        }

        [HttpDelete("{id}/imagenes-emprendimiento/{nombreArchivo}")]
        public IActionResult EliminarImagenEspecifica(int id, string nombreArchivo)
        {
            if (string.IsNullOrWhiteSpace(nombreArchivo))
                return BadRequest("Nombre de archivo inválido.");

            // Seguridad básica: evita path traversal
            nombreArchivo = Path.GetFileName(nombreArchivo);

            var carpeta = Path.Combine(_env.ContentRootPath, "media", "emprendimientos", id.ToString(), "imagenes_emprendimiento");
            var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

            if (!System.IO.File.Exists(rutaCompleta))
                return NotFound("No existe esa imagen para este emprendimiento.");

            System.IO.File.Delete(rutaCompleta);
            return NoContent();
        }

        // POST para agregar imágenes de emprendimiento (adicionales)
        [HttpPost("{id}/imagenes-emprendimiento")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SubirImagenesEmprendimiento(int id, [FromForm] ImagenesUploadDto dto)
        {
            var imagenes = dto.Imagenes;
            if (imagenes == null || imagenes.Count == 0)
                return BadRequest("No se recibieron archivos.");

            var existe = await _service.ExisteAsync(id);
            if (!existe)
                return NotFound();

            var carpeta = Path.Combine(_env.ContentRootPath, "media", "emprendimientos", id.ToString(), "imagenes_emprendimiento");
            Directory.CreateDirectory(carpeta);

            var rutas = new List<string>();
            foreach (var imagen in imagenes)
            {
                var nombreArchivo = Path.GetFileName(imagen.FileName);
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }

                var urlRelativa = Path.Combine("media", "emprendimientos", id.ToString(), "imagenes_emprendimiento", nombreArchivo).Replace("\\", "/");
                rutas.Add(urlRelativa);
            }

            return Created("", new { rutas });
        }

        [HttpPut("{id}/imagenes-emprendimiento")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ActualizarImagenesEmprendimiento(int id, [FromForm] ImagenesUploadDto dto)
        {
            var imagenes = dto.Imagenes;
            if (imagenes == null || imagenes.Count == 0)
                return NoContent(); // No elimina ni modifica nada si no recibe archivos

            var existe = await _service.ExisteAsync(id);
            if (!existe)
                return NotFound();

            var carpeta = Path.Combine(_env.ContentRootPath, "media", "emprendimientos", id.ToString(), "imagenes_emprendimiento");
            Directory.CreateDirectory(carpeta);

            var rutas = new List<string>();
            foreach (var imagen in imagenes)
            {
                var nombreArchivo = Path.GetFileName(imagen.FileName);
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }

                var urlRelativa = Path.Combine("media", "emprendimientos", id.ToString(), "imagenes_emprendimiento", nombreArchivo).Replace("\\", "/");
                rutas.Add(urlRelativa);
            }

            return Ok(new { rutas });
        }

        [HttpDelete("{id}/imagenes-emprendimiento")]
        public IActionResult EliminarImagenesEmprendimiento(int id)
        {
            var carpeta = Path.Combine(_env.ContentRootPath, "media", "emprendimientos", id.ToString(), "imagenes_emprendimiento");
            if (!Directory.Exists(carpeta))
                return NotFound("No hay imágenes para este emprendimiento.");

            var archivos = Directory.GetFiles(carpeta);
            foreach (var archivo in archivos)
                System.IO.File.Delete(archivo);

            return NoContent();
        }

        // =========== ENDPOINT DE CATEGORIAS DE EMPRENDIMIENTO ===========
        [HttpGet("/emprendimientos/{idEmprendimiento}/categorias")]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategoriasPorEmprendimiento(int idEmprendimiento)
        {
            var categorias = await _categoriaService.ObtenerCategoriasPorEmprendimientoAsync(idEmprendimiento);
            if (categorias == null || !categorias.Any())
                return NotFound("No se encontraron categorías para el emprendimiento.");
            return Ok(categorias);
        }

        // =========== MÉTODOS DE GUARDADO DE IMÁGENES ===========
        private async Task<string?> GuardarImagenPrincipal(int idEmprendimiento, IFormFile? imagen)
        {
            if (imagen == null || imagen.Length == 0)
                return null;

            var carpetaDestino = Path.Combine(_env.ContentRootPath, "media", "emprendimientos", idEmprendimiento.ToString(), "imagen_principal");
            Directory.CreateDirectory(carpetaDestino);

            var archivosAntiguos = Directory.GetFiles(carpetaDestino);
            foreach (var archivo in archivosAntiguos)
            {
                System.IO.File.Delete(archivo);
            }

            var nombreArchivo = Path.GetFileName(imagen.FileName);
            var rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }

            return Path.Combine("emprendimientos", idEmprendimiento.ToString(), "imagen_principal", nombreArchivo).Replace("\\", "/");
        }
    }
}

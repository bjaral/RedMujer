using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;

namespace RedMujer_Backend.services
{
    public class EmprendimientoService : IEmprendimientoService
    {
        private readonly IEmprendimientoRepository _repository;

        public EmprendimientoService(IEmprendimientoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EmprendimientoDto>> GetAllAsync()
        {
            var lista = await _repository.GetAllAsync();
            return lista.Select(e => new EmprendimientoDto
            {
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Modalidad = e.Modalidad.ToString(),
                Vigencia = e.Vigencia
            });
        }

        public async Task<IEnumerable<EmprendimientoDto>> GetRandomAsync(int cantidad)
        {
            var lista = await _repository.GetRandomAsync(cantidad);
            return lista.Select(e => new EmprendimientoDto
            {
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Modalidad = e.Modalidad.ToString(),
                Vigencia = e.Vigencia
            });
        }
    }
}

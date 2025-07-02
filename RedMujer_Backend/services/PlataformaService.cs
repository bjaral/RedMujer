using RedMujer_Backend.models;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.repositories;

namespace RedMujer_Backend.services
{
    public class PlataformaService
    {
        private readonly IPlataformaRepository _repository;

        public PlataformaService(IPlataformaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PlataformaDto>> GetAllAsync()
        {
            var plataformas = await _repository.GetAllAsync();
            return plataformas.Select(p => new PlataformaDto
            {
                Id_Plataforma = p.Id_Plataforma,
                Id_Emprendimiento = p.Id_Emprendimiento,
                Ruta = p.Ruta,
                Descripcion = p.Descripcion,
                Vigencia = p.Vigencia,
                Tipo_Plataforma = p.Tipo_Plataforma.ToString()
            });
        }

        public async Task<PlataformaDto?> GetByIdAsync(int id)
        {
            var p = await _repository.GetByIdAsync(id);
            if (p == null) return null;
            return new PlataformaDto
            {
                Id_Plataforma = p.Id_Plataforma,
                Id_Emprendimiento = p.Id_Emprendimiento,
                Ruta = p.Ruta,
                Descripcion = p.Descripcion,
                Vigencia = p.Vigencia,
                Tipo_Plataforma = p.Tipo_Plataforma.ToString()
            };
        }

        public async Task AddAsync(PlataformaDto dto)
        {
            var plataforma = new Plataforma
            {
                Id_Emprendimiento = dto.Id_Emprendimiento,
                Ruta = dto.Ruta,
                Descripcion = dto.Descripcion,
                Vigencia = dto.Vigencia,
                Tipo_Plataforma = Enum.Parse<Plataforma.TipoPlataforma>(dto.Tipo_Plataforma)
            };
            await _repository.InsertAsync(plataforma);
        }

        public async Task UpdateAsync(int id, PlataformaDto dto)
        {
            var plataforma = new Plataforma
            {
                Id_Plataforma = id,
                Id_Emprendimiento = dto.Id_Emprendimiento,
                Ruta = dto.Ruta,
                Descripcion = dto.Descripcion,
                Vigencia = dto.Vigencia,
                Tipo_Plataforma = Enum.Parse<Plataforma.TipoPlataforma>(dto.Tipo_Plataforma)
            };
            await _repository.UpdateAsync(plataforma);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

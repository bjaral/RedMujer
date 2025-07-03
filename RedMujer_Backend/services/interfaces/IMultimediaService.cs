using RedMujer_Backend.models;
using System.Threading.Tasks;
using RedMujer_Backend.repositories;

namespace RedMujer_Backend.services
{
    public interface IMultimediaService
    {
        Task AgregarMultimediaAsync(Multimedia multimedia);
    }

    public class MultimediaService : IMultimediaService
    {
        private readonly IMultimediaRepository _repo;
        public MultimediaService(IMultimediaRepository repo)
        {
            _repo = repo;
        }

        public async Task AgregarMultimediaAsync(Multimedia multimedia)
        {
            await _repo.AgregarMultimediaAsync(multimedia);
        }
    }
}

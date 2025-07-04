using RedMujer_Backend.models;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public interface IMultimediaRepository
    {
        Task AgregarMultimediaAsync(Multimedia multimedia);
    }
}

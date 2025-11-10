using System.Security.Claims;
using System.Threading.Tasks;
using RedMujer_Backend.services;

namespace RedMujer_Backend.Utils
{
    public static class AuthHelpers
    {
        // Devuelve el id de usuario si está presente en los claims (NameIdentifier o "id")
        public static int? GetUserId(ClaimsPrincipal? user)
        {
            if (user == null)
                return null;

            var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(claim)) return null;
            return int.TryParse(claim, out var id) ? id : null;
        }

        // Llama al servicio para validar si el usuario es propietaria del emprendimiento.
        // Retorna true si es propietaria, false en caso contrario (incluye cuando userId es null).
        public static async Task<bool> IsOwnerAsync(IEmprendimientoService emprendimientoService, ClaimsPrincipal? user, int idEmprendimiento)
        {
            if (emprendimientoService == null) return false;
            var userId = GetUserId(user);
            if (userId == null) return false;
            return await emprendimientoService.EsPropietariaAsync(idEmprendimiento, userId.Value);
        }
    }
}

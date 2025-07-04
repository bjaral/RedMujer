using Dapper;
using Npgsql;
using System.Data;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class MultimediaRepository : IMultimediaRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public MultimediaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

        public async Task AgregarMultimediaAsync(Multimedia multimedia)
        {
            const string query = @"
                INSERT INTO ""Multimedias"" 
                    (id_emprendimiento, tipo_multimedia, ruta, descripcion, vigencia)
                VALUES 
                    (@Id_Emprendimiento, @Tipo_Multimedia::tipo_multimedia, @Ruta, @Descripcion, @vigencia)
            ";
            using var connection = CreateConnection();
            await connection.ExecuteAsync(query, new
            {
                multimedia.Id_Emprendimiento,
                Tipo_Multimedia = multimedia.Tipo_Multimedia.ToString(),
                multimedia.Ruta,
                multimedia.Descripcion,
                multimedia.Vigencia
            });
        }
    }
}

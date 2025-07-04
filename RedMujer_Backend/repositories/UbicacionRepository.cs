using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public class UbicacionRepository : IUbicacionRepository
    {
        private readonly IConfiguration _config;
        private readonly string? _connectionString;
        public UbicacionRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Ubicacion>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<Ubicacion>("SELECT * FROM \"Ubicaciones\" WHERE \"vigencia\" = true");
        }

        public async Task<Ubicacion?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Ubicacion>(
                "SELECT * FROM \"Ubicaciones\" WHERE \"id_ubicacion\" = @Id AND \"vigencia\" = true", new { Id = id });
        }

        public async Task<int> InsertAsync(Ubicacion ubicacion)
        {
            using var connection = new NpgsqlConnection(_connectionString);
<<<<<<< HEAD
            await connection.ExecuteAsync(
                "INSERT INTO \"Ubicaciones\" (\"id_comuna\", \"Id_Emprendimiento\", \"Calle\", \"Numero\", \"Referencia\", \"Vigencia\") " +
                "VALUES (@id_comuna, @Id_Emprendimiento, @Calle, @Numero, @Referencia, @Vigencia)", ubicacion);
=======
            return await connection.ExecuteScalarAsync<int>(
                "INSERT INTO \"Ubicaciones\" (\"id_comuna\", \"calle\", \"numero\", \"referencia\", \"vigencia\") " +
                "VALUES (@Id_Comuna, @Calle, @Numero, @Referencia, @vigencia) RETURNING \"id_ubicacion\";", ubicacion);
>>>>>>> 848ffadbbde22cf0434e7a5441d434c236e670d5
        }

        public async Task UpdateAsync(Ubicacion ubicacion)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
<<<<<<< HEAD
                "UPDATE \"Ubicaciones\" SET \"id_comuna\" = @id_comuna, \"Id_Emprendimiento\" = @Id_Emprendimiento, " +
                "\"Calle\" = @Calle, \"Numero\" = @Numero, \"Referencia\" = @Referencia, \"Vigencia\" = @Vigencia " +
                "WHERE \"Id_Ubicacion\" = @Id_Ubicacion", ubicacion);
=======
                "UPDATE \"Ubicaciones\" SET \"id_comuna\" = @Id_Comuna," +
                "\"calle\" = @Calle, \"numero\" = @Numero, \"referencia\" = @Referencia, \"vigencia\" = @vigencia " +
                "WHERE \"id_ubicacion\" = @Id_Ubicacion", ubicacion);
>>>>>>> 848ffadbbde22cf0434e7a5441d434c236e670d5
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE \"Ubicaciones\" SET \"vigencia\" = false WHERE \"id_ubicacion\" = @Id", new { Id = id });
        }
    }
}

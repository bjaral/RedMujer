using Dapper;
using Npgsql;
using RedMujer_Backend.models;

public class MultimediaRepository : IMultimediaRepository
{
    private readonly string _connectionString;

    public MultimediaRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private NpgsqlConnection CrearConexion() => new NpgsqlConnection(_connectionString);

    // Obtener multimedia por ID
    public async Task<Multimedia?> GetByIdAsync(int idMultimedia)
    {
        using var connection = CrearConexion();
        return await connection.QueryFirstOrDefaultAsync<Multimedia>(
            "SELECT * FROM \"Multimedias\" WHERE id_multimedia = @Id", new { Id = idMultimedia });
    }

    // Listar multimedia por emprendimiento
    public async Task<IEnumerable<Multimedia>> GetByEmprendimientoIdAsync(int idEmprendimiento)
    {
        using var connection = CrearConexion();
        return await connection.QueryAsync<Multimedia>(
            "SELECT * FROM \"Multimedias\" WHERE id_emprendimiento = @Id", new { Id = idEmprendimiento });
    }

    // Agregar nuevo multimedia
    public async Task<int> AddAsync(Multimedia multimedia)
    {
        using var connection = CrearConexion();
        var id = await connection.ExecuteScalarAsync<int>(
            @"INSERT INTO ""Multimedias"" (id_emprendimiento, ruta, descripcion, vigencia, tipo_multimedia)
              VALUES (@Id_Emprendimiento, @Ruta, @Descripcion, @Vigencia, @Tipo_Multimedia)
              RETURNING id_multimedia;",
            multimedia);
        return id;
    }

    // Actualizar multimedia (solo descripción y vigencia)
    public async Task UpdateAsync(Multimedia multimedia)
    {
        using var connection = CrearConexion();
        await connection.ExecuteAsync(
            @"UPDATE ""Multimedias"" SET
                descripcion = @Descripcion,
                vigencia = @Vigencia
              WHERE id_multimedia = @Id_Multimedia;",
            multimedia);
    }

    // Eliminar multimedia por ID
    public async Task DeleteAsync(int idMultimedia)
    {
        using var connection = CrearConexion();
        await connection.ExecuteAsync(
            "DELETE FROM \"Multimedias\" WHERE id_multimedia = @Id", new { Id = idMultimedia });
    }
}

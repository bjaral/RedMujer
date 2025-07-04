using Dapper;
using Npgsql;
using RedMujer_Backend.models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Runtime.Serialization;
using System.Reflection;

namespace RedMujer_Backend.repositories
{
    public class RegistroRepository : IRegistroRepository
    {
        private readonly string _connectionString;

        public RegistroRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<IEnumerable<Registro>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var rows = await connection.QueryAsync<dynamic>(
                "SELECT * FROM \"Registros\"");

            // Mapear el enum manualmente para cada registro
            var lista = new List<Registro>();
            foreach (var r in rows)
            {
                lista.Add(new Registro
                {
                    IdRegistro = r.id_registro,
                    UsuarioId = r.id_usuario,
                    Fecha = r.fecha,
                    ValorActual = r.valor_actual,
                    TipoRegistro = ParseTipoRegistro(r.tipo_registro)
                });
            }
            return lista;
        }

        public async Task<Registro?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var r = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SELECT * FROM \"Registros\" WHERE \"id_registro\" = @Id",
                new { Id = id });

            if (r == null) return null;

            return new Registro
            {
                IdRegistro = r.id_registro,
                UsuarioId = r.id_usuario,
                Fecha = r.fecha,
                ValorActual = r.valor_actual,
                TipoRegistro = ParseTipoRegistro(r.tipo_registro)
            };
        }

        public async Task InsertAsync(Registro registro)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"INSERT INTO ""Registros"" 
                (""id_usuario"", ""fecha"", ""valor_actual"", ""tipo_registro"") 
                VALUES (@UsuarioId, @Fecha, @ValorActual, @TipoRegistro::tipo_registro)",
                new
                {
                    registro.UsuarioId,
                    registro.Fecha,
                    registro.ValorActual,
                    TipoRegistro = GetEnumMemberValue(registro.TipoRegistro)
                });
        }

        public async Task UpdateAsync(Registro registro)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"UPDATE ""Registros"" SET 
                    ""id_usuario"" = @UsuarioId,
                    ""fecha"" = @Fecha,
                    ""valor_actual"" = @ValorActual,
                    ""tipo_registro"" = @TipoRegistro::tipo_registro
                WHERE ""id_registro"" = @IdRegistro",
                new
                {
                    registro.UsuarioId,
                    registro.Fecha,
                    registro.ValorActual,
                    TipoRegistro = GetEnumMemberValue(registro.TipoRegistro),
                    registro.IdRegistro
                });
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "DELETE FROM \"Registros\" WHERE \"id_registro\" = @Id",
                new { Id = id });
        }

        // --- Métodos auxiliares ---
        private static string GetEnumMemberValue(TipoRegistro tipo)
        {
            var type = typeof(TipoRegistro);
            var memInfo = type.GetMember(tipo.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);
            return attributes.Length > 0
                ? ((EnumMemberAttribute)attributes[0]).Value
                : tipo.ToString();
        }

        private static TipoRegistro ParseTipoRegistro(string value)
        {
            foreach (var field in typeof(TipoRegistro).GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute)) as EnumMemberAttribute;
                if (attribute != null && attribute.Value == value)
                    return (TipoRegistro)field.GetValue(null);
            }
            throw new ArgumentException("Tipo de registro no válido: " + value);
        }
    }
}

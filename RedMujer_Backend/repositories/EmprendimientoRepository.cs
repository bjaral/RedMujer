using Dapper;
using Npgsql;
using System.Data;
using RedMujer_Backend.models;
using System.Runtime.Serialization;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RedMujer_Backend.repositories
{
    public class EmprendimientoRepository : IEmprendimientoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public EmprendimientoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);

        private string EnumToString<T>(T enumVal) where T : struct, Enum
        {
            var type = typeof(T);
            var memInfo = type.GetMember(enumVal.ToString());
            if (memInfo.Length > 0)
            {
                var attr = memInfo[0].GetCustomAttribute<EnumMemberAttribute>();
                if (attr != null)
                    return attr.Value!;
            }
            return enumVal.ToString();
        }

        private TipoModalidad? StringToModalidad(string? modalidad)
        {
            if (string.IsNullOrWhiteSpace(modalidad))
                return null;

            modalidad = modalidad.Trim();

            if (string.Equals(modalidad, "Presencial", StringComparison.OrdinalIgnoreCase))
                return TipoModalidad.Presencial;

            if (string.Equals(modalidad, "Online", StringComparison.OrdinalIgnoreCase))
                return TipoModalidad.Online;

            if (string.Equals(modalidad, "Presencial y Online", StringComparison.OrdinalIgnoreCase))
                return TipoModalidad.PresencialYOnline;

            return null;
        }

        public async Task<IEnumerable<Emprendimiento>> GetAllAsync()
        {
            const string query = "SELECT * FROM \"Emprendimientos\"";
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<dynamic>(query);

            return result.Select(e => new Emprendimiento
            {
                Id_Emprendimiento = e.id_emprendimiento,
                RUT = e.RUT,
                Nombre = e.nombre,
                Descripcion = e.descripcion,
                Horario_Atencion = e.horario_atencion,
                Vigencia = e.vigencia,
                Imagen = e.imagen,
                Modalidad = StringToModalidad(e.modalidad)
            });
        }

        // ---- MÉTODO MODIFICADO ----
        public async Task<int> InsertarEmprendimientoAsync(Emprendimiento e)
        {
            const string query = @"
                INSERT INTO public.""Emprendimientos""(
                    ""RUT"", nombre, descripcion, horario_atencion, vigencia, modalidad, imagen)
                VALUES (@RUT, @Nombre, @Descripcion, @Horario_Atencion, @Vigencia, @Modalidad::tipo_modalidad, @Imagen)
                RETURNING id_emprendimiento";

            using var connection = CreateConnection();
            var id = await connection.ExecuteScalarAsync<int>(query, new
            {
                e.RUT,
                e.Nombre,
                e.Descripcion,
                e.Horario_Atencion,
                e.Vigencia,
                Modalidad = e.Modalidad == null ? null : EnumToString(e.Modalidad.Value),
                e.Imagen
            });

            e.Id_Emprendimiento = id;
            return id;
        }
        // --------------------------

        public async Task ActualizarEmprendimientoAsync(Emprendimiento e)
        {
            const string query = @"
                UPDATE public.""Emprendimientos""
                SET 
                    ""RUT"" = @RUT,
                    nombre = @Nombre,
                    descripcion = @Descripcion,
                    horario_atencion = @Horario_Atencion,
                    vigencia = @Vigencia,
                    modalidad = @Modalidad::tipo_modalidad,
                    imagen = @Imagen
                WHERE id_emprendimiento = @Id_Emprendimiento";

            using var connection = CreateConnection();
            await connection.ExecuteAsync(query, new
            {
                e.RUT,
                e.Nombre,
                e.Descripcion,
                e.Horario_Atencion,
                e.Vigencia,
                Modalidad = e.Modalidad == null ? null : EnumToString(e.Modalidad.Value),
                e.Imagen,
                e.Id_Emprendimiento
            });
        }

        public async Task EliminarEmprendimientoAsync(int id)
        {
            const string query = @"DELETE FROM public.""Emprendimientos"" WHERE id_emprendimiento = @Id";

            using var connection = CreateConnection();
            await connection.ExecuteAsync(query, new { Id = id });
        }

        public async Task<Emprendimiento?> ObtenerPorIdAsync(int id)
        {
            const string query = @"SELECT * FROM ""Emprendimientos"" WHERE id_emprendimiento = @Id LIMIT 1";
            using var connection = CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<dynamic>(query, new { Id = id });

            if (result == null)
                return null;

            return new Emprendimiento
            {
                Id_Emprendimiento = result.id_emprendimiento,
                RUT = result.RUT,
                Nombre = result.nombre,
                Descripcion = result.descripcion,
                Horario_Atencion = result.horario_atencion,
                Vigencia = result.vigencia,
                Imagen = result.imagen,
                Modalidad = StringToModalidad(result.modalidad)
            };
        }

       public async Task<IEnumerable<Emprendimiento>> GetRandomAsync(int cantidad)
        {
            const string query = @"SELECT * FROM ""Emprendimientos""
                                WHERE vigencia = true
                                ORDER BY RANDOM()
                                LIMIT @cantidad";
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<dynamic>(query, new { cantidad });

            return result.Select(e => new Emprendimiento
            {
                Id_Emprendimiento = e.id_emprendimiento,
                RUT = e.RUT,
                Nombre = e.nombre,
                Descripcion = e.descripcion,
                Horario_Atencion = e.horario_atencion,
                Vigencia = e.vigencia,
                Imagen = e.imagen,
                Modalidad = StringToModalidad(e.modalidad)
            });
        }
    }
}

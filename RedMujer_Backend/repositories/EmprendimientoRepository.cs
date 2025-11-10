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
using System.Runtime.CompilerServices;

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
        {
            return new NpgsqlConnection(_connectionString);
        }

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

        // ==== CORREGIDO: acepta PresencialYOnline Y Presencial y Online ====
        private TipoModalidad? StringToModalidad(string? modalidad)
        {
            if (string.IsNullOrWhiteSpace(modalidad))
                return null;

            modalidad = modalidad.Trim();

            if (string.Equals(modalidad, "Presencial", StringComparison.OrdinalIgnoreCase))
                return TipoModalidad.Presencial;

            if (string.Equals(modalidad, "Online", StringComparison.OrdinalIgnoreCase))
                return TipoModalidad.Online;

            if (string.Equals(modalidad, "PresencialYOnline", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(modalidad, "Presencial y Online", StringComparison.OrdinalIgnoreCase))
                return TipoModalidad.PresencialYOnline;

            return null;
        }

        public async Task<Emprendimiento?> GetByIdAsync(int id)
        {
            const string query = @"
                SELECT id_emprendimiento, ""RUT"", nombre, descripcion, horario_atencion, vigencia, imagen, modalidad, video_url
                FROM public.""Emprendimientos""
                WHERE id_emprendimiento = @Id
		AND vigencia = true
                LIMIT 1;";

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
                Modalidad = StringToModalidad(result.modalidad),
                VideoUrl = result.video_url
            };
        }

        public async Task<IEnumerable<Emprendimiento>> GetAllAsync()
        {
            const string query = @"SELECT id_emprendimiento, ""RUT"", nombre, descripcion, horario_atencion, vigencia, imagen, modalidad, video_url
                                   FROM public.""Emprendimientos"" WHERE vigencia = true;";

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
                Modalidad = StringToModalidad(e.modalidad),
                VideoUrl = e.video_url
            });
        }

        public async Task<int> InsertarEmprendimientoAsync(Emprendimiento e)
        {
            const string query = @"
                INSERT INTO public.""Emprendimientos""(
                    ""RUT"", nombre, descripcion, horario_atencion, vigencia, modalidad, imagen, video_url)
                VALUES (@RUT, @Nombre, @Descripcion, @Horario_Atencion, @Vigencia, @Modalidad::tipo_modalidad, @Imagen, @VideoUrl)
                RETURNING id_emprendimiento;";

            using var connection = CreateConnection();
            var id = await connection.ExecuteScalarAsync<int>(query, new
            {
                e.RUT,
                e.Nombre,
                e.Descripcion,
                e.Horario_Atencion,
                e.Vigencia,
                Modalidad = e.Modalidad == null ? null : EnumToString(e.Modalidad.Value),
                e.Imagen,
                e.VideoUrl
            });

            e.Id_Emprendimiento = id;
            return id;
        }

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
                    imagen = @Imagen,
                    video_url = @VideoUrl
                WHERE id_emprendimiento = @Id_Emprendimiento;";

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
                e.VideoUrl,
                e.Id_Emprendimiento
            });
        }

        public async Task EliminarEmprendimientoAsync(int id)
        {
            const string query = @"UPDATE public.""Emprendimientos"" SET vigencia = false WHERE id_emprendimiento = @Id;";

            using var connection = CreateConnection();
            await connection.ExecuteAsync(query, new { Id = id });
        }

        public async Task<IEnumerable<Emprendimiento>> GetRandomAsync(int cantidad)
        {
            const string query = @"
                SELECT id_emprendimiento, ""RUT"", nombre, descripcion, horario_atencion, vigencia, imagen, modalidad, video_url
                FROM public.""Emprendimientos""
                WHERE vigencia = true
                ORDER BY RANDOM()
                LIMIT @cantidad;";

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
                Modalidad = StringToModalidad(e.modalidad),
                VideoUrl = e.video_url
            });
        }
        public async Task<string?> GetImagenPrincipalAsync(int id)
        {
            const string sql = @"SELECT imagen FROM public.""Emprendimientos"" WHERE id_emprendimiento = @Id";
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<string>(sql, new { Id = id });
            }
        }

        public async Task UpdateImagenPrincipalAsync(int id, string? ruta)
        {
            const string sql = @"UPDATE public.""Emprendimientos"" SET imagen = @Ruta WHERE id_emprendimiento = @Id";
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { Ruta = ruta, Id = id });
            }
        }
        // obtener los emprendimientos de una persona
        public async Task<IEnumerable<Emprendimiento>> GetByPersonaIdAsync(int id_Persona)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query = @"
                SELECT e.*
                FROM ""Emprendimientos"" e
                INNER JOIN ""Persona_emprendimiento"" pe ON e.""id_emprendimiento"" = pe.""id_emprendimiento""
                WHERE pe.""id_persona"" = @Id_Persona AND vigencia = true
            ";

            return await connection.QueryAsync<Emprendimiento>(query, new { Id_Persona = id_Persona });
        }

        public async Task<bool> EsPropietariaAsync(int idEmprendimiento, int idUsuario)
        {
            using var connection = CreateConnection();

            // Verificar si existe una persona activa asociada a ese usuario
            // Y que esa persona esté vinculada al emprendimiento
            var query = @"
                SELECT COUNT(*)
                FROM ""Persona_emprendimiento"" pe
                INNER JOIN ""Personas"" p ON pe.""id_persona"" = p.""id_persona""
                WHERE pe.""id_emprendimiento"" = @IdEmprendimiento 
                AND p.""id_usuario"" = @IdUsuario
                AND p.""vigencia"" = true;";

            var count = await connection.ExecuteScalarAsync<int>(query, new
            {
                IdEmprendimiento = idEmprendimiento,
                IdUsuario = idUsuario
            });

            return count > 0;
        }

        public async Task<IEnumerable<Persona>> GetPersonasByEmprendimientoIdAsync(int idEmprendimiento)
        {
            const string query = @"
                SELECT 
                    p.""id_persona"" as Id_Persona,
                    p.""id_ubicacion"" as Id_Ubicacion,
                    p.""id_usuario"" as Id_Usuario,
                    p.""RUN"",
                    p.""nombre"" as Nombre,
                    p.""primer_apellido"" as PrimerApellido,
                    p.""segundo_apellido"" as SegundoApellido,
                    p.""vigencia"" as Vigencia
                FROM public.""Personas"" p
                INNER JOIN public.""Persona_emprendimiento"" pe ON p.""id_persona"" = pe.""id_persona""
                WHERE pe.""id_emprendimiento"" = @IdEmprendimiento AND p.vigencia = true;";

            using var connection = CreateConnection();
            return await connection.QueryAsync<Persona>(query, new { IdEmprendimiento = idEmprendimiento });
        }
    }
}

using System;

namespace RedMujer_Backend.models
{
using System.Runtime.Serialization;

public enum TipoRegistro
{
    [EnumMember(Value = "inicio_sesion")]
    InicioSesion,
    [EnumMember(Value = "cierre_sesion")]
    CierreSesion,
    [EnumMember(Value = "creacion_cuenta")]
    CreacionCuenta,
    [EnumMember(Value = "actualizacion_perfil")]
    ActualizacionPerfil,
    [EnumMember(Value = "eliminacion_cuenta")]
    EliminacionCuenta,
    [EnumMember(Value = "creacion_emprendimiento")]
    CreacionEmprendimiento,
    [EnumMember(Value = "actualizacion_emprendimiento")]
    ActualizacionEmprendimiento,
    [EnumMember(Value = "eliminacion_emprendimiento")]
    EliminacionEmprendimiento,
    [EnumMember(Value = "carga_multimedia")]
    CargaMultimedia,
    [EnumMember(Value = "error")]
    Error
}

    public class Registro
    {
        public int Id_Registro { get; set; }
        public int Id_Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string? ValorActual { get; set; }
        public TipoRegistro TipoRegistro { get; set; } // <--- Aquí está el tipo de registro
    }
}


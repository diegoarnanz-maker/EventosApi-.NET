using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Dtos
{
    public class TipoResponseDto
    {
        public int IdTipo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
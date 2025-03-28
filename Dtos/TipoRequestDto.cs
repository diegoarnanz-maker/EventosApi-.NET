using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Dtos
{
    public class TipoRequestDto
    {
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }
    }
}
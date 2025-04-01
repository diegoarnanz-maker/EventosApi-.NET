using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Dtos
{
    public class ReservaUpdateDto
    {
        public int Cantidad { get; set; }
        public string? Observaciones { get; set; }
    }

}
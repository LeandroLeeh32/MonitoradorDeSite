using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoradorDeSite
{

    public class ResultadoMonitoramento
    {
        public string? Horario { get; set; }
        public string? Host { get; set; }
        public string? Status { get; set; }
        public object? Exception { get; set; }


    }
}

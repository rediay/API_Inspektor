using System;
using System.Collections.Generic;

namespace Inspektor_API_REST.Models
{
    public class ProcuraduriaData
    {
        public string? identification { get; set; }
        public string? name { get; set; }
        public string? num_siri { get; set; }
        public IEnumerable<ProcuraduriaDataSanciones>? sanciones { get; set; }
        public IEnumerable<ProcuraduriaDataDelitos>? delitos { get; set; }
        public IEnumerable<ProcuraduriaDataInstancias>? instancias { get; set; }
        public IEnumerable<ProcuraduriaDataEventos>? eventos { get; set; }
        public IEnumerable<ProcuraduriaInhabilidades>? inhabilidades { get; set; }
        public IEnumerable<ProcuraduriaDataInvestidura>? investiduras { get; set; }
    }
}
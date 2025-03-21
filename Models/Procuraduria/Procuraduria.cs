using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Inspektor_API_REST.Models
{
    public class Procuraduria
    {
        public String? html_response { get; set; }
        [JsonProperty("not_criminal_records")]
        public ProcuraduriaNotCriminalRecordsData not_criminal_records { get; set; }
        [JsonProperty("data")]
        public List<ProcuraduriaData>? data { get; set; }
    }
}
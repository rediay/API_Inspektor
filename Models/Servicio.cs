﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Servicio
    {
        public int IdServicio { get; set; }
        public string NombreServicio { get; set; }
        public bool Estado { get; set; }
        public string UrlSrv { get; set; }
    }
}

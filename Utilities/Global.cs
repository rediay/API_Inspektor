using Inspektor_API_REST.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspektor_API_REST.Utilities
{
    public class Global
    {
        public const string UserSession = "UserSession";

        public static String notIn(IEnumerable<Listas> info)
        {
            String cadena = "";
            if (info.Count() > 0)
            {
                int i = 0;
                foreach (Listas obj in info)
                {
                    i++;
                    cadena += obj.IdLista + (i < info.Count() ? ", " : "");
                }
            }
            return cadena.Trim();
        }
    }
}

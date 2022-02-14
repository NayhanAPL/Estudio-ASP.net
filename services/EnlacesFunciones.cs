using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace versión_5_asp.services
{
    public class EnlacesFunciones: IFunciones
    {
        public List<string> TodasLasPosibilidades() 
        {
            List<string> posibilidades = new List<string>() {
        "Crear tus propios trueques.",
        "Recibir las sugerencias que le ofrecemos para sus trueques.",
        "Iterar por todos los trueques que se encuentren activos para hacer su selección.",
        "Enviar ofertas de trueques a otros usuarios.",
        "Gestionar y mantenerce informado sobre sus ofertas enviadas.",
        "Recibir ofertas y gestionarlas hasta su finalización.",
        "Finalizar un TRUEQUE y comenzar la comunicación con otros usuarios.",
        "Recibir ayuda y leer la guía sobre TRUEQUE."
            };
            return posibilidades;
        }
        
    }
}

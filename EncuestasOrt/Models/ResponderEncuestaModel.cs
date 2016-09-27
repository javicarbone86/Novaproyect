using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncuestasOrt.Models
{


    public class ResponderEncuestaModel
    {
 
        public int IdEncuesta { get; set; }
        public IEnumerable<EncuestasOrt.Pregunta> preguntas { get; set; }

        public ICollection<EncuestasOrt.EncuestaRespuesta> respuestas { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncuestasOrt.Models
{
    public class GraficoPreguntaModel
    {
        public IEnumerable<Pregunta> preguntas { get; set; }

        public FiltrosEncuesta filtros { get; set; }
    }
}
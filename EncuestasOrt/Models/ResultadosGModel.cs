using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncuestasOrt.Models
{
    public class ResultadosPlantillasModel
    {
        public List<int> idEncuestaSegundoRango { set; get; }
        public List<int> idEncuesta { set; get; }
        public IEnumerable<EncuestasOrt.Pregunta> preguntas { set; get; }
        public Nullable<int> idPlantilla { set; get; }
    }

    public class ResultadosGModel
    {
        public ResultadosGModel()
        {

        }

        public int idEncuesta { set; get; }
        public IEnumerable<EncuestasOrt.Pregunta> preguntas { set; get; }
        public Nullable<DateTime> fechaDesde { set; get; }
        public Nullable<DateTime> fechaHasta { set; get; }
    }



}
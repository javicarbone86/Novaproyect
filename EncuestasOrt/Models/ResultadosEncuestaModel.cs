using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncuestasOrt.Models
{

    public class ResultadosEncuestaModel
    {
        public ResultadosEncuestaModel()
        {

        }


//        public IEnumerable<dynamic> respuestas { get; set; }

        public IEnumerable<DatosReporte> respuestas { get; set; }


    }


    public class DatosReporte
    {
        public int PreguntaId { get; set; }
        public string PreguntaDescripcion { get; set; }
        public int OpcionId { get; set; }
        public string OpcionValor { get; set; }

    }


}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EncuestasOrt.Models
{
    public class CrearEncuestaModel
    {
        public Encuesta encuesta { get; set; }
        public SelectList materias { get; set; }
        public SelectList cursos { get; set; }
        public SelectList tematicas { get; set; }
    }
}
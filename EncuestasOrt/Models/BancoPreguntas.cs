using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncuestasOrt.Models
{
    public class BancoPreguntas
    {

        public BancoPreguntas()
        {

        }

        public IEnumerable<EncuestasOrt.Pregunta> preguntas { get; set; }

        public FiltrosPregunta filtros { get; set; }

        public int idEncuesta { get; set; }

    }


    public class FiltrosPregunta
    {

        // Agregado por Gabriel el 22/07/2016
        public Nullable<int> opcionTematicaId { get; set; }
        public Nullable<int> opcionMateriaId { get; set; }
        public Nullable<int> opcionEncuestaId { get; set; }
        public Nullable<int> opcionEstado { get; set; }
        public Nullable<int> esPropia { get; set; }

        public string tematicaDescripcion { get; set; }
        public string materiaDescripcion { get; set; }
        public string opcionEncuestaDescripcion { get; set; }
        public string opcionEstadoDescripcion { get; set; }
        public string esPropiaDescripcion { get; set; }

        public IEnumerable<EncuestasOrt.Tematica> tematicas { get; set; }
        public IEnumerable<EncuestasOrt.Materia> materias { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncuestasOrt.Models
{

    public class MisEncuestasModel
    {
        public MisEncuestasModel()
        {

        }

        public IEnumerable<DatosEncuesta> encuestas { get; set; }

        public FiltrosEncuesta filtros { get; set; }

    }


    public class DatosEncuesta
    {

        public Encuesta encuesta { get; set;  }
        public IEnumerable<EncuestasOrt.Pregunta> preguntas { get; set; }

        // Agregado por Gabriel el 24/07/2016
        public Nullable<int> cantidadEncuestasRealizadas { get; set; }
        public Nullable<bool> puedoEditar { get; set; }
        public Nullable<bool>puedoVerEstadistica { get; set; }

    }

    public class FiltrosEncuesta
    {
       

        // Agregado por Gabriel el 22/07/2016
        public Nullable<int> opcionTematicaId { get; set; }
        public string  opcionCurso { get; set; }
        public Nullable<int> opcionMateriaId { get; set; }
        public Nullable<int> opcionEncuestaId { get; set; }
        public Nullable<int> opcionEstado { get; set; }
        public Nullable<int> esPropia { get; set; }
        
        public string tematicaDescripcion { get; set; }
        public string materiaDescripcion { get; set; }
        public string opcionEncuestaDescripcion { get; set; }
        public string opcionEstadoDescripcion { get; set; }
        public string esPropiaDescripcion { get; set; }
        public string cursoDescripcion { get; set; }
        
        public IEnumerable<EncuestasOrt.Tematica> tematicas { get; set; }
        public IEnumerable<EncuestasOrt.Materia> materias { get; set; }
        public IEnumerable<string> Curso { get; set; }

        public IEnumerable<string> TraerOrdenadoLosCursos(List<string> query)
        {

            int indice = 0;


            int total = query.Count();
            List<string> ListaDeCursos = new List<string>();
            string anterior = query[indice];

            while (indice < total)
            {
                while (indice < total && query[indice] != null)
                {
                    while (indice < total && query[indice] != null && anterior == query[indice])
                    {

                        indice++;

                    }
                    anterior = query[indice];
                    ListaDeCursos.Add(anterior);
                    indice++;

                }
                indice++;

            }

           return  ListaDeCursos;


        }
    }


}
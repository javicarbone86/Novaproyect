using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace EncuestasOrt
{

    [MetadataType(typeof(EncuestaMetadata))]
    public partial class Encuesta
    {
    }

    [MetadataType(typeof(PreguntaMetadata))]
    public partial class Pregunta
    {
    }
  


}
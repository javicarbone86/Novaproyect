
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------


namespace EncuestasOrt
{

using System;
    using System.Collections.Generic;
    
public partial class EncuestaRespuesta
{

    public int Id { get; set; }

    public int EncuestaID { get; set; }

    public int PreguntaID { get; set; }

    public Nullable<int> OpcionID { get; set; }

    public Nullable<int> EncuestaRealizadaID { get; set; }



    public virtual Encuesta Encuesta { get; set; }

    public virtual Opcion Opcion { get; set; }

    public virtual Pregunta Pregunta { get; set; }

    public virtual EncuestaRealizada EncuestaRealizada { get; set; }

}

}

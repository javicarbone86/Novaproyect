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
    
    public partial class Encuesta
    {
        public Encuesta()
        {
            this.EncuestaPregunta = new HashSet<EncuestaPregunta>();
            this.EncuestaRespuesta = new HashSet<EncuestaRespuesta>();
            this.EncuestaRealizada = new HashSet<EncuestaRealizada>();
        }
    
        public int Id { get; set; }
        public Nullable<int> MateriaID { get; set; }
        public string UsuarioID { get; set; }
        public string Descripcion { get; set; }
        public System.DateTime FechaHora { get; set; }
        public string ClaveAcceso { get; set; }
        public bool EsTemplate { get; set; }
        public bool Estado { get; set; }
        public Nullable<int> TemplateID { get; set; }
        public string Curso { get; set; }
        public Nullable<int> TematicaID { get; set; }
        public Nullable<int> Cantidad { get; set; }
    
        public virtual Materia Materia { get; set; }
        public virtual ICollection<EncuestaPregunta> EncuestaPregunta { get; set; }
        public virtual ICollection<EncuestaRespuesta> EncuestaRespuesta { get; set; }
        public virtual Tematica Tematica { get; set; }
        public virtual ICollection<EncuestaRealizada> EncuestaRealizada { get; set; }
    }
}

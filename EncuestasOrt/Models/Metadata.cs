using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;



namespace EncuestasOrt
{
    public class EncuestaMetadata
    {
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Materia")]
        public int MateriaID { get; set; }
        [Display(Name = "Usuario")]
        public int UsuarioID { get; set; }
        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Required]
        [Display(Name = "Fecha")]
        public System.DateTime FechaHora { get; set; }
        [Required]
        [Display(Name = "Clave de acceso")]
        public string ClaveAcceso { get; set; }
        [Required]
        [Display(Name = "Cantidad máxima de encuestados")]
        public string Cantidad { get; set; }

        [Display(Name = "Guardar como plantilla")]
        public bool EsTemplate { get; set; }
        [Display(Name = "Activo")]
        public bool Estado { get; set; }
        public Nullable<int> TemplateID { get; set; }
        [Display(Name = "Curso")]
        public string Curso { get; set; }
        public virtual Materia Materia { get; set; }
        

    }

    public class PreguntaMetadata
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Required]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }
        [Display(Name = "Obligatoria")]
        public bool Requerido { get; set; }
        [Display(Name = "Valor Minimo")]
        public Nullable<int> ValorMinimo { get; set; }
        [Display(Name = "Valor Maximo")]
        public Nullable<int> ValorMaximo { get; set; }
        [Display(Name = "Valor Normal Inicial")]
        public Nullable<int> ValorNormalInicial { get; set; }
        [Display(Name = "Valor Normal Final")]
        public Nullable<int> ValorNormalFinal { get; set; }
        [Display(Name = "Activa")]
        public Nullable<bool> EstadoInactivo { get; set; }
        [Display(Name = "Compartir")]
        public Nullable<bool> Compartir { get; set; }
        public string UsuarioID { get; set; }
        
    }






}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EncuestasOrt.Models
{
    public class CreateFromTemplateViewModel
    {

        public int Id { get; set; }

        [DisplayName("Templates")]
        public SelectList plantillas { get; set; }
        
        public int selectedTemplate { get; set; }

        [DisplayName("Materia")]
        public int MateriaID { get; set; }
        
        public int UsuarioID { get; set; }

        [DisplayName("Titulo")]
        public string Descripcion { get; set; }

        [DisplayName("Fecha")]
        public System.DateTime FechaHora { get; set; }

        [Required]
        [DisplayName("Clave de acceso")]
        public string ClaveAcceso { get; set; }
        
        public bool EsTemplate { get; set; }

        [DisplayName("Activo")]
        public bool Estado { get; set; }
        
        public Nullable<int> TemplateID { get; set; }

        public virtual Materia Materia { get; set; }
        

    }
}
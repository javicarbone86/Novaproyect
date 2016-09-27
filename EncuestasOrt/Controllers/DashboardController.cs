using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EncuestasOrt.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace EncuestasOrt.Controllers
{
    public class DashboardController : Controller
    {
        private EncuestasContainer db = new EncuestasContainer();


        [Authorize]
        public ActionResult Index()
        {
            DashboardModel model = new DashboardModel();
            DateTime todayDate = DateTime.Today;

            //encuestas
            model.Num_Encuestas = db.Encuesta.Where(c => c.EsTemplate == false).Count();

            // plantillas
            model.Num_Plantillas = db.Encuesta.Where(c => c.EsTemplate == true).Count();

            // respuestas
            model.Num_Respuestas = db.EncuestaRespuesta.GroupBy(i => i.EncuestaID).Count();

            // preguntas
            model.Num_Preguntas = db.Pregunta.Count();

            // preguntas
            model.test = 11;
            return View(model);
        }
    }
}

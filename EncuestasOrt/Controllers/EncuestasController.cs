using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EncuestasOrt;
using EncuestasOrt.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;
using PagedList;
using System.Globalization;

namespace EncuestasOrt.Controllers
{
    public class EncuestasController : Controller
    {
        private EncuestasContainer db = new EncuestasContainer();

        // GET: Encuestas
        public ActionResult IndexOld()
        {

            //   string currentUserId = User.Identity.GetUserId();

            var encuestas = db.Encuesta.Include(e => e.Materia);
            return View(encuestas.ToList());

        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           DatosEncuesta encuesta = (from e in db.Encuesta
                                      where (e.Id == id)
                                      select new DatosEncuesta
                                      {
                                          encuesta = e,
                                          preguntas = (from p in db.Pregunta
                                                       join q2 in db.EncuestaPregunta on p.Id equals q2.PreguntaID
                                                       where q2.EncuestaID == e.Id
                                                      orderby q2.Id ascending
                                                        select p
                                                           ).ToList()
                                                          
                                                          // .AsEnumerable().OrderBy(a => a.EncuestaPregunta)


                                      }).SingleOrDefault();
            

           


            if (encuesta == null)
            {
                return HttpNotFound();
            }

            return View(encuesta);
        }

        // GET: Encuestas/Create
        [Authorize]
        public ActionResult Create()
        {
            CrearEncuestaModel cencuesta_model = new CrearEncuestaModel();
            cencuesta_model.encuesta = new Encuesta();
            cencuesta_model.materias = new SelectList(db.Materia, "Id", "Descripcion");

            cencuesta_model.tematicas = new SelectList(db.Tematica, "Id", "Descripcion");


            cencuesta_model.cursos = new SelectList(new List<SelectListItem>
{
    new SelectListItem { Selected = true, Text = string.Empty, Value = ""},
    new SelectListItem { Selected = false, Text = "Curso A", Value = "A"},
    new SelectListItem { Selected = false, Text = "Curso B", Value = "B"},
    new SelectListItem { Selected = false, Text = "Curso C", Value = "C"},
    new SelectListItem { Selected = false, Text = "Curso D", Value = "D"},
    new SelectListItem { Selected = false, Text = "Curso E", Value = "E"},
    new SelectListItem { Selected = false, Text = "Curso F", Value = "F"},
    new SelectListItem { Selected = false, Text = "Curso G", Value = "G"},

}, "Value", "Text", 1);

            cencuesta_model.encuesta.Estado = true;
            cencuesta_model.encuesta.FechaHora = System.DateTime.Now;


            string currentUserId = User.Identity.GetUserId();
            //guardo el usuario
            if (currentUserId != null)
            {
                cencuesta_model.encuesta.UsuarioID = currentUserId.ToString();
            }
            else
            {
                cencuesta_model.encuesta.UsuarioID = "1";
            }


            // TODO agregar usuarioID y el flag de global


            return View(cencuesta_model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // public ActionResult Create([Bind(Include = "Id,MateriaID,UsuarioID,Descripcion,FechaHora,ClaveAcceso,Estado")] Encuesta encuesta)
        public ActionResult Create(CrearEncuestaModel modelo)
        {
            if (ModelState.IsValid)
            {
                if (modelo.encuesta.MateriaID != null)
                    modelo.encuesta.TematicaID = (from m in db.Materia where m.Id == modelo.encuesta.MateriaID select m.TematicaID).FirstOrDefault();

                modelo.encuesta.FechaHora = DateTime.Now;
                modelo.encuesta.UsuarioID = User.Identity.GetUserId();
                db.Encuesta.Add(modelo.encuesta);
                //db.SaveChanges();

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    TempData["error"] = "error al crear la encuesta" + e.ToString();
                    return RedirectToAction("Index");
                }

                return RedirectToAction("AsignarPreguntas", new { id = modelo.encuesta.Id });
            }

            return View(modelo);
        }

        // GET: Encuestas/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            bool puedoEditar = false;
            string currentUserId = User.Identity.GetUserId();
            puedoEditar = (from n in db.Encuesta where n.Id == id && n.UsuarioID == currentUserId select true).FirstOrDefault();
            if (!puedoEditar)
            {
                TempData["error"] = "Usted no puede editar la encuesta";
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrearEncuestaModel modelo = new CrearEncuestaModel();
            modelo.encuesta = db.Encuesta.Find(id);
            if (modelo.encuesta == null)
            {
                return HttpNotFound();
            }
            modelo.cursos = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = string.Empty, Value = ""},
                new SelectListItem { Selected = false, Text = "Curso A", Value = "A"},
                new SelectListItem { Selected = false, Text = "Curso B", Value = "B"},
                new SelectListItem { Selected = false, Text = "Curso C", Value = "C"},
                new SelectListItem { Selected = false, Text = "Curso D", Value = "D"},
                new SelectListItem { Selected = false, Text = "Curso E", Value = "E"},
                new SelectListItem { Selected = false, Text = "Curso F", Value = "F"},
                new SelectListItem { Selected = false, Text = "Curso G", Value = "G"},

            }, "Value", "Text", 1);
            ViewBag.materias = new SelectList(db.Materia, "Id", "Descripcion", modelo.encuesta.MateriaID);
            ViewBag.tematicas = new SelectList(db.Tematica, "Id", "Descripcion", modelo.encuesta.TematicaID);

            return View(modelo);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Encuesta encuesta)
        {
            if (ModelState.IsValid)
            {
                //if (encuesta.TemplateID != null)
                //    encuesta.TemplateID = (from m in db.Materia where m.Id == encuesta.MateriaID select m.TematicaID).FirstOrDefault();

                encuesta.FechaHora = (from e in db.Encuesta where e.Id == encuesta.Id select e.FechaHora).FirstOrDefault();
                encuesta.UsuarioID = (from e in db.Encuesta where e.Id == encuesta.Id select e.UsuarioID).FirstOrDefault();

                if (encuesta.MateriaID != null)
                    encuesta.TematicaID = (from m in db.Materia where m.Id == encuesta.MateriaID select m.TematicaID).FirstOrDefault();

                db.Entry(encuesta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MateriaID = new SelectList(db.Materia, "Id", "Descripcion", encuesta.MateriaID);
            return View(encuesta);
        }

        // GET: Encuestas/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            bool puedoEditar = false;
            string currentUserId = User.Identity.GetUserId();
            puedoEditar = (from n in db.Encuesta where n.Id == id && n.UsuarioID == currentUserId select true).FirstOrDefault();
            if (!puedoEditar)
            {
                TempData["error"] = "Usted no puede eliminar la encuesta";
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encuesta encuesta = db.Encuesta.Find(id);
            if (encuesta == null)
            {
                return HttpNotFound();
            }
            return View(encuesta);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //preguntas relacionadas 
            db.EncuestaPregunta.RemoveRange(db.EncuestaPregunta.Where(c => c.EncuestaID == id));
            //respuestas relacionadas
            db.EncuestaRespuesta.RemoveRange(db.EncuestaRespuesta.Where(c => c.EncuestaID == id));

            Encuesta encuesta = db.Encuesta.Find(id);
            db.Encuesta.Remove(encuesta);
            db.SaveChanges();

            TempData["success"] = "Encuesta eliminada";

            return RedirectToAction("Index");
        }

        /*
         *  Customs
         * 
         */

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Acceder(int id, string ClaveAcceso)
        {

            var encu = db.Encuesta.FirstOrDefault(a => a.Id == id);

            if (encu == null)
            {
                TempData["error"] = "Encuesta no encontrada.";

                return RedirectToAction("Index", "Home");
            }
            else
            {

                if (encu.Estado == false)
                {
                    TempData["warning"] = "encuesta deshabilitada";
                    return RedirectToAction("Index", "Home");

                }

                // reviso cookie
                string nroEncu = CookieStore.GetCookie(id.ToString());
                if (nroEncu != "")
                {
                    TempData["warning"] = "usted ya respondió a esta encuesta";
                    return RedirectToAction("Index", "Home");
                }



                if (encu.ClaveAcceso == ClaveAcceso)
                {
                    TempData["encuestaID"] = id;
                    return RedirectToAction("Responder", "Encuestas");

                }
                else
                {
                    TempData["error"] = "Contraseña incorrecta.";
                    return RedirectToAction("Index", "Home");
                }

            }

        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Responder()
        {
            if (TempData["encuestaID"] != null)
            {
                var surveyid = (int)TempData["encuestaID"];

                // Modificado por Gabriel el 27/07/2017
                //var cantidad_respondidas = db.EncuestaRespuesta.Where(c => c.EncuestaID == surveyid).Count();
                //var cantidad_preguntas = db.EncuestaPregunta.Where(c => c.EncuestaID == surveyid).Count();
                //if(cantidad_preguntas != 0){
                //    var division = cantidad_respondidas / cantidad_preguntas;
                //    var cant_permitida = db.Encuesta.First(a => a.Id == surveyid).Cantidad;
                //    if (division >= cant_permitida) {
                //        TempData["error"] = "Cantidad de encuestas realizadas alcanzadas";
                //        return RedirectToAction("Index","Home");
                //    }
                //}

                // Modificado por Gabriel el 27/07/2017
                // Controlar que no se exceda en la cantidad de encuestas permitidas
                int cantidadRealizadas = db.EncuestaRealizada.Where(e => e.EncuestaID == surveyid).Count();
                //int cant_permitida = (int)db.Encuesta.First(a => a.Id == surveyid).Cantidad;
                int cant_permitida = 0;
                cant_permitida = (int)db.Encuesta.FirstOrDefault(a => a.Id == surveyid).Cantidad;

                if (cantidadRealizadas >= cant_permitida)
                {
                    TempData["error"] = "Cantidad de encuestas realizadas alcanzadas";
                    return RedirectToAction("Index", "Home");
                }


                var encu = db.Encuesta.FirstOrDefault(a => a.Id == surveyid);
                ViewBag.TituloEncuesta = encu.Descripcion;

                var questions = (from p in db.Pregunta
                                 join q in db.EncuestaPregunta on p.Id equals q.PreguntaID
                                 where q.EncuestaID == surveyid
                                 select p).ToList();

                ResponderEncuestaModel model_responder = new ResponderEncuestaModel();

                model_responder.preguntas = questions;
                model_responder.respuestas = new List<EncuestaRespuesta>();

                ViewBag.IdEncuesta = surveyid;

                model_responder.IdEncuesta = encu.Id;

                return View(model_responder);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        // Modificado por Gabriel el 27/07/2016
        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult Responder(ResponderEncuestaModel model, FormCollection form)
        //{
        //    var count = form.Count;
        //    int i = 0;
        //    while (i < count)
        //    {

        //        var ValueCollection = form.Get(i);
        //        var KeyName = form.GetKey(i);

        //        if (!KeyName.Contains("respuestas"))
        //        {
        //            i++;
        //            continue;
        //        }

        //        String[] substrings = KeyName.Split('_');

        //        String[] subValues = ValueCollection.Split(',');

        //        foreach (string value in subValues)
        //        {
        //            int val = Convert.ToInt32(value);
        //            db.EncuestaRespuesta.Add(new EncuestaRespuesta { EncuestaID = model.IdEncuesta, PreguntaID = Int32.Parse(substrings.ElementAt(1)), OpcionID = val });

        //        }

        //        i++;
        //    }

        //    db.SaveChanges();
        //    TempData["success"] = "Respuestas recibidas";

        //    // guardo cookie
        //    string nroEncu = model.IdEncuesta.ToString();
        //    CookieStore.SetCookie(nroEncu, "respondida", TimeSpan.FromHours(1));

        //    return RedirectToAction("Index", "Home");
        //}

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Responder(ResponderEncuestaModel model, FormCollection form)
        {
            // Controlar que no se exceda en la cantidad de encuestas permitidas
            int cantidadRealizadas = db.EncuestaRealizada.Where(e => e.EncuestaID == model.IdEncuesta).Count();
            int cant_permitida = (int)db.Encuesta.First(a => a.Id == model.IdEncuesta).Cantidad;
            if (cantidadRealizadas >= cant_permitida)
            {
                TempData["error"] = "Cantidad de encuestas realizadas alcanzadas";
                return RedirectToAction("Index", "Home");
            }


            // crear encuestaRealizada
            EncuestaRealizada encuestaRealizada = new EncuestaRealizada();
            encuestaRealizada.EncuestaID = model.IdEncuesta;
            encuestaRealizada.FechaHora = DateTime.Now;

            // agrega encuestaRealizada a la DB
            db.EncuestaRealizada.Add(encuestaRealizada);


            var count = form.Count;
            int i = 0;
            while (i < count)
            {

                var ValueCollection = form.Get(i);
                var KeyName = form.GetKey(i);

                if (!KeyName.Contains("respuestas"))
                {
                    i++;
                    continue;
                }

                String[] substrings = KeyName.Split('_');

                String[] subValues = ValueCollection.Split(',');

                foreach (string value in subValues)
                {
                    int val = Convert.ToInt32(value);
                    db.EncuestaRespuesta.Add(new EncuestaRespuesta { EncuestaRealizadaID = encuestaRealizada.Id, EncuestaID = model.IdEncuesta, PreguntaID = Int32.Parse(substrings.ElementAt(1)), OpcionID = val });

                }

                i++;
            }

            try
            {
                db.SaveChanges();
                TempData["success"] = "Respuestas recibidas";
            }
            catch (Exception e)
            {
                TempData["error"] = "Error - No se guardaron las respuestas" + e.ToString();
                return RedirectToAction("Responder");
            }


            // guardo cookie
            string nroEncu = model.IdEncuesta.ToString();
            CookieStore.SetCookie(nroEncu, "respondida", TimeSpan.FromHours(1));

            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        [Authorize]
        public ActionResult CreatefromTemplate(int? id)
        {

            ViewBag.MateriaID = new SelectList(db.Materia, "Id", "Descripcion");

            ViewBag.Plantillas = db.Encuesta.Where(e => e.EsTemplate == true);

            CrearEncuestaModel modelo = new CrearEncuestaModel();
            modelo.encuesta = new Encuesta();
            modelo.encuesta.FechaHora = DateTime.Now;
            modelo.encuesta.Estado = true;
            //modelo.FechaHora = System.DateTime.Now;
            //modelo.Estado = true;

            if (id != null)
            {
                //modelo.TemplateID = id;
                modelo.encuesta.TemplateID = id;
            }

            if (modelo.encuesta.MateriaID != null)
                modelo.encuesta.TematicaID = (from m in db.Materia where m.Id == modelo.encuesta.MateriaID select m.TematicaID).FirstOrDefault();

            modelo.cursos = new SelectList(new List<SelectListItem>
{
    new SelectListItem { Selected = true, Text = string.Empty, Value = ""},
    new SelectListItem { Selected = false, Text = "Curso A", Value = "A"},
    new SelectListItem { Selected = false, Text = "Curso B", Value = "B"},
    new SelectListItem { Selected = false, Text = "Curso C", Value = "C"},
    new SelectListItem { Selected = false, Text = "Curso D", Value = "D"},
    new SelectListItem { Selected = false, Text = "Curso E", Value = "E"},
    new SelectListItem { Selected = false, Text = "Curso F", Value = "F"},
    new SelectListItem { Selected = false, Text = "Curso G", Value = "G"},

}, "Value", "Text", 1);

            modelo.materias = new SelectList(db.Materia, "Id", "Descripcion");

            return View(modelo);

        }

        [HttpPost]
        public ActionResult CreatefromTemplate(CrearEncuestaModel model)
        {

            /* if (model.MateriaID != null)
                 model.TematicaID = (from m in db.Materia where m.Id == model.MateriaID select m.TematicaID).FirstOrDefault();
            
             model.FechaHora = DateTime.Now;
             model.UsuarioID = User.Identity.GetUserId();

             db.Encuesta.Add(model);

             try
             {
                 db.SaveChanges();
             }
             catch (Exception e)
             {
                 TempData["error"] = "error al crear la encuesta" + e.ToString();
                 return RedirectToAction("Index");
             }

             var encu_preg = db.EncuestaPregunta.Where(e => e.EncuestaID == model.TemplateID).ToList();

             foreach (var item in encu_preg)
             {
                 db.EncuestaPregunta.Add(new EncuestaPregunta { EncuestaID = model.Id, PreguntaID = item.PreguntaID });

             }

             try
             {
                 db.SaveChanges();
             }
             catch (Exception e)
             {
                 TempData["error"] = "error al crear la encuesta";
                 return RedirectToAction("Index");
             }


             TempData["success"] = "encuesta creada";

             //return RedirectToAction("Index");

             return RedirectToAction("Details", new { @id = model.Id });
             */

            if (model.encuesta.MateriaID != null)
                model.encuesta.TematicaID = (from m in db.Materia where m.Id == model.encuesta.MateriaID select m.TematicaID).FirstOrDefault();

            model.encuesta.FechaHora = DateTime.Now;
            model.encuesta.UsuarioID = User.Identity.GetUserId();

            db.Encuesta.Add(model.encuesta);

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                TempData["error"] = "error al crear la encuesta" + e.ToString();
                return RedirectToAction("Index");
            }

            var encu_preg = db.EncuestaPregunta.Where(e => e.EncuestaID == model.encuesta.TemplateID).ToList();

            foreach (var item in encu_preg)
            {
                db.EncuestaPregunta.Add(new EncuestaPregunta { EncuestaID = model.encuesta.Id, PreguntaID = item.PreguntaID });

            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                TempData["error"] = "error al crear la encuesta";
                return RedirectToAction("Index");
            }


            TempData["success"] = "encuesta creada";

            //return RedirectToAction("Index");

            return RedirectToAction("Details", new { @id = model.encuesta.Id });
        }



        [HttpGet]
        [Authorize]
        public ActionResult AsignarPreguntas(int id)
        {
            ViewBag.idEncuesta = id;
            return View();

        }

        // TODO vista temporal
        [HttpGet]
        public ActionResult AsignarPreguntas2(int id)
        {
            ViewBag.idEncuesta = id;
            return View();

        }


        [HttpGet]
        [Authorize]
        public ActionResult EditarPreguntas(int id)
        {
            ViewBag.idEncuesta = id;
            return View();

        }


        [HttpPost]
        public ActionResult AgregarPregunta(int id, int encuestaId)
        {
            EncuestaPregunta encu = new EncuestaPregunta();
            encu.EncuestaID = encuestaId;
            encu.PreguntaID = id;

            db.EncuestaPregunta.Add(encu);

            try
            {
                db.SaveChanges();
                return Json(true);

            }
            catch (DbEntityValidationException e)
            {
                return Json(false);

            }


        }

        [HttpPost]
        [Authorize]
        public ActionResult RemoverPregunta(int id, int encuestaId)
        {
            Pregunta preg = db.Pregunta.Find(id);

            if (preg != null)
            {
                db.EncuestaPregunta.RemoveRange(db.EncuestaPregunta.Where(c => c.PreguntaID == id && c.EncuestaID == encuestaId));
                db.SaveChanges();

            }
            return Json(true);
        }


        public ActionResult VerPlantillas(string sortOrder, string currentFilter, string BuscDesc, int? page)
        {


            var plantillas = (from e in db.Encuesta
                              where e.EsTemplate == true
                              select new DatosEncuesta
                              {
                                  encuesta = e,
                                  preguntas = (from p in db.Pregunta
                                               join q in db.EncuestaPregunta on p.Id equals q.PreguntaID
                                               where q.EncuestaID == e.Id
                                               orderby q.Id ascending
                                               select p
                                                   ).ToList()

                              }).ToList();


            if (!String.IsNullOrEmpty(BuscDesc))
            {
                plantillas = plantillas.Where(s => s.encuesta.Descripcion.Contains(BuscDesc)).ToList();

            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(plantillas.ToPagedList(pageNumber, pageSize));

        }

        // Modificado por Gabriel el 22/07/2016
        /* public ActionResult MisEncuestas(string sortOrder, string currentFilter, string searchString, int? page)
        {

            string currentUserId = User.Identity.GetUserId();

            var mis_encuestas = db.Encuesta.Where(e => e.UsuarioID == currentUserId).OrderByDescending(e => e.FechaHora);


            // Modificaciones realizadas por Gabriel el 21/07/2016

            //var ModeloA = (from e in db.Encuesta
            //               where e.EsTemplate == true
            //               //                           join q in db.EncuestaPregunta on e.Id equals q.EncuestaID  // consultar doble validacion preguntas y encuesta                           
            //               select new DatosEncuesta
            //               {
            //                   encuesta = e,
            //                   preguntas = (from p in db.Pregunta
            //                                join q2 in db.EncuestaPregunta on p.Id equals q2.PreguntaID
            //                                select p
            //                                    ).ToList()
            //               }).ToList();

            var ModeloA = (from e in db.Encuesta
                           where e.EsTemplate == true
                           //                           join q in db.EncuestaPregunta on e.Id equals q.EncuestaID  // consultar doble validacion preguntas y encuesta                           
                           select new DatosEncuesta
                           {
                               encuesta = e,
                               preguntas = (from p in db.Pregunta
                                            join q2 in db.EncuestaPregunta on p.Id equals q2.PreguntaID
                                            where q2.EncuestaID == e.Id
                                            select p
                                                ).ToList()
                           }).ToList();

            var cant = ModeloA.Count();


            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(ModeloA.ToPagedList(pageNumber, pageSize));

        } */

        // Agregado por Gabriel el 22/07/2016
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? tematicaId, int? materiaId, int? opcionEncuestaId, int? estado, int? esPropia, string curso, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            string currentUserId = User.Identity.GetUserId();
            //string currentUserId = User.Identity.Name;   // captura el usuario o sea el email

            var mis_encuestas = db.Encuesta.Where(e => e.UsuarioID == currentUserId).OrderByDescending(e => e.FechaHora);


            // Busca si el usuario es perfil supervisor
            //bool esSupervisor = true;

            //esSupervisor = (from u in db.AspNetUsers
            //                                join ur in db.AspNetUserRoles on u.Id equals ur.
            //                                join r in db.AspNetRoles r
            //                                where u.Id == currentUserId
            //                                select true
            //                                    ).FirstOrDefault();

            bool esSupervisor = EsSupervisor();

            string opEncuestaDesc = "Encuestas/Plantillas";
            if (opcionEncuestaId != null)
            {
                switch ((int)opcionEncuestaId)
                {
                    case 1:
                        opEncuestaDesc = "Encuestas";
                        break;
                    case 2:
                        opEncuestaDesc = "Plantillas";
                        break;
                    case 3:
                        opEncuestaDesc = "Encuestas/Plantillas";
                        break;
                    case 4:
                        opEncuestaDesc = "Plantillas Institucionales";
                        break;
                    default:
                        opEncuestaDesc = "Encuestas/Plantillas";
                        break;
                }
            }

            string estadoDesc = "Activas";
            Nullable<bool> blnEstado = true;
            if (estado != null)
            {
                if (estado == 1)
                {
                    estadoDesc = "Activas";
                    blnEstado = true;
                }
                else
                {
                    if (estado == 2)
                    {
                        estadoDesc = "Inactivas";
                        blnEstado = false;
                    }
                    else
                    {
                        if (estado == 3)
                        {
                            estadoDesc = "Activas/Inactivas";
                            blnEstado = null;
                        }
                    }
                }
            }

            string esPropiaDesc = "Propias";
            if (esPropia == null || (esPropia != null && (int)esPropia == 1))
                esPropiaDesc = "Propias";
            else
                esPropiaDesc = "Todas";

            string tematicaDesc = "Temática";
            if (tematicaId != null && tematicaId != 0)
            {
                tematicaDesc = (from t2 in db.Tematica where t2.Id == (int)tematicaId select t2.Descripcion).FirstOrDefault();
            }

            string materiaDesc = "Materia";
            if (materiaId != null && materiaId != 0)
            {
                materiaDesc = (from m2 in db.Materia where m2.Id == (int)materiaId select m2.Descripcion).FirstOrDefault();
            }
            //me esta rompiendo la fecha porque le estoy asignando valores distintos en formato,tanto en el server como en la vista, debo ponerle otro, el seteado funcionaba hasta por ahi nomas



            DateTime? auxfecha;

            if (fechaHasta == null)
            {
                fechaHasta = DateTime.Now;
                fechaHasta = DateTime.Parse(((DateTime)fechaHasta).ToShortDateString());
            }
            fechaHasta = ((DateTime)fechaHasta).AddHours(23).AddMinutes(59).AddSeconds(59);
            if (fechaDesde == null)
            {
                fechaDesde = DateTime.Now;
                fechaDesde = DateTime.Parse(((DateTime)fechaDesde).ToShortDateString()).AddDays(-60);
            }
            if (fechaDesde > fechaHasta) {
                auxfecha = ((DateTime)fechaHasta).AddHours(-23).AddMinutes(-59).AddSeconds(-59);
                fechaHasta = ((DateTime)fechaDesde).AddHours(23).AddMinutes(59).AddSeconds(59);
                fechaDesde = auxfecha;
            }

            

           






            var ModeloA = (from e in db.Encuesta
                               //where e.EsTemplate == true
                           where (tematicaId == null || tematicaId == 0 || (tematicaId != null && e.TematicaID == tematicaId))
                                && (materiaId == null || materiaId == 0 || (materiaId != null && e.MateriaID == materiaId))
                                && (curso == null || curso == "*" || curso== "CURSOS" || (e.Curso == curso))
                                 && (  fechaDesde <= e.FechaHora && e.FechaHora <= fechaHasta )
                                && (opcionEncuestaId == null || opcionEncuestaId == 0 || (opcionEncuestaId != null
                                    && ((opcionEncuestaId == 1 && e.EsTemplate == false)
                                        || (opcionEncuestaId == 2 && e.EsTemplate == true)
                                        || (opcionEncuestaId == 3)
                                        || (opcionEncuestaId == 4 && e.EsTemplate == true && e.TematicaID == null && e.MateriaID == null))))
                                && (blnEstado == null || (blnEstado != null && e.Estado == blnEstado))
                                && (((esPropia == null || esPropia == 1) && e.UsuarioID == currentUserId) || esPropia == 0)
                                && ((e.UsuarioID == currentUserId) || e.EsTemplate == true || esSupervisor == true)
                           select new DatosEncuesta
                           {
                               encuesta = e,
                               preguntas = (from p in db.Pregunta
                                            join q2 in db.EncuestaPregunta on p.Id equals q2.PreguntaID
                                            where q2.EncuestaID == e.Id
                                            orderby q2.Id ascending
                                            select p
                                                ).ToList(),

                               cantidadEncuestasRealizadas = (from t in db.Encuesta
                                                              join r in db.EncuestaRespuesta on t.Id equals r.EncuestaID
                                                              where (e.EsTemplate == false && t.Id == e.Id) ||
                                                                    (e.EsTemplate == true && t.TemplateID == e.Id)
                                                              select r
                                                ).Count(),

                               puedoEditar = (from n in db.Encuesta where n.Id == e.Id && n.UsuarioID == currentUserId select true).FirstOrDefault(),
                               puedoVerEstadistica = (from a in db.Encuesta where (a.Id == e.Id && a.UsuarioID == currentUserId) || esSupervisor == true select true).FirstOrDefault()

                           }).OrderByDescending(e => e.encuesta.EsTemplate).ThenByDescending(e => e.encuesta.FechaHora).AsEnumerable().ToList();

            FiltrosEncuesta filtros = new FiltrosEncuesta();
            filtros.tematicas = (from t in db.Tematica select t).ToList();
            filtros.materias = (from m in db.Materia where m.TematicaID == tematicaId select m).ToList();
           

                var cursofiltrado = (from c in db.Encuesta where c.Curso != null orderby c.Curso  select c.Curso).Distinct().ToList();

            filtros.Curso = cursofiltrado;
            


          


            filtros.opcionTematicaId = tematicaId;
            filtros.opcionMateriaId = materiaId;
            filtros.opcionEncuestaId = opcionEncuestaId;
            filtros.opcionEstado = estado;
            filtros.esPropia = esPropia;
            filtros.cursoDescripcion = curso;
            filtros.opcionCurso= curso;

            filtros.tematicaDescripcion = tematicaDesc;
            filtros.materiaDescripcion = materiaDesc;
            filtros.opcionEncuestaDescripcion = opEncuestaDesc;
            filtros.opcionEstadoDescripcion = estadoDesc;
            filtros.esPropiaDescripcion = esPropiaDesc;
            

            filtros.FechaDesde = fechaDesde ;
            filtros.FechaHasta = fechaHasta;

            var cant = ModeloA.Count();


            //int pageSize = 6;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //return View(ModeloA.ToPagedList(pageNumber, pageSize));


            MisEncuestasModel model = new MisEncuestasModel();
            model.encuestas = ModeloA.ToPagedList(pageNumber, pageSize);
            model.filtros = filtros;

            return View(model);
        }
        

        [HttpGet]
        [Authorize]
        public ActionResult Resultados(int id, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            if (fechaDesde == null)
                fechaDesde = DateTime.Now.AddDays(-30);

            if (fechaHasta == null)
                fechaHasta = DateTime.Now;

            fechaDesde = DateTime.Parse(((DateTime)fechaDesde).ToShortDateString());
            fechaHasta = DateTime.Parse(((DateTime)fechaHasta).ToShortDateString());
            
            DateTime fechaHastaConsulta = ((DateTime)fechaHasta).AddDays(1);
            
            ResultadosGModel model = new ResultadosGModel();
            
            model.idEncuesta = id;
            model.fechaDesde = fechaDesde;
            model.fechaHasta = fechaHasta;
            return View(model);

        }


        [HttpGet]
        [Authorize]
        public ActionResult getResultadosEncuesta(int idEncuesta, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            if (fechaDesde == null)
                fechaDesde = DateTime.Now.AddDays(-30);

            if (fechaHasta == null)
                fechaHasta = DateTime.Now;

            fechaDesde = DateTime.Parse(((DateTime)fechaDesde).ToShortDateString());
            fechaHasta = DateTime.Parse(((DateTime)fechaHasta).ToShortDateString());

            DateTime fechaHastaConsulta = ((DateTime)fechaHasta).AddDays(1);

            var questions = (from p in db.Pregunta
                             join q in db.EncuestaPregunta on p.Id equals q.PreguntaID
                             join e in db.Encuesta on q.EncuestaID equals e.Id
                             where ((e.Id == idEncuesta) || (e.EsTemplate == true && e.TemplateID == idEncuesta))
                                    && (e.FechaHora >= fechaDesde && e.FechaHora < fechaHastaConsulta)
                             select p).AsEnumerable().ToList();

            ResultadosGModel model = new ResultadosGModel();
            model.idEncuesta = idEncuesta;
            model.preguntas = questions;
            model.fechaDesde = fechaDesde;
            model.fechaHasta = fechaHasta;
            return PartialView("_ResultadoEncuesta", model);

        }

        [HttpGet]
        [Authorize]
        public ActionResult getResultadosEncuestaLinea(int idEncuesta, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            if (fechaDesde == null)
                fechaDesde = DateTime.Now.AddDays(-30);

            if (fechaHasta == null)
                fechaHasta = DateTime.Now;

            fechaDesde = DateTime.Parse(((DateTime)fechaDesde).ToShortDateString());
            fechaHasta = DateTime.Parse(((DateTime)fechaHasta).ToShortDateString());

            DateTime fechaHastaConsulta = ((DateTime)fechaHasta).AddDays(1);

            var questions = (from p in db.Pregunta
                             join q in db.EncuestaPregunta on p.Id equals q.PreguntaID
                             join e in db.Encuesta on q.EncuestaID equals e.Id
                             where ((e.Id == idEncuesta) || (e.EsTemplate == true && e.TemplateID == idEncuesta))
                                    && (e.FechaHora >= fechaDesde && e.FechaHora < fechaHastaConsulta)
                             select p).AsEnumerable().ToList();

            ResultadosGModel model = new ResultadosGModel();
            model.idEncuesta = idEncuesta;
            model.preguntas = questions;
            model.fechaDesde = fechaDesde;
            model.fechaHasta = fechaHasta;
            return PartialView("_ResultadoEncuestaLinea", model);

        }




        [HttpGet]
        [Authorize]
        public ActionResult GetGraficoPregunta(int idEncuesta, int idPregunta, DateTime? fechaDesde, DateTime? fechaHasta)
        {
             
            if (fechaDesde == null)
                fechaDesde = DateTime.Now.AddDays(-30);

            if (fechaHasta == null)
                fechaHasta = DateTime.Now;
            
            fechaDesde = DateTime.Parse(((DateTime)fechaDesde).ToShortDateString( ));
            fechaHasta = DateTime.Parse(((DateTime)fechaHasta).ToShortDateString());

            DateTime fechaHastaConsulta = ((DateTime)fechaHasta).AddDays(1);
             

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("item"));
            dt.Columns.Add(new DataColumn("cantidad"));


            var respuestas = (from p in db.Opcion
                              where (p.PreguntaID == idPregunta)
                              select new
                              {
                                  Id = p.Id,
                                  PreguntaID = p.PreguntaID,
                                  Valor = p.valor,
                                  Peso = p.PesoEstadistico
                              }).ToList();

                foreach (var item in respuestas)
            {
                DataRow row1 = dt.NewRow();
                row1[0] = item.Valor;

                var encuestarespuestas = (from p in db.EncuestaRespuesta

                                          join e in db.Encuesta on p.EncuestaID equals e.Id
                                          where p.OpcionID == item.Id 
                                                && ((e.Id == idEncuesta) || (e.EsTemplate == false && e.TemplateID == idEncuesta))
                                                && (e.FechaHora >= fechaDesde && e.FechaHora < fechaHastaConsulta)
                                  
                                          select new
                                          {
                                              Id = p.Id,
                                              EncuestaID = p.EncuestaID,
                                              PreguntaID = p.PreguntaID,
                                              OpcionID = p.OpcionID,
                                          }).ToList();

                row1[1] = encuestarespuestas.Count();
                dt.Rows.Add(row1);
            }


            ViewBag.idChart = idPregunta;
            return PartialView("_PreguntaGrafico", dt);


        }
        [HttpGet]
        [Authorize]
        public ActionResult GetGraficoPreguntaLinea(int idEncuesta, int idPregunta, DateTime? fechaDesde, DateTime? fechaHasta)
        {

            if (fechaDesde == null)
                fechaDesde = DateTime.Now.AddDays(-30);

            if (fechaHasta == null)
                fechaHasta = DateTime.Now;

            fechaDesde = DateTime.Parse(((DateTime)fechaDesde).ToShortDateString());
            fechaHasta = DateTime.Parse(((DateTime)fechaHasta).ToShortDateString());

            DateTime fechaHastaConsulta = ((DateTime)fechaHasta).AddDays(1);


            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("item"));
            dt.Columns.Add(new DataColumn("cantidad"));


            var respuestas = (from p in db.Opcion
                              where (p.PreguntaID == idPregunta)
                              select new
                              {
                                  Id = p.Id,
                                  PreguntaID = p.PreguntaID,
                                  Valor = p.valor,
                                  Peso = p.PesoEstadistico
                              }).ToList();

            foreach (var item in respuestas)
            {
                DataRow row1 = dt.NewRow();
                row1[0] = item.Valor;

                var encuestarespuestas = (from p in db.EncuestaRespuesta

                                          join e in db.Encuesta on p.EncuestaID equals e.Id
                                          where p.OpcionID == item.Id
                                                && ((e.Id == idEncuesta) || (e.EsTemplate == false && e.TemplateID == idEncuesta))
                                                && (e.FechaHora >= fechaDesde && e.FechaHora < fechaHastaConsulta)

                                          select new
                                          {
                                              Id = p.Id,
                                              EncuestaID = p.EncuestaID,
                                              PreguntaID = p.PreguntaID,
                                              OpcionID = p.OpcionID,
                                          }).ToList();

                row1[1] = encuestarespuestas.Count();
                dt.Rows.Add(row1);
            }


            ViewBag.idChart = idPregunta;
            return PartialView("_PreguntaGraficoLinea", dt);


        }



        [HttpGet]
        [Authorize]
        public ActionResult GetEditarPregunta(int id, int? idEncuesta)
        {

            Pregunta model = db.Pregunta.Find(id);
            
            SelectListItem selListItem = new SelectListItem() { Value = "radio", Text = "Seleccion simple" };
            SelectListItem selListItem2 = new SelectListItem() { Value = "check", Text = "Seleccion multiple" };
            List<SelectListItem> lista = new List<SelectListItem>();

            lista.Add(selListItem);
            lista.Add(selListItem2);

            ViewBag.OpcionesPregunta = lista;
            ViewBag.materias = new SelectList(db.Materia, "Id", "Descripcion");
            ViewBag.tematicas = new SelectList(db.Tematica, "Id", "Descripcion");
            

            ViewBag.idEncuesta = idEncuesta;
            
            return PartialView("_EditPregunta", model);
        }



        [HttpGet]

        public ActionResult Construccion()
        {
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        



        // Verifica si el usuario l ogueado es Supervisor
        private Boolean EsSupervisor()
        { 
            bool esSupervisor = false;
            string sentenciaSQL = "select UserId, RoleId from AspNetUserRoles where UserId = '" + User.Identity.GetUserId().ToString() + "' and RoleId = '2'";

            DataSet datos = TraerPorSQL(sentenciaSQL);

            if (datos.Tables[0].Rows.Count > 0)
                esSupervisor = true;

            return esSupervisor;
        }
        
        #region Traer por sentenciaSQL
        // Traer por sentenciaSQL
        private DataSet TraerPorSQL(string sentenciaSQL)
        {
            string stringDeConexion = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

            System.Data.SqlClient.SqlConnection conexion = new System.Data.SqlClient.SqlConnection(stringDeConexion);
            System.Data.SqlClient.SqlCommand comando = new System.Data.SqlClient.SqlCommand(sentenciaSQL, conexion);
            System.Data.SqlClient.SqlDataAdapter adaptador = new System.Data.SqlClient.SqlDataAdapter(comando);

            DataSet dsDatos = new DataSet();

            comando.CommandType = CommandType.Text;

            try
            {
                //conex  ion.Open();
                adaptador.SelectCommand = comando;
                adaptador.Fill(dsDatos, "DATOS");

                return dsDatos;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                dsDatos.Dispose();
                adaptador.Dispose();
                comando.Dispose();
                conexion.Close();
            }
        }
        #endregion

    }
}

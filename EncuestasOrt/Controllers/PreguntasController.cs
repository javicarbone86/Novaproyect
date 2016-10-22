using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EncuestasOrt;
using Microsoft.AspNet.Identity;
using PagedList;
using EncuestasOrt.Models;


namespace EncuestasOrt.Views
{

    [Authorize]
    public class PreguntasController : Controller
    {
        private EncuestasContainer db = new EncuestasContainer();

        // GET: Preguntas
        public ActionResult IndexOldPage(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var Questions = from Preguntas in db.Pregunta 
                        
                            select Preguntas;


            Questions = Questions.OrderBy(Preguntas => Preguntas.Id);

            if (!String.IsNullOrEmpty(searchString))
            {
                Questions = Questions.Where(s => s.Descripcion.Contains(searchString));
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(Questions.ToPagedList(pageNumber, pageSize));

            //return View(db.Pregunta.ToList());
        }

        // GET: Preguntas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Pregunta.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            return View(pregunta);
        }




        // GET: Preguntas/Create
        public ActionResult Create()
        {

            SelectListItem selListItem = new SelectListItem() { Value = "radio", Text = "Seleccion simple" };
            SelectListItem selListItem2 = new SelectListItem() { Value = "check", Text = "Seleccion multiple" };

            List<SelectListItem> lista = new List<SelectListItem>();

            lista.Add(selListItem);
            lista.Add(selListItem2);

            ViewBag.OpcionesPregunta = lista;

            ViewBag.materias = new SelectList(db.Materia, "Id", "Descripcion");
            ViewBag.tematicas = new SelectList(db.Tematica, "Id", "Descripcion");


            // modelo
            Pregunta modelo_pregunta = new Pregunta();


            // usuario actual
            string currentUserId = User.Identity.GetUserId();

            //guardo el usuario
            if (currentUserId != null)
            {
                modelo_pregunta.UsuarioID = currentUserId.ToString();
            }
            else
            {
                // TODO Fix temporal
                modelo_pregunta.UsuarioID = "1";

            }


            return View(modelo_pregunta);







        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pregunta pregunta, string[] dynamicField)
        {
            if (ModelState.IsValid)
            {

                foreach (string s in dynamicField)
                {

                    pregunta.Opcion.Add(new Opcion { valor = s });
                }

                db.Pregunta.Add(pregunta);

                db.SaveChanges();

                TempData["success"] = "Pregunta guardada correctamente";
                return RedirectToAction("Index");
            }

            return View(pregunta);
        }

        // GET: Preguntas/Edit/5
        public ActionResult Edit(int? id)
        {
            bool puedoEditar = false;
            string currentUserId = User.Identity.GetUserId();
            puedoEditar = (from n in db.Pregunta where n.Id == id && n.UsuarioID == currentUserId select true).FirstOrDefault();
            if (!puedoEditar) {
                TempData["error"] = "Usted no puede editar la pregunta";
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Pregunta.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }

            SelectListItem selListItem = new SelectListItem() { Value = "radio", Text = "Seleccion simple" };
            SelectListItem selListItem2 = new SelectListItem() { Value = "check", Text = "Seleccion multiple" };

            List<SelectListItem> lista = new List<SelectListItem>();

            lista.Add(selListItem);
            lista.Add(selListItem2);

            ViewBag.OpcionesPregunta = lista;

            return View(pregunta);

        }

        // POST: Preguntas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pregunta pregunta)
        {
            if (ModelState.IsValid)
            {

                foreach (var row in pregunta.Opcion)
                {
                    if (row.Id == 0)
                    {
                        db.Entry(row).State = EntityState.Added;
                    }
                    else
                    {
                        db.Entry(row).State = EntityState.Modified;
                    }
                    //db.Opcion.Add(row);

                }
                //test
                db.SaveChanges();

                db.Entry(pregunta).State = EntityState.Modified;
                db.SaveChanges();

                TempData["success"] = "Preguta modificada correctamente";

                return RedirectToAction("Index");
            }
            return View(pregunta);
        }

        // GET: Preguntas/Delete/5
        public ActionResult Delete(int? id)
        {
            bool puedoEditar = false;
            string currentUserId = User.Identity.GetUserId();
            puedoEditar = (from n in db.Pregunta where n.Id == id && n.UsuarioID == currentUserId select true).FirstOrDefault();
            if (!puedoEditar)
            {
                TempData["error"] = "Usted no puede eliminar la pregunta";
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Pregunta.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            return View(pregunta);
        }

        // POST: Preguntas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //borro Opciones relacionadas a ese ID de pregunta.
            db.Opcion.RemoveRange(db.Opcion.Where(c => c.PreguntaID == id));
            // borro pregunta
            Pregunta pregunta = db.Pregunta.Find(id);
            if (pregunta.EncuestaPregunta.Count() == 0)
            {
                db.Pregunta.Remove(pregunta);
                db.SaveChanges();

                TempData["success"] = "Pregunta eliminada";
            }
            else
            {

                TempData["error"] = "Pregunta no puede ser eliminada, esta siendo usada en " + pregunta.EncuestaPregunta.Count() + " encuestas";
            }


            return RedirectToAction("Index");
        }



        public ActionResult AgregarOpcion(int idPregunta)
        {
            var opcion = new Opcion();
            opcion.PreguntaID = idPregunta;
            return PartialView("_OpcionEditor", opcion);

        }

        public ActionResult BorrarOpcion(int idOpcion)
        {
            bool puedoEditar = false;
            string currentUserId = User.Identity.GetUserId();
            puedoEditar = (from n in db.EncuestaRespuesta where n.OpcionID == idOpcion select true).FirstOrDefault();
            if (!puedoEditar)
            {
                TempData["error"] = "No se puede eliminar la opción dado que esta siendo usada";
                string edit = "Edit/" + 28;
                //return RedirectToAction("Edit", "Preguntas", new { id = 28 });
                return RedirectToAction("Index", "Preguntas");
            }

            if (idOpcion != 0)
            {
                Opcion opcion = db.Opcion.Find(idOpcion);

                if (opcion != null)
                {
                    db.EncuestaRespuesta.RemoveRange(db.EncuestaRespuesta.Where(c => c.OpcionID == idOpcion));
                    db.Opcion.Remove(opcion);
                    db.SaveChanges();
                    TempData["success"] = "Opción elimitada";
                }


            }

            return View();
        }


        [HttpGet]
        public ActionResult Index()
        {

            return View();

        }


        [HttpGet]
        public JsonResult GetPreguntas(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total;
            string currentUserId = User.Identity.GetUserId();

            try
            {
                var data = db.Pregunta.Select(p => new
                {
                    Id = p.Id,
                    Descripcion = p.Descripcion,
                    Tipo = p.Tipo,
                    Requerido = p.Requerido,
                    Usuario  = p.UsuarioID
                }).Where(p => p.Usuario == currentUserId).AsQueryable();

                if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
                {
                    if (direction.Trim().ToLower() == "asc")
                    {

                        data = SortController.OrderBy(data, sortBy);
                    }
                    else
                    {
                        data = SortController.OrderBy(data, sortBy);
                    }
                }
                else { data = SortController.OrderBy(data, "Id");  }
                var preguntas = data.ToList();
                total = data.Count();

                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    preguntas = preguntas.Where(p => p.Descripcion.ToUpper().Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U").Contains(searchString.ToUpper().Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U"))).ToList();
                    total = preguntas.Count();
                }

                if (page.HasValue && limit.HasValue)
                {
                    int start = (page.Value - 1) * limit.Value;
                    var records = preguntas.Skip(start).Take(limit.Value);
                    return Json(new { records, total }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { preguntas, total }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }


        }


        [HttpPost]
        public JsonResult Remove(int id)
        {
            //borro Opciones relacionadas a ese ID de pregunta.
            db.Opcion.RemoveRange(db.Opcion.Where(c => c.PreguntaID == id));
            //borro pregunta
            Pregunta pregunta = db.Pregunta.Find(id);
            db.Pregunta.Remove(pregunta);
            db.SaveChanges();
            return Json(true);
        }



        [HttpPost]
        public JsonResult Save(int id)
        {

            Pregunta pregunta = db.Pregunta.Find(id);

            foreach (var row in pregunta.Opcion)
            {
                if (row.Id == 0)
                {
                    db.Entry(row).State = EntityState.Added;
                }
                else
                {
                    db.Entry(row).State = EntityState.Modified;
                }
                //db.Opcion.Add(row);

            }
            //test
            db.SaveChanges();

            db.Entry(pregunta).State = EntityState.Modified;
            db.SaveChanges();

            TempData["success"] = "Preguta modificada correctamente";


            return Json(true);
        }


        [HttpGet]
        public ActionResult Grafico(int id)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("item"));
            dt.Columns.Add(new DataColumn("cantidad"));



            var respuestas = (from p in db.Opcion
                              where (p.PreguntaID == id)
                              select new
                              {
                                  Id = p.Id,
                                  PreguntaID = p.PreguntaID,
                                  Valor = p.valor,
                                  Peso = p.PesoEstadistico
                              }).ToList();

            /* var encuestarespuestas = (from p in db.EncuestaRespuesta
                                       where (p.PreguntaID == id)
                                       select new
                                       {
                                           Id = p.Id,
                                           EncuestaID = p.EncuestaID,
                                           PreguntaID = p.PreguntaID,
                                           OpcionID = p.OpcionID,
                                       }).ToList();*/


            foreach (var item in respuestas)
            {
                DataRow row1 = dt.NewRow();
                row1[0] = item.Valor;



                var encuestarespuestas = (from p in db.EncuestaRespuesta
                                          where (p.OpcionID == item.Id) //&& p.PreguntaID = id
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

            ViewBag.data = dt;

            Pregunta preg = db.Pregunta.Find(id);
            ViewBag.Descripcion = preg.Descripcion;

            return View();
        }


        [HttpGet]
        public ActionResult GetPreguntasAsignadas(int id)
        {
            ViewBag.idEncuesta = id; // TODO

            var data = (from e in db.Pregunta
                        join p in db.EncuestaPregunta on e.Id equals p.PreguntaID
                        where p.EncuestaID == id
                        orderby e.Id ascending
                        select e).AsEnumerable();

            return PartialView("_PreguntasAsignadas", data);

            

        }


        [HttpGet]
        public ActionResult GetPreguntasSinAsignar(int id, int? page, int? tematicaId, int? materiaId, int? estado, int? esPropia)
        {

            string currentUserId = User.Identity.GetUserId();

            // Busco las preguntas que estan agregadas a la encuesta 
            var listadoExcep = (from e in db.EncuestaPregunta
                                where e.EncuestaID == id
                                orderby e.Id ascending
                                select new { PreguntaId = e.PreguntaID }).ToList();

            // Convierto a listado
            List<int> listado = new List<int>();
            foreach (var item in listadoExcep)
            {
                listado.Add(item.PreguntaId);
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

            // Busco las pregunta que no esten agregadas a la encuesta
            var data = (from p in db.Pregunta
                        where (!listado.Contains(p.Id)

                     && (tematicaId == null || tematicaId == 0 || (tematicaId != null && p.TematicaID == tematicaId))
                     && (materiaId == null || materiaId == 0 || (materiaId != null && p.MateriaID == materiaId))
                     && (((esPropia == null || esPropia == 1) && p.UsuarioID == currentUserId) || esPropia == 0)

                        )
                        select p).AsEnumerable().OrderBy(e => e.Id).ToList();
            


            FiltrosPregunta filtros = new FiltrosPregunta();

            filtros.tematicas = (from t in db.Tematica select t).ToList();
            filtros.materias = (from m in db.Materia where m.TematicaID == tematicaId select m).ToList();

            filtros.opcionTematicaId = tematicaId;
            filtros.opcionMateriaId = materiaId;
            //filtros.opcionEncuestaId = opcionEncuestaId;
            filtros.opcionEstado = estado;
            filtros.esPropia = esPropia;

            filtros.tematicaDescripcion = tematicaDesc;
            filtros.materiaDescripcion = materiaDesc;
            //filtros.opcionEncuestaDescripcion = opEncuestaDesc;
            filtros.opcionEstadoDescripcion = estadoDesc;
            filtros.esPropiaDescripcion = esPropiaDesc;


            int pageSize = 6;
            int pageNumber = (page ?? 1);



            BancoPreguntas model = new BancoPreguntas();
            model.preguntas = data.ToPagedList(pageNumber, pageSize);
            model.filtros = filtros;

            //return View(model);

            return PartialView("_PreguntasBanco", model);

        }




        [HttpGet]
        public ActionResult GetNuevaPregunta(int? idEncuesta)
        {
            Pregunta data = new Pregunta();

            SelectListItem selListItem = new SelectListItem() { Value = "radio", Text = "Seleccion simple" };
            SelectListItem selListItem2 = new SelectListItem() { Value = "check", Text = "Seleccion multiple" };

            List<SelectListItem> lista = new List<SelectListItem>();

            lista.Add(selListItem);
            lista.Add(selListItem2);

            ViewBag.OpcionesPregunta = lista;


            ViewBag.materias = new SelectList(db.Materia, "Id", "Descripcion");
            ViewBag.tematicas = new SelectList(db.Tematica, "Id", "Descripcion");


            // usuario actual
            string currentUserId = User.Identity.GetUserId();

            //guardo el usuario
            if (currentUserId != null)
            {
                data.UsuarioID = currentUserId.ToString();
            }
            else
            {
                // TODO Fix temporal
                data.UsuarioID = "1";

            }

            ViewBag.idEncuesta = idEncuesta; // TODO

            // Busco Tema
            Encuesta encu = db.Encuesta.Find(idEncuesta);
            if (encu != null)
            {
                data.MateriaID = encu.MateriaID;
                data.TematicaID = encu.TematicaID;
            }

            return PartialView("_AddPregunta", data);

        }


        [HttpPost]
        public JsonResult PostNuevaPregunta(Pregunta preg, string[] dynamicField)
        {

            if (ModelState.IsValid)
            {

                foreach (string s in dynamicField)
                {

                    preg.Opcion.Add(new Opcion { valor = s });
                }

                preg.EstadoInactivo = false; // todo
                db.Pregunta.Add(preg);

                db.SaveChanges();
                return Json(preg.Id, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public JsonResult PostEditPregunta(Pregunta preg)
        {

            if (ModelState.IsValid)
            {

                foreach (var row in preg.Opcion)
                {
                    if (row.Id == 0)
                    {
                        db.Entry(row).State = EntityState.Added;
                    }
                    else
                    {
                        db.Entry(row).State = EntityState.Modified;
                    }
                    //db.Opcion.Add(row);

                }

                db.SaveChanges();

                db.Entry(preg).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);


            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult BorrarOpcionJson(int idOpcion)
        {

            if (idOpcion != 0)
            {
                Opcion opcion = db.Opcion.Find(idOpcion);

                if (opcion != null)
                {
                    db.EncuestaRespuesta.RemoveRange(db.EncuestaRespuesta.Where(c => c.OpcionID == idOpcion));
                    db.Opcion.Remove(opcion);
                    db.SaveChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);


                }


            }
            return Json(false, JsonRequestBehavior.AllowGet);



        }


        [HttpGet]
        public ActionResult GetFiltros(int idEncuesta, int? page, int? tematicaId, int? materiaId, int? opcionEncuestaId, int? estado, int? esPropia)
        {
            BancoPreguntas model = new BancoPreguntas();
            FiltrosPregunta filtros = new FiltrosPregunta();

            filtros.tematicas = (from t in db.Tematica select t).ToList();
            filtros.materias = (from m in db.Materia where m.TematicaID == tematicaId select m).ToList();

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

            filtros.opcionTematicaId = tematicaId;
            filtros.opcionMateriaId = materiaId;
            filtros.opcionEncuestaId = opcionEncuestaId;
            filtros.opcionEstado = estado;
            filtros.esPropia = esPropia;

            filtros.tematicaDescripcion = tematicaDesc;
            filtros.materiaDescripcion = materiaDesc;
            filtros.opcionEncuestaDescripcion = opEncuestaDesc;
            filtros.opcionEstadoDescripcion = estadoDesc;
            filtros.esPropiaDescripcion = esPropiaDesc;

            model.filtros = filtros;

            model.idEncuesta = idEncuesta;


            return PartialView("_FiltrosPregunta", model);

        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public sealed class NoCacheAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuting(ResultExecutingContext filterContext)
            {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();

                base.OnResultExecuting(filterContext);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

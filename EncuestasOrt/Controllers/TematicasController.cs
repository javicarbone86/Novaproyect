using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EncuestasOrt;

namespace EncuestasOrt.Views.Materias
{
    public class TematicasController : Controller
    {
        private EncuestasContainer db = new EncuestasContainer();

        // GET: Tematicas
        public ActionResult Index()
        {
            var tematica = db.Tematica.Include(t => t.Carrera);
            return View(tematica.ToList());
        }

        // GET: Tematicas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tematica tematica = db.Tematica.Find(id);
            if (tematica == null)
            {
                return HttpNotFound();
            }
            return View(tematica);
        }

        // GET: Tematicas/Create
        public ActionResult Create()
        {
            ViewBag.CarreraID = new SelectList(db.Carrera, "Id", "Descripcion");
            return View();
        }

        // POST: Tematicas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CarreraID,Descripcion")] Tematica tematica)
        {
            if (ModelState.IsValid)
            {
                db.Tematica.Add(tematica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CarreraID = new SelectList(db.Carrera, "Id", "Descripcion", tematica.CarreraID);
            return View(tematica);
        }

        // GET: Tematicas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tematica tematica = db.Tematica.Find(id);
            if (tematica == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarreraID = new SelectList(db.Carrera, "Id", "Descripcion", tematica.CarreraID);
            return View(tematica);
        }

        // POST: Tematicas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CarreraID,Descripcion")] Tematica tematica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tematica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarreraID = new SelectList(db.Carrera, "Id", "Descripcion", tematica.CarreraID);
            return View(tematica);
        }

        // GET: Tematicas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tematica tematica = db.Tematica.Find(id);
            if (tematica == null)
            {
                return HttpNotFound();
            }
            return View(tematica);
        }

        // POST: Tematicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tematica tematica = db.Tematica.Find(id);
            db.Tematica.Remove(tematica);
            db.SaveChanges();
            return RedirectToAction("Index");
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

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
    public class PlantillasController : Controller
    {
        private EncuestasContainer db = new EncuestasContainer();
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
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? tematicaId, int? materiaId, int? opcionEncuestaId, int? estado, int? esPropia, string curso, DateTime? fechaDesde, DateTime? fechaHasta, int? plantillaId, DateTime? fechaRangoDesde, DateTime? fechaRangoHasta )
        {
            string currentUserId = User.Identity.GetUserId();


            var mis_encuestas = db.Encuesta.Where(e => e.UsuarioID == currentUserId).OrderByDescending(e => e.FechaHora);

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
            if (fechaDesde > fechaHasta)
            {
                auxfecha = ((DateTime)fechaHasta).AddHours(-23).AddMinutes(-59).AddSeconds(-59);
                fechaHasta = ((DateTime)fechaDesde).AddHours(23).AddMinutes(59).AddSeconds(59);
                fechaDesde = auxfecha;
            }










            var ModeloA = (from e in db.Encuesta
                               //where e.EsTemplate == true
                           where (tematicaId == null || tematicaId == 0 || (tematicaId != null && e.TematicaID == tematicaId))
                                && (materiaId == null || materiaId == 0 || (materiaId != null && e.MateriaID == materiaId))
                                && (curso == null || curso == "*" || curso == "CURSOS" || (e.Curso == curso))
                                 && (plantillaId == null || plantillaId == 0 || (e.Id == plantillaId))
                                 && (fechaDesde <= e.FechaHora && e.FechaHora <= fechaHasta)
                                && (opcionEncuestaId == null || opcionEncuestaId == 0 || (opcionEncuestaId != null
                                    && ((opcionEncuestaId == 1 && e.EsTemplate == false)
                                        || (opcionEncuestaId == 2 && e.EsTemplate == true)
                                        || (opcionEncuestaId == 3)
                                        || (opcionEncuestaId == 4 && e.EsTemplate == true && e.TematicaID == null && e.MateriaID == null))))
                                && (blnEstado == null || (blnEstado != null && e.Estado == blnEstado))
                                //&& (((esPropia == null || esPropia == 1) && e.UsuarioID == currentUserId) || esPropia == 0)
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


            var cursofiltrado = (from c in db.Encuesta where c.Curso != null orderby c.Curso select c.Curso).Distinct().ToList();
            //query de Encuestas que son plantillas para el combobox
            var PlantillfFiltrado = (from c in db.Encuesta where c.EsTemplate == true orderby c.TemplateID select c).Distinct().ToList();
            filtros.Curso = cursofiltrado;
            //aca le paso todas las encuestas que son plantilla
            filtros.Plantilla = PlantillfFiltrado;

            //aca le paso el id de la plantilla seleccionada a la vista
            filtros.PlantillaId = plantillaId;


            if (plantillaId == null || plantillaId == 0)
            {
                filtros.PlantillaDescripcion = "PLANTILLAS";
                filtros.PlantillaId = 0;

            }
            else {
                filtros.PlantillaDescripcion = (from c in db.Encuesta where c.Id == plantillaId  select c.Descripcion).FirstOrDefault(); 

            }
            
            filtros.opcionTematicaId = tematicaId;
            filtros.opcionMateriaId = materiaId;
            filtros.opcionEncuestaId = opcionEncuestaId;
            filtros.opcionEstado = estado;
            filtros.esPropia = esPropia;
            filtros.cursoDescripcion = curso;
            filtros.opcionCurso = curso;

            filtros.tematicaDescripcion = tematicaDesc;
            filtros.materiaDescripcion = materiaDesc;
            filtros.opcionEncuestaDescripcion = opEncuestaDesc;
            filtros.opcionEstadoDescripcion = estadoDesc;
            filtros.esPropiaDescripcion = esPropiaDesc;
            filtros.FechaRangoDesde = fechaRangoDesde;
            filtros.FechaRangoHasta = fechaRangoHasta;

            filtros.FechaDesde = fechaDesde;
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
        public ActionResult Resultados(int? tematicaId, int? materiaId, string curso, DateTime? fechaDesde, DateTime? fechaHasta, int? plantillaId, DateTime? fechaRangoDesde, DateTime? fechaRangoHasta)
        {
            string currentUserId = User.Identity.GetUserId();
            bool esSupervisor = EsSupervisor();

            var idEncuestas = (from e in db.Encuesta
                               where (tematicaId == null || tematicaId == 0 || (tematicaId != null && e.TematicaID == tematicaId))
                                    && (materiaId == null || materiaId == 0 || (materiaId != null && e.MateriaID == materiaId))
                                    && (e.TemplateID == plantillaId)
                                    && (curso == null || curso == "*" || curso == "CURSOS" || (e.Curso == curso))
                                     && (fechaDesde <= e.FechaHora && e.FechaHora <= fechaHasta)
                                    && (e.UsuarioID == currentUserId)
                                    && ((e.UsuarioID == currentUserId) || e.EsTemplate == false || esSupervisor == true)
                               select e.Id
                                    ).ToList();

            var idEncuestaSegundoRango = (from e in db.Encuesta
                                          where (tematicaId == null || tematicaId == 0 || (tematicaId != null && e.TematicaID == tematicaId))
                                               && (materiaId == null || materiaId == 0 || (materiaId != null && e.MateriaID == materiaId))
                                               && (e.TemplateID == plantillaId)
                                               && (curso == null || curso == "*" || curso == "CURSOS" || (e.Curso == curso))
                                                 && (fechaRangoDesde <= e.FechaHora && e.FechaHora <= fechaRangoHasta)
                                               && (e.UsuarioID == currentUserId)
                                               && ((e.UsuarioID == currentUserId) || e.EsTemplate == false || esSupervisor == true)
                                          select e.Id
                                      ).ToList();

            var Preguntas = (from e in db.Pregunta join p in db.EncuestaPregunta on e.Id equals p.PreguntaID

                             where p.EncuestaID == plantillaId select e).ToList();

            ResultadosPlantillasModel model = new ResultadosPlantillasModel();
            model.idEncuestaSegundoRango = idEncuestaSegundoRango;
            model.idEncuesta = idEncuestas;
            model.preguntas = Preguntas;
            model.idPlantilla = plantillaId;
            return View(model);

        }
        [HttpGet]
        [Authorize]
        public ActionResult getResultadosEncuestaLinea(IEnumerable<int> idEncuestas, IEnumerable<int> idEncuestaSegundoRango, List<Pregunta> Preguntas ,Nullable<int> Plantillaid)
        {
            ResultadosPlantillasModel model = new ResultadosPlantillasModel();
            model.idEncuestaSegundoRango = idEncuestaSegundoRango;
            model.idEncuesta = idEncuestas;
            model.preguntas = Preguntas;
            model.idPlantilla = Plantillaid;
            return PartialView("_ResultadoEncuestaLineaP", model);

        }
        [HttpGet]
        [Authorize]
        public ActionResult GetGraficoPreguntaLinea(IEnumerable<int> idEncuestas, IEnumerable<int> idEncuestaSegundoRango,int idPregunta, Nullable<int> Plantillaid)
        {



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
                                                && idEncuestas.Contains(e.Id)
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


    }
}
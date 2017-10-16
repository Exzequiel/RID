using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using RID.DB;
using PagedList;

namespace RID.Controllers
{
    public class ReporteController : Controller
    {
        //DbContext  
        private BodMantEntities context = new BodMantEntities();
        // GET: DetalleSalida
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;
            var listadetalle = context.salida_detalle.OrderByDescending(x => x.id_detalle_salida).ToPagedList(pageNumber, pageSize);
            //var listadetalle = context.salida_detalle.ToList();
            return View(listadetalle);
        }

        //[HttpGet]
        public ActionResult ExportarSalidas()
        {
                List<salida_detalle> allsalidas = new List<salida_detalle>();
                allsalidas = context.salida_detalle.ToList();

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reporte"), "ReporteSalida.rpt"));
                rd.SetDataSource(allsalidas);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                //string savedFilename = string.Format("ReporteSalida_{0}", DateTime.Now);
                //return File(stream, "application/pdf", savedFilename);
                return File(stream, "application/pdf", "ExportarSalidas.pdf");
            }
        }
    }


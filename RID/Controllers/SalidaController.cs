using RID.DB;
using RID.Models.Salida;
using RID.Models.Reporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RID.Controllers
{
    [Authorize(Roles = "Administrador,Standard,Supervisor")]
    public class SalidaController : BaseController
    {
        public SalidaController()
        {
            ViewBag.ControllerName = "Salida";
        }
        #region Listar

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CargarTablaListaSalida()
        {
            using (var conexion = new BodMantEntities())
            {
                var list = conexion.salida.Select(x=> new ListaSalidaViewModel {
                    id_salida = x.id_salida,
                    nro_salida = x.nro_salida,
                    fecha_transaccion = x.fecha_transaccion,
                    confirmado = x.confirmado,
                    id_departamento = x.id_departamento,
                    departamento = x.departamento.descripcion,
                    activo = x.activo

                }).ToList();

                if (Convert.ToInt32(getConfiguracion("Departamento_BodMant")) != ObtenerIdDepartamentoPorUsuario())
                {
                    //var departamento = ObtenerIdDepartamentoPorUsuario();
                    //list = list.Where(x => x.id_departamento == departamento).ToList();
                }
                var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }      
        }

        [HttpGet]
        public ActionResult VerDetalleSalida(int Id)
        {
            ViewBag.Id = Id;
            return PartialView();      
        }

        [HttpGet]
        public ActionResult CargarTablaDetalleSalida(int IdSalida)
        {
            using (var context = new BodMantEntities())
            {
                var list = context.salida_detalle.Where(x => x.id_salida == IdSalida && x.activo).Select(x => new CrearDetalleSalidaViewModel
                {
                    id_salida = x.id_salida,
                    id_item = x.id_item,
                    id_detalle_salida = x.id_detalle_salida,
                    cant_aentregar = x.cant_aentregar,
                    cod_item = x.item.cod_item,
                    descripcion = x.item.descripcion,
                    maquina = x.maquina.cod_maquina+","+x.item.objeto.cod_objeto,
                    lote = x.lote.cod_lote,
                    tecnico = x.tecnico.nombre +" "+x.tecnico.apellido,
                    activo = x.activo

                }).ToList();
                var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        #endregion

        #region Crear
        [HttpGet]
        [Authorize(Roles = "Administrador,Standard")]
        public ActionResult CrearSalida()
        {
            using (var conexion = new BodMantEntities())
            {
                var IdDepartamento = ObtenerIdDepartamentoPorUsuario();
                ViewBag.ListaMaquina = conexion.maquina.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_maquina.ToString(), Text = x.descripcion_maquina + " - " + x.cod_maquina }).ToList();
                ViewBag.ListaLote = conexion.lote.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_lote.ToString(), Text = x.cod_lote }).ToList();
                ViewBag.ListaTecnico = conexion.tecnico.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_tecnico.ToString(), Text = x.nombre + " " + x.apellido }).ToList();
                ViewBag.ListaItem = conexion.item.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_item.ToString(), Text = x.cod_item + " - " + x.descripcion + " | Objeto: " + x.objeto.cod_objeto }).ToList();
                return View( new CrearSalidaViewModel {nro_salida = getConfiguracion("CorrelativoSalida"), fecha_transaccion = DateTime.Now, NombreDepartamento = ObtenerNombreDepartamentoPorUsuario() });
            }
        }

        [HttpPost]
        public ActionResult CrearSalida(CrearSalidaViewModel model)
        {
            using(var context = new BodMantEntities())
            {
                var NuevaSalida = context.salida.Add(new salida {
                    nro_salida = getConfiguracion("CorrelativoSalida"),
                    fecha_transaccion = DateTime.Now,
                    id_departamento = ObtenerIdDepartamentoPorUsuario(),
                    confirmado = false,
                    activo =true,
                    
                });

                foreach (var detalle in model.ListaDetalle??new List<CrearDetalleSalidaViewModel>())
                {
                    context.salida_detalle.Add(new salida_detalle {
                        cant_aentregar = detalle.cant_aentregar,
                        id_salida = NuevaSalida.id_salida,
                        id_item = detalle.id_item,
                        id_maquina = detalle.id_maquina,
                        id_lote = detalle.id_lote==0?(int?)null:detalle.id_lote,
                        id_tecnico = detalle.id_tecnico,
                        activo = true
                    });
                }
                var resultado = context.SaveChanges() > 0;
                if (resultado) SumarCorrelativo("CorrelativoSalida");
                return Json(EnviarResultado(resultado, "Baja Creada"), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObtenerInfoItem(int IdItem, int IdTecnico, int IdMaquina, int IdLote=0)
        {
            using (var context = new BodMantEntities())
            {
                var model = context.item.Find(IdItem);
                var modelTecnico = context.tecnico.Find(IdTecnico);
                var modelMaquina = context.maquina.Find(IdMaquina);
                var modelIdLote = context.lote.Find(IdLote);
                return Json(new CrearDetalleSalidaViewModel {
                    id_detalle_salida = 0,
                    cant_aentregar = 0,
                    id_salida = 0,
                    id_item = IdItem,
                    cod_item = model.cod_item,
                    descripcion = model.descripcion,
                    maquina = modelMaquina.cod_maquina+","+model.objeto.cod_objeto,
                    lote = modelIdLote==null?"": modelIdLote.cod_lote,
                    tecnico = modelTecnico.nombre +" "+modelTecnico.apellido,
                    id_tecnico = IdTecnico,
                    id_maquina = IdMaquina,
                    id_lote = IdLote

                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Editar
        [HttpGet]
        public ActionResult EditarSalida(int IdSalida)
        {
            using(var context = new BodMantEntities())
            {
                var ModelSalida = context.salida.Find(IdSalida);
                ViewBag.ListaMaquina = context.maquina.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_maquina.ToString(), Text = x.descripcion_maquina + " - " + x.cod_maquina }).ToList();
                ViewBag.ListaLote = context.lote.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_lote.ToString(), Text = x.cod_lote }).ToList();
                ViewBag.ListaTecnico = context.tecnico.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_tecnico.ToString(), Text = x.nombre + " " + x.apellido }).ToList();
                ViewBag.ListaItem = context.item.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_item.ToString(), Text = x.cod_item + " - " + x.descripcion + " | Objeto: " + x.objeto.cod_objeto + " |" }).ToList();

                return View("CrearSalida", new CrearSalidaViewModel
                {
                    id_salida = ModelSalida.id_salida,
                    nro_salida = ModelSalida.nro_salida,
                    NombreDepartamento = ModelSalida.departamento.descripcion,
                    fecha_transaccion = ModelSalida.fecha_transaccion,
                    EsEditar = true,
                });
            }
        }

        [HttpPost]
        public ActionResult EditarSalida(CrearSalidaViewModel model)
        {
            using (var context = new BodMantEntities())
            {
                var ModelSalida = context.salida.Find(model.id_salida);

                ModelSalida.salida_detalle.ToList().ForEach(x => x.activo = false);

                foreach (var detalle in model.ListaDetalle)
                {
                    if (ModelSalida.salida_detalle.Any(x => x.id_detalle_salida == detalle.id_detalle_salida))
                    {
                        var ModelDetalle = ModelSalida.salida_detalle.FirstOrDefault(x => x.id_detalle_salida == detalle.id_detalle_salida);
                        ModelDetalle.activo = true;
                        ModelDetalle.cant_aentregar = detalle.cant_aentregar;
                    }else
                    {
                        context.salida_detalle.Add(new salida_detalle {
                            id_salida = ModelSalida.id_salida,
                            id_item = detalle.id_item,
                            cant_aentregar = detalle.cant_aentregar,
                            id_maquina = detalle.id_maquina,
                            id_lote = detalle.id_lote,
                            id_tecnico = detalle.id_tecnico,
                            activo = true
                        });
                    }
                }
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Editar Salida"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EditarFilaDetalle(int IdDetalleSalida, int Cantidad)
        {
            using (var context = new BodMantEntities())
            {
                var model = context.salida_detalle.Find(IdDetalleSalida);
                model.cant_aentregar = Cantidad;
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Editar Detalle Salida"), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        [HttpGet]
        public ActionResult Deshabilitar(int Id)
        {
            using(var context = new BodMantEntities())
            {
                var model = context.salida.Find(Id);
                model.salida_detalle.ToList().ForEach(x => { x.activo = false; });
                model.activo = false;
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Deshabilitar Salida"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ConfirmarSalida(int Id)
        {
            using (var context = new BodMantEntities())
            {
                var model = context.salida.Find(Id);
                model.confirmado = true;

                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Confirmar Entrega"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CargarReporte()
        {
            using (var conexion = new BodMantEntities())
            {
                var list = conexion.salida_detalle.Select(x => new ListarReporteSalida
                {
                    cod_item = x.item.cod_item,
                    cant_aentregar = x.cant_aentregar,
                    descripcion = x.item.descripcion,
                    cod_objeto = x.item.objeto.cod_objeto,
                    maquina = x.maquina.descripcion_maquina,
                    lote = x.lote.cod_lote,

                }).ToList();

                var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }


    }
}
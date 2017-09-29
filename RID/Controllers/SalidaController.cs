using RID.DB;
using RID.Models.Salida;
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
                    activo = x.activo,

                }).ToList();

                if (Convert.ToInt32(getConfiguracion("Departamento_BodMant")) != ObtenerIdDepartamentoPorUsuario())
                {
                    var departamento = ObtenerIdDepartamentoPorUsuario();
                    list = list.Where(x => x.id_departamento == departamento).ToList();
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
                    //cant_disponible = x.item.cant_disponible - (x.item.entrega_detalle.Any(y => y.activo && y.entrega.confirmado == false) ? x.item.entrega_detalle.Where(y => y.activo && y.entrega.confirmado == false).Sum(z => z.cant_aentregar) : 0) - (x.item.requisa_detalle.Any(y => y.activo) ? x.item.requisa_detalle.Where(y => y.activo).Sum(z => z.cant_enviada) : 0),
                    ubicacion = x.item.ubicacion.descripcion,
                    descripcion =x.item.cod_item+" - "+ x.item.descripcion,
                    objeto = x.item.objeto.cod_objeto,
                    maquina = x.maquina.cod_maquina,
                    tecnico = x.tecnico.nombre,
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
        public ActionResult CrearSalida()
        {
            using (var conexion = new BodMantEntities())
            {
                var IdDepartamento = ObtenerIdDepartamentoPorUsuario();
                ViewBag.ListaMaquina = conexion.maquina.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_maquina.ToString(), Text = x.descripcion_maquina + " - " + x.cod_maquina }).ToList();
                ViewBag.ListaTecnico = conexion.tecnico.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_tecnico.ToString(), Text = x.nombre + " " + x.apellido }).ToList();
                //ViewBag.ListaObjeto = conexion.objeto.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_objeto.ToString(), Text = x.cod_objeto }).ToList();
                //ViewBag.ListaObjeto = new List<SelectListItem>();
                ViewBag.ListaItem = conexion.item.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_item.ToString(), Text = x.cod_item + " - " + x.descripcion + " | Ubicación: " + x.ubicacion.descripcion + " | Objeto: " + x.objeto.cod_objeto + " |" }).ToList();
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

                //foreach (var IdObjeto in model.id_objeto.ToString())
                //{
                //    context.objeto_por_salida.Add(new objeto_por_salida
                //    {
                //        id_objeto = IdObjeto,
                //        id_salida = NuevaSalida.id_salida
                //    });
                //}

                foreach (var detalle in model.ListaDetalle??new List<CrearDetalleSalidaViewModel>())
                {
                    context.salida_detalle.Add(new salida_detalle {
                        cant_aentregar = detalle.cant_aentregar,
                        id_salida = NuevaSalida.id_salida,
                        id_item = detalle.id_item,
                        id_maquina = detalle.id_maquina,
                        id_tecnico = detalle.id_tecnico,
                        activo = true
                    });
                }
                var resultado = context.SaveChanges() > 0;
                if (resultado) SumarCorrelativo("CorrelativoSalida");
                return Json(EnviarResultado(resultado, "Baja Creada"), JsonRequestBehavior.AllowGet);
            }
        }

        //public ActionResult ObtenerListaObjetosPorSalida(int IdSalida)
        //{
        //    using (var context = new BodMantEntities())
        //    {
        //        var lista = context.objeto.Where(x => x.activo && x.id_objeto == IdSalida).Select(x => new { id = x.id_objeto, text = x.cod_objeto }).ToList();
        //        return Json(new { list = lista }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        public ActionResult ObtenerInfoItem(int IdItem)
        {
            using (var context = new BodMantEntities())
            {
                var model = context.item.Find(IdItem);
                return Json(new CrearDetalleSalidaViewModel {
                    id_detalle_salida = 0,
                    cant_aentregar = 0,
                    id_salida = 0,
                    id_item = IdItem,
                    //cant_disponible = model.cant_disponible - (model.entrega_detalle.Any(y => y.activo && y.entrega.confirmado == false) ? model.entrega_detalle.Where(y => y.activo && y.entrega.confirmado == false).Sum(z => z.cant_aentregar) : 0) - (model.requisa_detalle.Any(y => y.activo) ? model.requisa_detalle.Where(y => y.activo).Sum(z => z.cant_enviada) : 0),
                    cod_item = model.cod_item,
                    descripcion = model.descripcion,
                    ubicacion = model.ubicacion.descripcion,
                    objeto = model.objeto.cod_objeto,

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
                ViewBag.ListaMaquina = context.maquina.Where(x => x.activo ).Select(x => new SelectListItem { Value = x.id_maquina.ToString(), Text = x.descripcion_maquina }).ToList();
                ViewBag.ListaTecnico = context.tecnico.Where(x => x.activo ).Select(x => new SelectListItem { Value = x.id_tecnico.ToString(), Text = x.nombre /*+ x.apellido */}).ToList();
                ViewBag.ListaItem = context.item.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_item.ToString(), Text = x.cod_item + " - " + x.descripcion + " | Ubicación: " + x.ubicacion.descripcion + " | Objeto: " + x.objeto.cod_objeto + " |" }).ToList();

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
                //ModelSalida.id_maquina = model.id_maquina;
                //ModelSalida.id_tecnico = model.id_tecnico;

                context.objeto_por_salida.RemoveRange(ModelSalida.objeto_por_salida);

                //foreach (var idobjeto in model.id_objeto) { context.objeto_por_salida.Add(new objeto_por_salida { id_objeto = idobjeto, id_salida = ModelSalida.id_salida }); }
                //ModelSalida.salida_detalle.ToList().ForEach(x => x.activo = false);

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
                            id_tecnico = detalle.id_tecnico,
                            activo = true,
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
                return Json(EnviarResultado(resultado, "Deshabilitar Entrega"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ConfirmarSalida(int Id)
        {
            using (var context = new BodMantEntities())
            {
                var model = context.salida.Find(Id);
                model.confirmado = true;
                foreach(var detalle in model.salida_detalle.Where(x=>x.activo).ToList())
                {
                    //detalle.item.cant_disponible = detalle.item.cant_disponible - detalle.cant_aentregar;
                    detalle.cant_aentregar = detalle.cant_aentregar;
                }
                var resultado = context.SaveChanges() > 0;
                return Json(EnviarResultado(resultado, "Confirmar Entrega"), JsonRequestBehavior.AllowGet);
            }
        }

    }
}
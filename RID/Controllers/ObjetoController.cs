using RID.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RID.Models.Objeto;
namespace CASMUL.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class ObjetoController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new BodMantEntities())
            {
                var list = contextCm.objeto.ToList().Select(x => new ListObjetoViewModel { Descripcion = x.descripcion_maquina, CodObjeto = x.cod_objeto, Activo = x.activo, IdObjeto = x.id_objeto });
                return View(list);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateObjetoViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    contextCm.objeto.Add(new objeto { descripcion_maquina = model.Descripcion, cod_objeto = model.CodObjeto, activo = true });
                    var result = contextCm.SaveChanges() > 0;
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(model);

                }

            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var contextCm = new BodMantEntities())
            {
                var model = contextCm.objeto.FirstOrDefault(x => x.id_objeto == id);
                return View(new EditObjetoViewModel { IdObjeto = model.id_objeto, Descripcion = model.descripcion_maquina, CodObjeto = model.cod_objeto });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditObjetoViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    var modelDb = contextCm.objeto.FirstOrDefault(x => x.id_objeto == model.IdObjeto);
                    modelDb.descripcion_maquina = model.Descripcion;
                    modelDb.cod_objeto = model.CodObjeto;
                    var result = contextCm.SaveChanges() > 0;
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se ha detectado ningun cambio !!");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(model);

                }
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var contextCm = new BodMantEntities())
            {
                var modelDb = contextCm.objeto.FirstOrDefault(x => x.id_objeto == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}
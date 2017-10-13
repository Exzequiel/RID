using RID.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RID.Models.Maquina;

namespace RID.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class MaquinaController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new BodMantEntities())
            {
                var list = contextCm.maquina.ToList().Select(x => new ListMaquinaViewModel {
                    Descripcion = x.descripcion_maquina,
                    CodMaquina = x.cod_maquina,
                    Activo = x.activo,
                    IdMaquina = x.id_maquina
                });
                return View(list);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateMaquinaViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    contextCm.maquina.Add(new maquina { descripcion_maquina = model.Descripcion, cod_maquina = model.CodMaquina, activo = true });
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
                var model = contextCm.maquina.FirstOrDefault(x => x.id_maquina == id);
                return View(new EditMaquinaViewModel { IdMaquina = model.id_maquina, Descripcion = model.descripcion_maquina, CodMaquina = model.cod_maquina });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditMaquinaViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    var modelDb = contextCm.maquina.FirstOrDefault(x => x.id_maquina == model.IdMaquina);
                    modelDb.descripcion_maquina = model.Descripcion;
                    modelDb.cod_maquina = model.CodMaquina;
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
                var modelDb = contextCm.maquina.FirstOrDefault(x => x.id_maquina == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}
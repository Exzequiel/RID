using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RID.DB;
using RID.Models.Lote;
namespace RID.Controllers
{
    [Authorize(Roles = "Administrador,Standard,Supervisor")]
    public class LoteController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new BodMantEntities())
            {
                var list = contextCm.lote.ToList().Select(x => new ListLoteVIewModel { CodLote = x.cod_lote, Activo =  x.activo, IdLote = x.id_lote });
                return View(list);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateLoteViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    if (contextCm.lote.Any(x => x.cod_lote == model.CodLote.Trim()))
                    {
                        ModelState.AddModelError("", "Lote ya existente, escriba uno diferente");
                        return View(model);

                    }

                    contextCm.lote.Add(new lote { cod_lote = model.CodLote, activo = true });
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
                var model = contextCm.lote.FirstOrDefault(x => x.id_lote == id);
                return View(new EditLoteViewModel { IdLote = model.id_lote, CodLote = model.cod_lote });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditLoteViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    if (contextCm.lote.Where(x=>x.id_lote!=model.IdLote).Any(x => x.cod_lote == model.CodLote.Trim()))
                    {
                        ModelState.AddModelError("", "Lote ya existente, escriba uno diferente");
                        return View(model);
                    }
                    var modelDb = contextCm.lote.FirstOrDefault(x => x.id_lote == model.IdLote);
                    modelDb.cod_lote = model.CodLote;
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
                var modelDb = contextCm.lote.FirstOrDefault(x => x.id_lote == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}
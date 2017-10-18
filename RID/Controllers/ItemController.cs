using RID.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RID.Models.Item;
namespace RID.Controllers
{
    [Authorize(Roles = "Administrador,Standard,Supervisor")]
    public class ItemController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new BodMantEntities())
            {
                var list = contextCm.item.ToList().Select(x => new ListItemViewModel { CodObjeto = x.cod_objeto, Descripcion = x.descripcion, activo = x.activo, IdItem = x.id_item, CodItem = x.cod_item}).ToList();
                return View(list);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateItemVIewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    if(contextCm.item.Any(x=>x.cod_item == model.CodItem.Trim()))
                    {
                        ModelState.AddModelError("", "Código de Item ya existente, escriba uno diferente");
                        return View(model);
                    }

                    contextCm.item.Add(new item { cod_item = model.CodItem, descripcion = model.Descripcion, cod_objeto = model.CodObjeto, activo = true });
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
                var model = contextCm.item.Find(id);
                return View(new EditItemViewModel {  IdItem = model.id_item,  Descripcion = model.descripcion, CodItem = model.cod_item, CodObjeto = model.cod_objeto });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditItemViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    if (contextCm.item.Where(x => x.id_item != model.IdItem).Any(x => x.cod_item == model.CodItem.Trim()))
                    {
                        ModelState.AddModelError("", "Cod item ya existente, escriba uno diferente");
                        return View(model);
                    }
                    var modelDb = contextCm.item.FirstOrDefault(x => x.id_item == model.IdItem);
                    modelDb.descripcion = model.Descripcion;
                    modelDb.cod_objeto = model.CodObjeto;
                    modelDb.cod_item = model.CodItem;
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
                var modelDb = contextCm.item.FirstOrDefault(x => x.id_item == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}
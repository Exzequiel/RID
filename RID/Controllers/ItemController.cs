using RID.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RID.Models.Item;
namespace RID.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class ItemController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new BodMantEntities())
            {
                var list = contextCm.item.ToList().Select(x => new ListItemViewModel { Objeto = x.objeto.cod_objeto, Descripcion = x.descripcion, Ubicacion = x.ubicacion.descripcion, Activo = x.activo, IdItem = x.id_item, cod_item = x.cod_item }).ToList();
                return View(list);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            using (var contextCm = new BodMantEntities())
            {
                ViewBag.SelectObjeto = contextCm.objeto.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_objeto.ToString(), Text = c.cod_objeto }).ToList();
                ViewBag.SelectUbicacion = contextCm.ubicacion.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_ubicacion.ToString(), Text = c.descripcion }).ToList();
                return View();
            }
        }
        [HttpPost]
        public ActionResult Create(CreateItemVIewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                ViewBag.SelectObjeto = contextCm.objeto.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_objeto.ToString(), Text = c.cod_objeto }).ToList();
                ViewBag.SelectUbicacion = contextCm.ubicacion.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_ubicacion.ToString(), Text = c.descripcion }).ToList();
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    if(contextCm.item.Any(x=>x.cod_item == model.cod_item.Trim()))
                    {
                        ModelState.AddModelError("", "Código de Item ya existente, escriba uno diferente");
                        return View(model);

                    }

                    contextCm.item.Add(new item { descripcion = model.Descripcion, id_ubicacion = model.IdUbicacion, id_objeto = model.IdObjeto, activo = true, cod_item = model.cod_item});
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
                ViewBag.SelectObjeto = contextCm.objeto.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_objeto.ToString(), Text = c.cod_objeto }).ToList();
                ViewBag.SelectUbicacion = contextCm.ubicacion.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_ubicacion.ToString(), Text = c.descripcion }).ToList();

                var model = contextCm.item.Find(id);
                return View(new EditItemViewModel {  IdItem = model.id_item,  Descripcion = model.descripcion, cod_item = model.cod_item, IdUbicacion = model.id_ubicacion, IdObjeto = model.id_objeto });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditItemViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {

                    ViewBag.SelectObjeto = contextCm.objeto.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_objeto.ToString(), Text = c.cod_objeto }).ToList();
                    ViewBag.SelectUbicacion = contextCm.ubicacion.Where(c => c.activo == true).ToList().Select(c => new SelectListItem { Value = c.id_ubicacion.ToString(), Text = c.descripcion }).ToList();

                    if (!ModelState.IsValid) return View(model);
                    if (contextCm.item.Where(x => x.id_item != model.IdItem).Any(x => x.cod_item == model.cod_item.Trim()))
                    {
                        ModelState.AddModelError("", "Cod item ya existente, escriba uno diferente");
                        return View(model);
                    }
                    var modelDb = contextCm.item.FirstOrDefault(x => x.id_item == model.IdItem);
                    modelDb.descripcion = model.Descripcion;
                    modelDb.id_ubicacion = model.IdUbicacion;
                    modelDb.id_objeto = model.IdObjeto;
                    modelDb.cod_item = model.cod_item;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RID.DB;
using RID.Models.Ubicacion;
namespace RID.Controllers
{
    [Authorize(Roles = "Administrador,Standard,Supervisor")]
    public class UbicacionController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new BodMantEntities())
            {
                var list = contextCm.ubicacion.ToList().Select(x => new ListUbicacionVIewModel { Description = x.descripcion, Activo =  x.activo, IdUbicacion = x.id_ubicacion });
                return View(list);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateUbicacionViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    if (contextCm.ubicacion.Any(x => x.descripcion == model.Description.Trim()))
                    {
                        ModelState.AddModelError("", "Ubicación ya existente, escriba uno diferente");
                        return View(model);

                    }

                    contextCm.ubicacion.Add(new ubicacion { descripcion = model.Description, activo = true });
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
                var model = contextCm.ubicacion.FirstOrDefault(x => x.id_ubicacion == id);
                return View(new EditUbicacionViewModel { IdUbicacion = model.id_ubicacion, Description = model.descripcion });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditUbicacionViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    if (contextCm.ubicacion.Where(x=>x.id_ubicacion!=model.IdUbicacion).Any(x => x.descripcion == model.Description.Trim()))
                    {
                        ModelState.AddModelError("", "Ubicación ya existente, escriba uno diferente");
                        return View(model);
                    }
                    var modelDb = contextCm.ubicacion.FirstOrDefault(x => x.id_ubicacion == model.IdUbicacion);
                    modelDb.descripcion = model.Description;
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
                var modelDb = contextCm.ubicacion.FirstOrDefault(x => x.id_ubicacion == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}
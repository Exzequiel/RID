using RID.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RID.Models.Tecnico;
namespace RID.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class TecnicoController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new BodMantEntities())
            {
                var list = contextCm.tecnico.ToList().Select(x => new ListTecnicoViewModel { Nombre = x.nombre, Apellido = x.apellido, Puesto = x.puesto, Activo = x.activo, IdTecnico = x.id_tecnico });
                return View(list);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateTecnicoViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    contextCm.tecnico.Add(new tecnico { nombre = model.Nombre, apellido = model.Apellido, puesto = model.Puesto, activo = true });
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
                var model = contextCm.tecnico.FirstOrDefault(x => x.id_tecnico == id);
                return View(new EditTecnicoViewModel { IdTecnico = model.id_tecnico, Nombre = model.nombre, Apellido = model.apellido, Puesto = model.puesto });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditTecnicoViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    var modelDb = contextCm.tecnico.FirstOrDefault(x => x.id_tecnico == model.IdTecnico);
                    modelDb.nombre = model.Nombre;
                    modelDb.apellido = model.Apellido;
                    modelDb.puesto = model.Puesto;
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
                var modelDb = contextCm.tecnico.FirstOrDefault(x => x.id_tecnico == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RID.DB;
using RID.Models.Departamento;
namespace RID.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class DepartamentoController : Controller
    {
        public ActionResult Index()
        {
            using (var contextCm = new BodMantEntities())
            {
                var list = contextCm.departamento.ToList().Select(x => new ListDepartamentoViewModel { Description = x.descripcion, Activo =  x.activo, IdDepartamento = x.id_departamento });
                return View(list);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateDepartamentoViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    if (contextCm.departamento.Any(x => x.descripcion == model.Description.Trim()))
                    {
                        ModelState.AddModelError("", "Departamento ya existente, escriba uno diferente");
                        return View(model);

                    }

                    contextCm.departamento.Add(new departamento { descripcion = model.Description, activo = true });
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
                var model = contextCm.departamento.FirstOrDefault(x => x.id_departamento == id);
                return View(new EditDepartamentoViewModel { IdDepartamento = model.id_departamento, Description = model.descripcion });
            }
        }
        [HttpPost]
        public ActionResult Edit(EditDepartamentoViewModel model)
        {
            using (var contextCm = new BodMantEntities())
            {
                try
                {
                    if (!ModelState.IsValid) return View(model);
                    if (contextCm.departamento.Where(x=>x.id_departamento!=model.IdDepartamento).Any(x => x.descripcion == model.Description.Trim()))
                    {
                        ModelState.AddModelError("", "Departamento ya existente, escriba uno diferente");
                        return View(model);
                    }
                    var modelDb = contextCm.departamento.FirstOrDefault(x => x.id_departamento == model.IdDepartamento);
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
                var modelDb = contextCm.departamento.FirstOrDefault(x => x.id_departamento == id);
                modelDb.activo = !modelDb.activo;
                var result = contextCm.SaveChanges() > 0;
                return RedirectToAction("Index");
            }
        }
    }
}
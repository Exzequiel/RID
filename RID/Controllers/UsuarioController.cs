using RID;
using RID.Controllers;
using RID.DB;
using RID.Models;
using RID.Models.Base;
using RID.Models.Usuario;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RID.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class UsuarioController : BaseController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // GET: Usuario
        public ActionResult Index()
        {
            return View();
            //var name = User.Identity.Name;
        }

        public ActionResult ListarUsuarios()
        {
            try
            {
                using (var context = new BodMantEntities())
                {
                    var listaUsuarios = context.usuario.Select(x => new ListaUsuarioViewModel {
                        Id = x.id_usuario,
                        Nombre = x.nombre,
                        Apellido = x.apellido,
                        Email = x.AspNetUsers.Email,
                        Estado = x.activo,
                        Perfil = x.AspNetUsers.AspNetRoles.Any() ? x.AspNetUsers.AspNetRoles.FirstOrDefault().Name : "",
                        UserName = x.AspNetUsers.UserName,
                        NombreUbicacion = x.ubicacion!=null?x.ubicacion.descripcion:""
                        //esAdmin = x.AspNetUsers.AspNetRoles.Any(z=>z.Name=="Administrador")
                    }).ToList();

                    var jsonResult = Json(listaUsuarios, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = Int32.MaxValue;
                    return jsonResult;
                }
            }
            catch(Exception e) {
                return View();
            }
        }

            
        [HttpGet]
        public ActionResult CrearUsuario()
        {
            using (var context = new BodMantEntities())
            {
                ViewBag.ListaTipoUsuario = context.AspNetRoles.Where(x=>x.activo??false).Select(x => new SelectListItem { Value = x.Id, Text = x.Name }).ToList();
                ViewBag.ListaUbicacion = context.ubicacion.Where(x => x.activo).Select(x => new SelectListItem {Value=x.id_ubicacion.ToString(), Text=x.descripcion }).ToList();
                return View(new CrearUsuarioViewModel { });
            }
           
        }

        [HttpPost]
        public async Task<ActionResult> CrearUsuario(CrearUsuarioViewModel model)
        {
            try
            {
                using (var context = new BodMantEntities())
                {
                    var user = new ApplicationUser { UserName = model.UserName.Trim(), Email = model.Email.Trim() };
                    var result = await UserManager.CreateAsync(user, model.Password.Trim());
                    if (result.Succeeded)
                    {
                        await UserManager.AddToRoleAsync(user.Id, context.AspNetRoles.Find(model.RoleUsuario).Name);
                        var test = context.usuario.Add(new usuario
                        {
                            nombre = model.Nombre.Trim(),
                            apellido = model.Apellido.Trim(),
                            IdAspnetUser = user.Id,
                            activo=true,
                            cuenta_usuario = model.UserName,
                            email = model.Email,
                            id_ubicacion = model.IdUbicacion
                          
                        });

                        var resultado = context.SaveChanges() > 0;
                        return Json(EnviarResultado(resultado, "Crear Usuario"), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(EnviarResultado(result.Succeeded, result.Errors.FirstOrDefault()), JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception e)
            {
                return Json( EnviarResultado(false, e.Message), JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult EditarInfoUsuario(int Id)
        {
            using (var context = new BodMantEntities())
            {
                var usuario = context.usuario.Find(Id);
                return PartialView(new CrearUsuarioViewModel
                {
                    Nombre = usuario.nombre,
                    Apellido = usuario.apellido,
                });
            }
        }

        [HttpPost]
        public ActionResult EditarInfoUsuario(CrearUsuarioViewModel model)
        {
            using (var context = new BodMantEntities())
            {
                var usuario = context.usuario.Find(model.Id);
                usuario.nombre = model.Nombre;
                usuario.apellido = model.Apellido;
                context.Entry(usuario).State = EntityState.Modified;
                var result = context.SaveChanges() > 0;
                return Json(new MensajeRespuestaViewModel
                {
                    Titulo = "Editar Usuario",
                    Mensaje = result ? "Se edito Satisfactoriamente" : "Error al editar el usuario",
                    Estado = result
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public  ActionResult EditarCuentaUsuario(int Id)
        {
            using (var context = new BodMantEntities())
            {
                ViewBag.ListaTipoUsuario = context.AspNetRoles.Where(x=>x.activo??false).Select(x => new SelectListItem { Value = x.Id, Text = x.Name }).ToList();
                ViewBag.ListaUbicacion = context.ubicacion.Where(x => x.activo).Select(x => new SelectListItem { Value = x.id_ubicacion.ToString(), Text = x.descripcion }).ToList();
                var usuario = context.usuario.Find(Id);
                return PartialView(new CrearUsuarioViewModel
                {
                    Id = usuario.id_usuario,
                    UserName = usuario.AspNetUsers.UserName,
                    Email = usuario.AspNetUsers.Email,
                    IdAspNetUser = usuario.IdAspnetUser,
                    RoleUsuario = usuario.AspNetUsers.AspNetRoles.FirstOrDefault()?.Id??"",  
                    Estado = usuario.activo,
                    IdUbicacion = usuario.id_ubicacion
                });

            }
        }


        [HttpPost]
        public async Task<ActionResult> EditarCuentaUsuario(CrearUsuarioViewModel model)
        {
            try
            {
                using (var context = new BodMantEntities())
                {
                    var usuario = context.usuario.Find(model.Id);
                    usuario.AspNetUsers.UserName = model.UserName;
                    usuario.AspNetUsers.Email = model.Email;
                    usuario.activo = model.Estado;
                    usuario.id_ubicacion = model.IdUbicacion;
                    context.Entry(usuario).State = EntityState.Modified;
                    var roles = await UserManager.GetRolesAsync(usuario.AspNetUsers.Id);
                    await UserManager.RemoveFromRolesAsync(usuario.AspNetUsers.Id, roles.ToArray());
                    var result2 = await UserManager.AddToRoleAsync(usuario.AspNetUsers.Id, context.AspNetRoles.Find(model.RoleUsuario).Name);
                   
                    var result = context.SaveChanges() > 0;
                    return Json(new MensajeRespuestaViewModel
                    {
                        Titulo = "Editar Usuario",
                        Mensaje = result && result2.Succeeded ? "Se edito Satisfactoriamente" : "Error al editar el usuario",
                        Estado = result
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new MensajeRespuestaViewModel
                {
                    Titulo = "Editar Usuario",
                    Mensaje = "Error al editar el usuario",
                    Estado = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeshabilitarUsuario(int Id)
        {
            using (var context = new BodMantEntities())
            {
                var usuario = context.usuario.Find(Id);
                usuario.activo = false;
                           
                var result = context.SaveChanges() > 0;
                return Json(new MensajeRespuestaViewModel
                {
                    Titulo = "Deshabilitar Usuario",
                    Mensaje = result ? "Se deshabilito Satisfactoriamente" : "Error al deshabilitar el usuario",
                    Estado = result
                }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public ActionResult ResetContrasena(int Id)
        {
            using (var context = new BodMantEntities())
            {
                return PartialView(new CambiarContrasenaViewModel { IdUser = Id });
            }
        }


        [HttpPost]
        public async Task<ActionResult> ResetContrasena(CambiarContrasenaViewModel model)
        {
            using (var context = new BodMantEntities())
            {
                var User = context.usuario.Find(model.IdUser);
                string code = await UserManager.GeneratePasswordResetTokenAsync(User.IdAspnetUser);
                var result = await UserManager.ResetPasswordAsync(User.IdAspnetUser, code, model.NewPassword);

                return Json(new MensajeRespuestaViewModel
                {
                    Titulo = "Cambiar Contrasena",
                    Mensaje = result.Succeeded ? "Se cambio Satisfactoriamente" : "Error al cambiar la contrasena",
                    Estado = result.Succeeded
                }, JsonRequestBehavior.AllowGet);
            }
        }

        
        [HttpGet]
        public ActionResult DetalleUsuario()
        {
            using (var context = new BodMantEntities())
            {
                int idUsuario = ObtenerIdUsuario();
                var Usuario = context.usuario.FirstOrDefault(x => x.id_usuario == idUsuario);

                var  detalleUsuario = new DetalleUsuarioViewModel { PersonaID = Usuario.id_usuario,
                                                                    Nombre = Usuario.nombre,
                                                                    Apellido=Usuario.apellido,
                                                                  };
                return View(detalleUsuario); 
            }
        }
    }
}
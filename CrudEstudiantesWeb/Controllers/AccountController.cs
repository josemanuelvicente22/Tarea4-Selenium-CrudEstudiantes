using CrudEstudiantesWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrudEstudiantesWeb.Controllers
{
    public class AccountController : Controller
    {
        // Credenciales fijas
        private const string USER = "admin";
        private const string PASS = "admin123";

        public IActionResult Login()
        {
            // Si ya está logueado, lo mando al listado
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                return RedirectToAction("Index", "Estudiantes");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Usuario == USER && model.Clave == PASS)
            {
                HttpContext.Session.SetString("IsLoggedIn", "true");
                HttpContext.Session.SetString("Usuario", model.Usuario);

                return RedirectToAction("Index", "Estudiantes");
            }

            model.MensajeError = "Usuario o contraseña incorrectos.";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

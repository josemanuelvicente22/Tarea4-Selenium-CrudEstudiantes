using CrudEstudiantesWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrudEstudiantesWeb.Controllers
{
    public class AccountController : Controller
    {
        private const string USER = "admin";
        private const string PASS = "admin123";

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (model.Usuario == USER && model.Clave == PASS)
            {
                // Por simplicidad, guardamos un flag en sesión
                HttpContext.Session.SetString("IsLoggedIn", "true");
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

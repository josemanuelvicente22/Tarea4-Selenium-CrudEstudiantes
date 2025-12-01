using CrudEstudiantesWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CrudEstudiantesWeb.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly EstudianteService _service;

        public EstudiantesController(EstudianteService service)
        {
            _service = service;
        }

        private bool UsuarioNoLogueado()
        {
            return HttpContext.Session.GetString("IsLoggedIn") != "true";
        }

        public IActionResult Index()
        {
            if (UsuarioNoLogueado())
                return RedirectToAction("Login", "Account");

            var lista = _service.GetAll();
            return View(lista);
        }

        public IActionResult Create()
        {
            if (UsuarioNoLogueado())
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Estudiante estudiante)
        {
            if (UsuarioNoLogueado())
                return RedirectToAction("Login", "Account");

            // Validaciones de duplicados
            if (_service.MatriculaExiste(estudiante.Matricula))
            {
                ModelState.AddModelError("Matricula", "Ya existe un estudiante con esa matrícula.");
            }

            if (_service.CorreoExiste(estudiante.Correo))
            {
                ModelState.AddModelError("Correo", "Ya existe un estudiante con ese correo electrónico.");
            }

            if (ModelState.IsValid)
            {
                _service.Create(estudiante);
                return RedirectToAction(nameof(Index));
            }

            return View(estudiante);
        }


        public IActionResult Edit(int id)
        {
            if (UsuarioNoLogueado())
                return RedirectToAction("Login", "Account");

            var est = _service.GetById(id);
            if (est == null) return NotFound();
            return View(est);
        }

        [HttpPost]
        public IActionResult Edit(Estudiante estudiante)
        {
            if (UsuarioNoLogueado())
                return RedirectToAction("Login", "Account");

            // Validaciones de duplicados excluyendo el mismo Id
            if (_service.MatriculaExiste(estudiante.Matricula, estudiante.Id))
            {
                ModelState.AddModelError("Matricula", "Ya existe otro estudiante con esa matrícula.");
            }

            if (_service.CorreoExiste(estudiante.Correo, estudiante.Id))
            {
                ModelState.AddModelError("Correo", "Ya existe otro estudiante con ese correo electrónico.");
            }

            if (ModelState.IsValid)
            {
                _service.Update(estudiante);
                return RedirectToAction(nameof(Index));
            }

            return View(estudiante);
        }


        public IActionResult Delete(int id)
        {
            if (UsuarioNoLogueado())
                return RedirectToAction("Login", "Account");

            var est = _service.GetById(id);
            if (est == null) return NotFound();
            return View(est);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (UsuarioNoLogueado())
                return RedirectToAction("Login", "Account");

            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

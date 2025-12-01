using CrudEstudiantesWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrudEstudiantesWeb.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly EstudianteService _service;

        public EstudiantesController(EstudianteService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var lista = _service.GetAll();
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                _service.Create(estudiante);
                return RedirectToAction(nameof(Index));
            }
            return View(estudiante);
        }

        public IActionResult Edit(int id)
        {
            var est = _service.GetById(id);
            if (est == null) return NotFound();
            return View(est);
        }

        [HttpPost]
        public IActionResult Edit(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                _service.Update(estudiante);
                return RedirectToAction(nameof(Index));
            }
            return View(estudiante);
        }

        public IActionResult Delete(int id)
        {
            var est = _service.GetById(id);
            if (est == null) return NotFound();
            return View(est);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

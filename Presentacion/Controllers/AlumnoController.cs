using Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Presentacion.Helpers;

namespace Presentacion.Controllers
{
    [Authorize]
    public class AlumnoController : Controller
    {
        private readonly AlumnoService _alumnoService;

        public AlumnoController(AlumnoService alumnoService)
        {
            _alumnoService = alumnoService;
        }

        [HttpGet]
        public IActionResult Index(
            string? buscarDni,
            string? buscarApellido)
        {
            List<AlumnoListadoItem> alumnos =
                _alumnoService.ListarAlumnos(
                    buscarDni,
                    buscarApellido);

            ViewBag.BuscarDni = buscarDni;
            ViewBag.BuscarApellido = buscarApellido;

            return View(alumnos);
        }

        [HttpGet]
        public IActionResult Alta()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Alta(Alumno alumno)
        {
            if (!ModelState.IsValid)
                return View(alumno);

            try
            {
                _alumnoService.AltaAlumno(alumno);
                TempData["Mensaje"] =
                    $"Alumno {alumno.Nombre} {alumno.Apellido} registrado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error =
                    "Ocurrió un error: " + Errores.Describir(ex, "MongoDB");
                return View(alumno);
            }
        }

        [HttpGet]
        public IActionResult Editar(string id)
        {
            try
            {
                Alumno? alumno = _alumnoService.ObtenerAlumno(id);
                if (alumno == null)
                {
                    TempData["Error"] = "No se encontró el alumno.";
                    return RedirectToAction("Index");
                }

                return View(alumno);
            }
            catch (Exception ex)
            {
                TempData["Error"] = Errores.Describir(ex, "MongoDB");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Alumno alumno)
        {
            if (!ModelState.IsValid)
                return View(alumno);

            try
            {
                _alumnoService.ActualizarAlumno(alumno);
                TempData["Mensaje"] =
                    $"Alumno {alumno.Nombre} {alumno.Apellido} actualizado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error =
                    "Ocurrió un error: " + Errores.Describir(ex, "MongoDB");
                return View(alumno);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(string id)
        {
            try
            {
                _alumnoService.EliminarAlumno(id);
                TempData["Mensaje"] =
                    "Alumno eliminado junto con sus cursos.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = Errores.Describir(ex, "MongoDB");
            }

            return RedirectToAction("Index");
        }
    }
}

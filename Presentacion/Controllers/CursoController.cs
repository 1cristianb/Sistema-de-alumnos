using Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Presentacion.Helpers;

namespace Presentacion.Controllers
{
    [Authorize]
    public class CursoController : Controller
    {
        private readonly AlumnoService _alumnoService;
        private readonly CursoService _cursoService;

        public CursoController(
            AlumnoService alumnoService,
            CursoService cursoService)
        {
            _alumnoService = alumnoService;
            _cursoService = cursoService;
        }

        [HttpGet]
        public IActionResult Alta(string alumnoId)
        {
            if (!CargarAlumno(alumnoId))
                return RedirectToAction("Index", "Alumno");

            return View(new Curso
            {
                AlumnoId = alumnoId,
                Anio = DateTime.Now.Year
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Alta(Curso curso)
        {
            if (!CargarAlumno(curso.AlumnoId))
                return RedirectToAction("Index", "Alumno");

            if (!ModelState.IsValid)
                return View(curso);

            try
            {
                _cursoService.AltaCurso(curso);
                TempData["Mensaje"] =
                    $"Curso \"{curso.NombreCurso}\" asociado correctamente al alumno.";
                return RedirectToAction("Index", "Alumno");
            }
            catch (Exception ex)
            {
                ViewBag.Error =
                    "Ocurrió un error: " + Errores.Describir(ex);
                return View(curso);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(string id)
        {
            try
            {
                _cursoService.EliminarCurso(id);
                TempData["Mensaje"] = "Curso eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = Errores.Describir(ex);
            }

            return RedirectToAction("Index", "Alumno");
        }

        private bool CargarAlumno(string alumnoId)
        {
            try
            {
                Alumno? alumno = _alumnoService.ObtenerAlumno(alumnoId);
                if (alumno == null)
                {
                    TempData["Error"] = "No se encontró el alumno.";
                    return false;
                }

                ViewBag.Alumno = alumno;
                return true;
            }
            catch (Exception ex)
            {
                TempData["Error"] = Errores.Describir(ex);
                return false;
            }
        }
    }
}

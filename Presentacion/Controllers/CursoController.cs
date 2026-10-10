using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Negocio;
using Presentacion.Helpers;

namespace Presentacion.Controllers
{
    [Authorize]
    public class CursoController : Controller
    {
        private readonly AlumnoService _alumnoService = new AlumnoService();
        private readonly CursoService _cursoService = new CursoService();
        private readonly IConfiguration _configuracion;

        public CursoController(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        // Alta de un curso para un alumno concreto (se identifica por alumnoId + la base donde vive)
        [HttpGet]
        public IActionResult Alta(string alumnoId, string tipoBD)
        {
            if (!CargarAlumno(alumnoId, tipoBD))
                return RedirectToAction("Index", "Alumno");

            return View(new Curso { AlumnoId = alumnoId, Anio = DateTime.Now.Year });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Alta(Curso curso, string tipoBD)
        {
            if (!CargarAlumno(curso.AlumnoId, tipoBD))
                return RedirectToAction("Index", "Alumno");

            if (!ModelState.IsValid)
                return View(curso);

            try
            {
                _cursoService.AltaCurso(curso, tipoBD, _configuracion.ObtenerCadena(tipoBD));
                TempData["Mensaje"] = $"Curso \"{curso.NombreCurso}\" asociado correctamente al alumno en {tipoBD}.";
                return RedirectToAction("Index", "Alumno");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error: " + Errores.Describir(ex, tipoBD);
                return View(curso);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(string id, string tipoBD)
        {
            try
            {
                _cursoService.EliminarCurso(id, tipoBD, _configuracion.ObtenerCadena(tipoBD));
                TempData["Mensaje"] = "Curso eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = Errores.Describir(ex, tipoBD);
            }
            return RedirectToAction("Index", "Alumno");
        }

        // Carga el alumno en ViewBag para mostrar a quién se le agrega el curso
        private bool CargarAlumno(string alumnoId, string tipoBD)
        {
            try
            {
                var alumno = _alumnoService.ObtenerAlumno(alumnoId, tipoBD, _configuracion.ObtenerCadena(tipoBD));
                if (alumno == null)
                {
                    TempData["Error"] = "No se encontró el alumno.";
                    return false;
                }

                ViewBag.Alumno = alumno;
                ViewBag.TipoBD = tipoBD;
                return true;
            }
            catch (Exception ex)
            {
                TempData["Error"] = Errores.Describir(ex, tipoBD);
                return false;
            }
        }
    }
}

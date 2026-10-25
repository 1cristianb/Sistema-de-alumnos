using Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Presentacion.Helpers;

namespace Presentacion.Controllers
{
    public class AlumnoController : Controller
    {
        private readonly AlumnoService _alumnoService;
        private readonly IConfiguration _configuracion;

        public AlumnoController(
            AlumnoService alumnoService,
            IConfiguration configuracion)
        {
            _alumnoService = alumnoService;
            _configuracion = configuracion;
        }

        // Listado: por defecto muestra los alumnos de AMBAS bases; el combo filtra por base.
        [HttpGet]
        public IActionResult Index(string? buscarDni, string? buscarApellido, string tipoBD = "Todas")
        {
            var bases = _configuracion.ObtenerBases(tipoBD).Keys;
            var resultado = _alumnoService.ListarAlumnos(bases, buscarDni, buscarApellido);

            ViewBag.TipoBDActual = tipoBD;
            ViewBag.BuscarDni = buscarDni;
            ViewBag.BuscarApellido = buscarApellido;
            ViewBag.Advertencias = resultado.Advertencias;

            return View(resultado.Items);
        }

        [HttpGet]
        public IActionResult Alta()
        {
            ViewBag.TipoBD = "LiteDB";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Alta(Alumno alumno, string tipoBD)
        {
            ViewBag.TipoBD = tipoBD;

            if (!ModelState.IsValid)
                return View(alumno);

            try
            {
                _alumnoService.AltaAlumno(alumno, tipoBD);
                TempData["Mensaje"] = $"Alumno {alumno.Nombre} {alumno.Apellido} registrado correctamente en {tipoBD}.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error: " + Errores.Describir(ex, tipoBD);
                return View(alumno);
            }
        }

        [HttpGet]
        public IActionResult Editar(string id, string tipoBD)
        {
            try
            {
                var alumno = _alumnoService.ObtenerAlumno(id, tipoBD);
                if (alumno == null)
                {
                    TempData["Error"] = "No se encontró el alumno.";
                    return RedirectToAction("Index");
                }

                ViewBag.TipoBD = tipoBD;
                return View(alumno);
            }
            catch (Exception ex)
            {
                TempData["Error"] = Errores.Describir(ex, tipoBD);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Alumno alumno, string tipoBD)
        {
            ViewBag.TipoBD = tipoBD;

            if (!ModelState.IsValid)
                return View(alumno);

            try
            {
                _alumnoService.ActualizarAlumno(alumno, tipoBD);
                TempData["Mensaje"] = $"Alumno {alumno.Nombre} {alumno.Apellido} actualizado correctamente en {tipoBD}.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error: " + Errores.Describir(ex, tipoBD);
                return View(alumno);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(string id, string tipoBD)
        {
            try
            {
                _alumnoService.EliminarAlumno(id, tipoBD);
                TempData["Mensaje"] = $"Alumno eliminado de {tipoBD} junto con sus cursos.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = Errores.Describir(ex, tipoBD);
            }
            return RedirectToAction("Index");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Phoenixx99JD_Zeuspet.Web.Services;

namespace Phoenixx99JD_Zeuspet.Web.Controllers;

[ApiController]
[Route("api/estadisticas")]
public class EstadisticasController : ControllerBase
{
    private readonly VeterinariaService _service;

    public EstadisticasController(VeterinariaService service) => _service = service;

    [HttpGet]
    public ActionResult<Dictionary<string, object>> Obtener() => _service.ObtenerEstadisticas();
}

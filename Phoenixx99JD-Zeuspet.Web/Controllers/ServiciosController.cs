using Microsoft.AspNetCore.Mvc;
using Phoenixx99JD_Zeuspet.Web.Models;
using Phoenixx99JD_Zeuspet.Web.Services;

namespace Phoenixx99JD_Zeuspet.Web.Controllers;

[ApiController]
[Route("api/servicios")]
public class ServiciosController : ControllerBase
{
    private readonly VeterinariaService _service;

    public ServiciosController(VeterinariaService service) => _service = service;

    [HttpGet]
    public ActionResult<List<object>> Listar()
    {
        var servicios = new List<object>
        {
            new { tipo = "ConsultaGeneral", descripcion = new ConsultaGeneral().Descripcion },
            new { tipo = "Vacunacion", descripcion = new Vacunacion().Descripcion }
        };
        return servicios;
    }

    [HttpPost("atender")]
    public ActionResult<string> Atender([FromBody] AtenderRequest request)
    {
        var mascota = _service.BuscarMascotaPorId(request.MascotaId);
        if (mascota == null) return NotFound("Mascota no encontrada.");

        try
        {
            return _service.AtenderServicio(request.Tipo, mascota.Nombre);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public record AtenderRequest(string Tipo, string MascotaId);

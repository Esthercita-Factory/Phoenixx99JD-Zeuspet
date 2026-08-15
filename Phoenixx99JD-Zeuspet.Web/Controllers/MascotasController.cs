using Microsoft.AspNetCore.Mvc;
using Phoenixx99JD_Zeuspet.Web.Models;
using Phoenixx99JD_Zeuspet.Web.Services;

namespace Phoenixx99JD_Zeuspet.Web.Controllers;

[ApiController]
[Route("api/mascotas")]
public class MascotasController : ControllerBase
{
    private readonly VeterinariaService _service;

    public MascotasController(VeterinariaService service) => _service = service;

    [HttpGet]
    public ActionResult<List<Mascota>> Listar() => _service.ListarMascotas();

    [HttpGet("cliente/{clienteId}")]
    public ActionResult<List<Mascota>> ListarDeCliente(string clienteId) => _service.ListarMascotasDeCliente(clienteId);

    [HttpGet("{id}")]
    public ActionResult<Mascota> Obtener(string id)
    {
        var mascota = _service.BuscarMascotaPorId(id);
        return mascota == null ? NotFound() : mascota;
    }

    [HttpPost]
    public ActionResult<Mascota> Crear([FromBody] MascotaRequest request)
    {
        try
        {
            var mascota = _service.AgregarMascota(request.Nombre, request.Especie, request.Raza, request.Edad, request.ClienteId);
            return CreatedAtAction(nameof(Obtener), new { id = mascota.Id }, mascota);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Modificar(string id, [FromBody] MascotaRequest request)
    {
        var ok = _service.ModificarMascota(id, request.Nombre, request.Especie, request.Raza, request.Edad);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Eliminar(string id)
    {
        return _service.EliminarMascota(id) ? NoContent() : NotFound();
    }
}

public record MascotaRequest(string Nombre, string Especie, string Raza, int Edad, string ClienteId);

using Microsoft.AspNetCore.Mvc;
using Phoenixx99JD_Zeuspet.Web.Models;
using Phoenixx99JD_Zeuspet.Web.Services;

namespace Phoenixx99JD_Zeuspet.Web.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly VeterinariaService _service;

    public ClientesController(VeterinariaService service) => _service = service;

    [HttpGet]
    public ActionResult<List<Cliente>> Listar() => _service.ListarClientes();

    [HttpGet("{id}")]
    public ActionResult<Cliente> Obtener(string id)
    {
        var cliente = _service.BuscarClientePorId(id);
        return cliente == null ? NotFound() : cliente;
    }

    [HttpPost]
    public ActionResult<Cliente> Crear([FromBody] ClienteRequest request)
    {
        var cliente = _service.AgregarCliente(request.Nombre, request.Edad, request.Telefono, request.Email, request.Direccion);
        return CreatedAtAction(nameof(Obtener), new { id = cliente.Id }, cliente);
    }

    [HttpPut("{id}")]
    public IActionResult Modificar(string id, [FromBody] ClienteRequest request)
    {
        var ok = _service.ModificarCliente(id, request.Nombre, request.Edad, request.Telefono, request.Email, request.Direccion);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Eliminar(string id)
    {
        return _service.EliminarCliente(id) ? NoContent() : NotFound();
    }
}

public record ClienteRequest(string Nombre, int Edad, string Telefono, string Email, string Direccion);

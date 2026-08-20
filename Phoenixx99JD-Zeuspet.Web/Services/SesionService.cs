namespace Phoenixx99JD_Zeuspet.Web.Services;

public sealed class SesionService
{
    private string _rol = "";

    public string Rol
    {
        get => _rol;
        set
        {
            if (_rol == value) return;

            _rol = value;
            OnCambio?.Invoke();
        }
    }

    public event Action? OnCambio;
}

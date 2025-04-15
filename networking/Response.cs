using Agentie_turism_transport_csharp.networking;

namespace networking;

public class Response
{
    public ResponseType Type { get; set; }
    public string ErrorMessage { get; set; }
    public object Data { get; set; } // Poate fi orice obiect din domeniu

    public override string ToString()
    {
        return $"[type={Type}, error={ErrorMessage}, data={Data}]";
    }
}
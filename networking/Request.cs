using Agentie_turism_transport_csharp.networking;

namespace networking;

public class Request
{
    public RequestType Type { get; set; }
    public object Data { get; set; } // Poate fi orice obiect din domeniu

    public override string ToString()
    {
        return $"Request[type={Type}, data={Data}]";
    }
}
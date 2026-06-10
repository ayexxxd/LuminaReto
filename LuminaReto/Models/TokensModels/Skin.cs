using System.Text.Json.Serialization;

public class SkinData
{
    public int? IdSkin { get; set; }
    public int IdJuego { get; set; }

    [JsonPropertyName("Atributo")]
    public string Imagen { get; set; }

    public string Nombre { get; set; }
    public string ColorHex { get; set; }
    public int Costo { get; set; }
    public bool PuedeComprar { get; set; }
    public int TokensFaltantes { get; set; }
    public bool Owned { get; set; }
}
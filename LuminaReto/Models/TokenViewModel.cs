using System.Collections.Generic;

public class TokenViewModel
{
    public int WhirlTokens { get; set; }
    public int GanadosMes { get; set; }
    public string UltimaRecompensa { get; set; }

    public string DateFilter { get; set; } = "todas";
    public string TypeFilter { get; set; } = "todas";

    public List<Transaccion> ListaTransacciones { get; set; } = new();
    public List<Recompensa> ListaRecompensas { get; set; } = new();
}
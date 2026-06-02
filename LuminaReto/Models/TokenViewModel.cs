using System.Collections.Generic;

public class TokenViewModel
{
    public int IdUser { get; set; }
    public string WhirlTokens { get; set; }
    public string GanadosMes { get; set; }
    public string UltimaRecompensa { get; set; }

    public string DateFilter { get; set; } = "todas";
    public string TypeFilter { get; set; } = "todas";

    public List<Transaccion> ListaTransacciones { get; set; } = new();
    public List<Recompensa> ListaRecompensas { get; set; } = new();
}
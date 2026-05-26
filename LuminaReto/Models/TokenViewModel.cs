using System.Collections.Generic;

public class TokenViewModel
{
    public int WhirlTokens { get; set; }
    public int GanadosMes { get; set; }
    public int ProximaRecompensa { get; set; }
    public int TargetProgreso { get; set; }
    public int PorcentajeProgreso { get; set; }
    public int RestantesProgreso { get; set; }
    
    public List<Transaccion> ListaTransacciones { get; set; } = new();
    public List<Recompensa> ListaRecompensas { get; set; } = new();
}
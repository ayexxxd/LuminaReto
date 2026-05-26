using System;

public class Recompensa
{

    public int Id { get; set; }

    public string NombreRecompensa { get; set; }

    public string Imagen { get; set; }

    public int Costo { get; set; }

    public bool PuedeCanjear { get; set; }

    public int TokensFaltantes { get; set; }
}
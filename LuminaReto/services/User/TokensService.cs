using System.Text;
using System.Text.Json;

public class TokensService : ITokensService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public TokensService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task UpdatePoints(int id, int points)
    {
    //var url = "https://127.0.0.1:5010/updatepoints";
    var url = "https://10.14.255.45:5010/updatepoints";

    var body = new
    {
        idUser = id,
        points = points
    };
        await _httpClient.PutAsJsonAsync(url, body);
    }

    public async Task<List<Recompensa>> GetRecompensas()
    {
        //string url = "https://127.0.0.1:5010/recompensas";
        string url = "https://10.14.255.45:5010/recompensas";
         
        var listaRecompensas = await _httpClient.GetFromJsonAsync<List<Recompensa>>(url, _jsonOptions);
        return listaRecompensas ?? new List<Recompensa>();
    }

    public async Task<List<Transaccion>> GetTransacciones(int id, string date)
    {
        //var url = "https://127.0.0.1:5010/transacciones/" + id + "/" + date;
        var url = "https://10.14.255.45:5010/transacciones/" + id + "/" + date;

        var listaTransacciones = await _httpClient.GetFromJsonAsync<List<Transaccion>>(url, _jsonOptions);
        return listaTransacciones ?? new List<Transaccion>();
    }

    public async Task<int> GetUserPoints(int id)
    {
        var url = "https://127.0.0.1:5010/getpoints/" + id;
        //var url = "https://10.14.255.45:5010/getpoints/" + id;

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return 0;
        var responseJson = await response.Content.ReadAsStringAsync();
        return int.Parse(responseJson);
    }
    public async Task<int> GetUserPointsMonth(int id)
    {
        var url = "https://127.0.0.1:5010/getpointsMes/" + id;
        //var url = "https://10.14.255.45:5010/getpointsMes/" + id;

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return 0;
        var responseJson = await response.Content.ReadAsStringAsync();
        return int.Parse(responseJson);
    }

    public async Task CrearTransaccion(int userId,int? recompensaId,int monto,string descripcion)
    {
        var url = "https://127.0.0.1:5010/transaccion";
        //var url = "https://10.14.255.45:5010/transaccion";

        var body = new
        {
            idUser = userId,
            idReward = recompensaId,
            monto = monto,
            description = descripcion
        };
        await _httpClient.PostAsJsonAsync(url, body);
    }

    public async Task<string> GetUltimaRecompensa(int userId)
    {
        var url = "https://127.0.0.1:5010/lastreward/"+ userId;
        //var url ="https://10.14.255.45:5010/lastreward/"+ userId;

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return "Sin canjes";

        return (await response.Content.ReadAsStringAsync()).Trim('"'); // Elimina comillas si la respuesta es un string JSON
    }

    public async Task<string> TokensGraph(int userId)
    {
        const string url = "https://lookerstudio.google.com/embed/reporting/fede5ef6-97e0-4fd2-b1f5-94b604aaf0c2/page/9nmzF";

        var parameters = new {
            userId = userId
        };

        var json = JsonSerializer.Serialize(parameters);
        var encoded = Uri.EscapeDataString(json);//para que { y " no rompan la url
        return url + "?params=" + encoded;//junto
    }

    public async Task<List<SkinData>> GetCatalogoSkins()
    {
        //var url = "https://10.14.255.45:5010/catalogo_skins";
        var url = "https://127.0.0.1:5010/catalogo_skins";
        var lista = await _httpClient.GetFromJsonAsync<List<SkinData>>(url, _jsonOptions);
        return lista ?? new List<SkinData>();
    }

    public async Task<List<SkinData>> GetSkinsUsuario(int idUsuario)
    {
        var url= "https://127.0.0.1:5010/misSkins/" + idUsuario;
        //var url = "https://10.14.255.45:5010/misSkins/" + idUsuario;
        var lista = await _httpClient.GetFromJsonAsync<List<SkinData>>(url, _jsonOptions);
        return lista ?? new List<SkinData>();
    }

    public async Task ComprarSkin(int idUsuario, int idSkin)
    {
        var url = "https://127.0.0.1:5010/comprarSkin";
        //var url = "https://10.14.255.45:5010/comprarSkin";
        var body = new { idUser = idUsuario, idSkin = idSkin };
        await _httpClient.PostAsJsonAsync(url, body);
}
}
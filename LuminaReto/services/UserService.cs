using System.Text;
using System.Text.Json;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    
    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task UpdatePoints(int id, int points)
    {
        var url = "https://127.0.0.1:5010/updatepoints";

        var body = new
        {
            idUser = id,
            points = points
        };

        await _httpClient.PutAsJsonAsync(url, body);
    }

    public async Task<List<Recompensa>> GetRecompensas()
    {
        string url = "https://127.0.0.1:5010/recompensas";
        var listaRecompensas = await _httpClient.GetFromJsonAsync<List<Recompensa>>(url);
        return listaRecompensas ?? new List<Recompensa>();
    }

    public async Task<List<Transaccion>> GetTransacciones(int id, string date)
    {
        var url = "https://127.0.0.1:5010/transacciones/" + id + "/" + date;
        var listaTransacciones = await _httpClient.GetFromJsonAsync<List<Transaccion>>(url);
        return listaTransacciones ?? new List<Transaccion>();
    }

    public async Task<int> GetUserPoints(int id)
    {
        var url = "https://127.0.0.1:5010/getpoints/" + id;
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return 0;
        var responseJson = await response.Content.ReadAsStringAsync();
        return int.Parse(responseJson);
    }
    public async Task<int> GetUserPointsMonth(int id)
    {
        var url = "https://127.0.0.1:5010/getpointsMes/" + id;
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        return 0;
        var responseJson = await response.Content.ReadAsStringAsync();
        return int.Parse(responseJson);
    }

    public async Task CrearTransaccion(int userId,int recompensaId,int monto,string descripcion)
    {
        var url = "https://127.0.0.1:5010/transaccion";

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

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return "Sin canjes";

        return (await response.Content.ReadAsStringAsync()).Trim('"'); // Elimina comillas si la respuesta es un string JSON
    }


}
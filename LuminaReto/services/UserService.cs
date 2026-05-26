using System.Text;
using System.Text.Json;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    
    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

   /* public async Task<User> GetUserById(int id)
    {
        var url = "https://127.0.0.1:5001/user/" + id;

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return new User();

        var responseJson = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<User>(responseJson) ?? new User();
    }*/


    // 💡 CORREGIDO: Ahora coincide exactamente con "Task UpdatePoints" de tu interfaz
    public async Task UpdatePoints(int id, int points)
{
    var url =
        "https://127.0.0.1:5010/updatepoints/"
        + id + "/"
        + points;

    await _httpClient.PostAsync(url, null);
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

    public async Task CrearTransaccion(int userId,
    int recompensaId,
    int monto,
    string descripcion)
{
    var url =
        "https://127.0.0.1:5010/transaccion/"
        + userId + "/"
        + recompensaId + "/"
        + monto;

    var body = new
    {
        description = descripcion
    };

    await _httpClient.PostAsJsonAsync(url, body);
}
}
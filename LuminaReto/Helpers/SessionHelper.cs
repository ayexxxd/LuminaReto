using Microsoft.AspNetCore.Http;

namespace LuminaReto.Helpers;

public static class SessionHelper
{
    private const string IntUserIdKey = "IdUsuario";
    private const string StringUserIdKey = "UserId";

    public static int GetUserId(ISession session)
    {
        var intUserId = session.GetInt32(IntUserIdKey);
        if (intUserId.HasValue)
        {
            return intUserId.Value;
        }

        var stringUserId = session.GetString(StringUserIdKey);
        if (int.TryParse(stringUserId, out var parsedUserId))
        {
            return parsedUserId;
        }

        return 0;
    }

    public static void SetUserId(ISession session, int userId)
    {
        session.SetInt32(IntUserIdKey, userId);
        session.SetString(StringUserIdKey, userId.ToString());
    }
}

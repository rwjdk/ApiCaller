namespace ApiCaller.AuthorizationTypes;

public class ApiClientBearerTokenAuthorization(string token) : ApiClientAuthorization
{
    public string Token { get; } = token;
}
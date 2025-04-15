using ApiCaller.AuthorizationTypes;

namespace ApiCaller;

public class ApiClientOptions
{
    //todo - Add Date Formats
    public string BaseUrl { get; set; }
    public ApiClientAuthorization Authorization { get; set; }
}
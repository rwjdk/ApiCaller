namespace ApiCaller.AuthorizationTypes;

public class ApiClientQueryAuthorization(params QueryParam[] queryParams) : ApiClientAuthorization
{
    public QueryParam[] QueryParams { get; } = queryParams;
}
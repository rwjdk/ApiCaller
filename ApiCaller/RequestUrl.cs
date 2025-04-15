using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace ApiCaller;

public class RequestUrl(string requestUrl, params QueryParam[] QueryParams)
{
    public override string ToString()
    {
        var stringBuilder = new StringBuilder(requestUrl);
        string separator = requestUrl.Contains("?") ? "&" : "?";
        foreach (QueryParam param in QueryParams)
        {
            stringBuilder.Append($"{separator}{param.Key}={HttpUtility.UrlEncode(param.Value)}");
            separator = "&";
        }

        return stringBuilder.ToString();
    }
}
using System.Globalization;

namespace ApiCaller;

public class QueryParam(string key, string value)
{
    public QueryParam(string key, int value) : this(key, value.ToString(CultureInfo.InvariantCulture))
    {
        //Empty
    }

    public string Key { get; } = key;
    public string Value { get; } = value;
}
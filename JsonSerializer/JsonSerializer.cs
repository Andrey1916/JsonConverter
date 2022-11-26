using System.Collections;
using System.Reflection;
using System.Text;

namespace JsonSerializer;

public static class JsonConverter
{
    public static string Serialize(object @object)
    {
        if (@object is null)
        {
            return "null";
        }

        if (@object is IEnumerable array)
        {
            return SerializeArray(array);
        }
        else
        {
            return SerializeObject(@object);
        }
    }

    private static string SerializeArray(IEnumerable array)
    {
        var builder = new StringBuilder("[");
        var list = new List<string>();

        foreach (var item in array)
        {
            string obj = item is IEnumerable arrayItem
                ? SerializeArray(arrayItem)
                : SerializeObject(item);

            list.Add(obj);
        }

        builder.AppendJoin(',', list);
        builder.Append(']');

        return builder.ToString();
    }

    private static readonly Type[] _numberTypes = new Type[]
    {
        typeof(int),     typeof(uint),
        typeof(short),   typeof(ushort),
        typeof(decimal), typeof(long),
        typeof(ulong)
    };

    private static string SerializeObject(object @object)
    {
        var builder = new StringBuilder("{");

        var type = @object.GetType();
        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        for (uint i = 0; i < properties.Length; ++i)
        {
            string propName = string.Concat(
                properties[i].Name[0].ToString().ToLower(),
                properties[i].Name.AsSpan(1));

            builder.Append('\"');
            builder.Append(propName);
            builder.Append("\":");

            var propValue = properties[i].GetValue(@object, null);

            if (propValue is null)
            {
                builder.Append("null");
            }
            else if (properties[i].PropertyType == typeof(bool))
            {
                string obj = propValue.ToString()
                                      !.ToLower();

                builder.Append(obj);
            }
            else if (properties[i].PropertyType == typeof(IEnumerable))
            {
                string obj = SerializeArray((IEnumerable)propValue);

                builder.Append(obj);
            }
            else if (_numberTypes.Contains(properties[i].PropertyType))
            {
                builder.Append(
                    propValue.ToString()
                    );
            }
            else if (properties[i].PropertyType == typeof(string))
            {
                builder.Append('\"');
                builder.Append(
                    propValue.ToString()
                    );
                builder.Append('\"');
            }
            else
            {
                string obj = SerializeObject(propValue);

                builder.Append(obj);
            }

            if (i != properties.Length - 1)
            {
                builder.Append(',');
            }
        }

        builder.Append('}');

        return builder.ToString();
    }
}

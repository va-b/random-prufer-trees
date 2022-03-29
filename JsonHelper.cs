using Newtonsoft.Json.Linq;

namespace TestJobj;

public static class JsonHelper
{
    public static JObject MergeMany(IEnumerable<JObject> objects)
    {
        var result = new JObject();
        foreach (var obj in objects)
        {
            result.StraightMerge(obj);
        }

        return result;
    }

    public static bool Contains(this JObject first, JObject second)
    {
        foreach (var (key, value) in second)
        {
            var prop = first.Property(key.ToLowerInvariant());
            if (prop == null || !JObject.DeepEquals(prop.Value, value))
            {
                return false;
            }
        }

        return true;
    }

    public static void StraightMerge(this JObject first, JObject second, bool secondPriority = false)
    {
        foreach (KeyValuePair<string, JToken?> contentItem in second)
        {
            JProperty? existingProperty = first.Property(contentItem.Key, StringComparison.InvariantCulture);

            if (existingProperty == null)
            {
                first.Add(contentItem.Key, contentItem.Value);
            }
            else if (contentItem.Value != null)
            {
                if (existingProperty.Value is not JContainer existingContainer ||
                    existingContainer.Type != contentItem.Value.Type)
                {
                    if (!IsNull(contentItem.Value))
                    {
                        if (!IsNull(existingProperty.Value) && existingProperty.Value.Type == JTokenType.Boolean &&
                            !secondPriority)
                        {
                            existingProperty.Value = (bool)existingProperty.Value || (bool)contentItem.Value;
                        }
                        else
                        {
                            existingProperty.Value = contentItem.Value;
                        }
                    }
                }
                else
                {
                    // arrays not supported
                    StraightMerge((JObject)existingContainer, (JObject)contentItem.Value);
                }
            }
        }
    }

    private static bool IsNull(JToken token) => token.Type == JTokenType.Null || token is JValue { Value: null };
}
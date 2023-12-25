using bielu.SchemaGenerator.Build.Configuration;
using bielu.SchemaGenerator.Core.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema.Generation;

namespace bielu.SchemaGenerator.Build.Services;

public class SchemaGenerator : ISchemaGenerator
{
    private readonly JsonSchemaGenerator _schemaGenerator;

    public SchemaGenerator()
    {
        _schemaGenerator = new JsonSchemaGenerator(
            new SchemaGeneratorSettings());
    }

    public string Generate(Type type)
    {
        var uSyncSchema = GenerateSchema(type);
        return uSyncSchema.ToString();
    }

    private JObject GenerateSchema(Type type)
    {
        var prefixProperty =
            type.GetFields().FirstOrDefault(x => Attribute.IsDefined(x, typeof(SchemaPrefixAttribute)));

        string schema = _schemaGenerator.Generate(type).ToJson();
        var prefix = prefixProperty?.GetValue(null);
        if (prefix != null)
        {
            foreach (var prefixValue in prefix.ToString().Split(":").Reverse())
            {
                schema = new JObject
                {
                    { "$schema", "http://json-schema.org/draft-04/schema#" },
                    { "title", prefixValue },
                    { "type", "object" },
                    {
                        "properties", new JObject
                        {
                            { prefixValue, JToken.Parse(schema) }
                        }
                    }
                }.ToString();
            }
        }

        return JsonConvert.DeserializeObject<JObject>(schema);
    }
}
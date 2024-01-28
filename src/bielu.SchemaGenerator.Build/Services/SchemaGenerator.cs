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
var schemaObject = _schemaGenerator.Generate(type);
        var prefix = prefixProperty?.GetValue(null).ToString().Replace(":", "");
        schemaObject.Title = prefix != null ? $"{prefix}" : schemaObject.Title;
        string schema = schemaObject.ToJson();

        return JsonConvert.DeserializeObject<JObject>(schema);
    }
}
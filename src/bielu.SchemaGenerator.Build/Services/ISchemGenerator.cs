namespace bielu.SchemaGenerator.Build.Services;

public interface ISchemaGenerator
{
    public string Generate(Type type);
}
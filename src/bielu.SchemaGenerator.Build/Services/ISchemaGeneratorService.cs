using System.Reflection;

namespace bielu.SchemaGenerator.Build.Services;

public interface ISchemaGeneratorService
{
    public void GenerateSchema(IEnumerable<Assembly> assemblies);
}
using System.Reflection;
using bielu.SchemaGenerator.Build.Configuration;
using bielu.SchemaGenerator.Build.Extensions;

namespace bielu.SchemaGenerator.Build.Services;

public class SchemaGeneratorService : ISchemaGeneratorService
{
    private readonly ISchemaGenerator _generator;
    private readonly Options _options;

    public SchemaGeneratorService(ISchemaGenerator generator, Options options)
    {
        _generator = generator;
        _options = options;
    }

    public void GenerateSchema(IEnumerable<Assembly> assemblies)
    {
        string targetTemplate = String.Empty;

        using (var stream =
               typeof(SchemaGeneratorService).Assembly.GetManifestResourceStream(
                   "bielu.SchemaGenerator.Build.Template.TargetTemplate.xml"))
            if (stream != null)
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    targetTemplate = reader.ReadToEnd();
                }
            }

        foreach (var assembly in assemblies)
        {
            string targetFile = String.Empty;
            List<string> schemaFiles = new List<string>();
            foreach (var classType in assembly.GetTypesWithHelpAttribute())
            {
                var schema = _generator.Generate(classType)
                    .Replace("\"USync\"", "\"uSync\"");
                var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory,
                    _options.OutputFile.Replace("assemblyName", assembly.GetName().Name)
                        .Replace("className", classType.Name)));
                Console.WriteLine("Path to use {0}", path);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                targetFile = path.Replace("Schema", "buildTransitive")
                    .Replace($"appsettings-schema.{classType.Name}.json", $"{assembly.GetName().Name}.Targets.props");
                var directoryForBuildTarget = Path.GetDirectoryName(targetFile);
                Directory.CreateDirectory(directoryForBuildTarget);
                File.WriteAllText(path, schema);
                FileInfo oFileInfo = new FileInfo(path);
                schemaFiles.Add(oFileInfo.Name);
                Console.WriteLine("Ensured directory exists");


                Console.WriteLine("File written at {0}", path);
            }

            var generatedTemplate = targetTemplate.Replace("schemaFiles",
                string.Join("\n",
                    schemaFiles.Select(x =>
                        $"<UmbracoJsonSchemaFiles Include=\"$(MSBuildThisFileDirectory)..\\Schema\\{x}\"  />")));
            File.WriteAllText(targetFile, generatedTemplate);
        }
    }
}

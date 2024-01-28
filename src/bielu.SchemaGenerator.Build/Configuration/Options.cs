using CommandLine;

namespace bielu.SchemaGenerator.Build.Configuration;

public class Options
{
    [Option('o', "outputFile", Required = false,
        HelpText = "",
        Default = "..\\..\\..\\..\\assemblyName\\Schema\\appsettings-schema.className.json")]
    public string? OutputFile { get; set; }
}

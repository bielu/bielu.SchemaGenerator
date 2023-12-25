// See https://aka.ms/new-console-template for more information

using System.Reflection;
using bielu.SchemaGenerator.Build.Configuration;
using bielu.SchemaGenerator.Build.Services;
using CommandLine;

namespace bielu.SchemaGenerator.Project;

internal class Program
{
    static readonly IList<Assembly> Assemblies = new List<Assembly>()
    {
        typeof(Program).Assembly,
    };

    public static async Task Main(string[] args)
    {
        try
        {
            await Parser.Default.ParseArguments<Options>(args)
                .WithParsedAsync(x=>Execute(x));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static async Task Execute(Options options)
    {
        Task.FromResult(() =>
        {
            var schemaGenerator = new SchemaGeneratorService(new Build.Services.SchemaGenerator(), options);
            schemaGenerator.GenerateSchema(Assemblies);
        });
    }
}
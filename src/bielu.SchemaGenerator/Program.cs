using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace bielu.Umbraco.Cdn;

internal class Program
{
    static void Main(string[] args)
    {
        var versionString = Assembly.GetEntryAssembly()?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion
            .ToString();

        Console.WriteLine($"SchemaGenerator v{versionString}");
        Console.WriteLine("-------------");
        Console.WriteLine("\nUsage:");
        Console.WriteLine(" SchemaGenerator path/to/project.csproj or path/to/solution.sln");
        if (args.Length == 0)
        {
            return;
        }
List<string> paths = new();
        if (args[0].EndsWith("csproj"))
        {
            Console.WriteLine("Generating schema for project...");
            paths.Add(args[0]);
        }
        if (args[0].EndsWith("sln"))
        {
            Console.WriteLine("Generating schema for solution...");
            var projects = Directory
                .GetFiles( GetPath(args[0]), "*.csproj", SearchOption.AllDirectories)
                .ToList();
            paths.AddRange(projects.Select(x=>x));
            
        }

        foreach (var path in paths)
        {
            Console.WriteLine($"Working on project... {path}");
            var binDirectory = GetPath(path) + "/bin";
            var configurationDirectory = binDirectory + args[1];
            var assemblyPath = configurationDirectory + args[2] + Path.GetFileNameWithoutExtension(path) + ".dll";
            var DLL = Assembly.LoadFile(assemblyPath);
            var types = DLL.GetTypes();

        }
    }
    private static string GetPath(string path)
    {
        var index = path.IndexOf("/") >0 ? path.LastIndexOf("/") : path.LastIndexOf("\\");

        return path.Substring(0,index );
    }
}
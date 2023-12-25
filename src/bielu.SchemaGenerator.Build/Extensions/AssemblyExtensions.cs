using System.Reflection;
using bielu.SchemaGenerator.Core.Attributes;

namespace bielu.SchemaGenerator.Build.Extensions;

public static class AssemblyExtensions
{
    public static IEnumerable<Type> GetTypesWithHelpAttribute(this Assembly assembly) {
        foreach(Type type in assembly.GetTypes()) {
            if (type.GetCustomAttributes(typeof(SchemaGenerationAttribute), true).Length > 0) {
                yield return type;
            }
        }
    }
}
using System;

namespace bielu.SchemaGenerator.Core.Attributes;
[AttributeUsage(AttributeTargets.Class)]

public class SchemaGenerationAttribute : Attribute
{
}
[AttributeUsage(AttributeTargets.Property)]
public class SchemaPrefixAttribute : Attribute
{
    public SchemaPrefixAttribute()
    {
    }
}

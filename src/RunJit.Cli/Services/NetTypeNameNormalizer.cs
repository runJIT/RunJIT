using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Services
{
    public static class AddNetTypeNameNormalizerExtension
    {
        public static void AddNetTypeNameNormalizer(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<NetTypeNameNormalizer>();
        }
    }

    // I need a conversion of all system types names to the C# names
    public class NetTypeNameNormalizer
    {
        public string Normalize(Type type)
        {
            var typeName = type.Name;

            switch (typeName)
            {
                case nameof(String):
                    return "string";
                case nameof(Int16):
                    return "short";
                case nameof(Int32):
                    return "int";
                case nameof(Int64):
                    return "long";
                case nameof(Byte):
                    return "byte";
                case nameof(Single):
                    return "float";
                case nameof(Double):
                    return "double";
                case nameof(Boolean):
                    return "bool";
                case nameof(Decimal):
                    return "decimal";
                case nameof(DateTime):
                    return "DateTime";
                case nameof(DateTimeOffset):
                    return "DateTimeOffset";
                case nameof(TimeSpan):
                    return "TimeSpan";
                case nameof(Guid):
                    return "Guid";
                default:
                    return typeName;
            }
        }
    }
}

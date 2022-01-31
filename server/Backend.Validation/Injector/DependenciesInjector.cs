using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Backend.Validation.Injector
{
    public static class DependenciesInjector
    {
        public static void AddInjectableFluentValidationDependencies(this IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            IEnumerable<TypeInfo> classTypes = assembly.ExportedTypes.Select(t => IntrospectionExtensions.GetTypeInfo(t)).Where(t => t.IsClass && !t.IsAbstract);

            foreach (TypeInfo type in classTypes)
            {
                IEnumerable<TypeInfo> interfaces = type.ImplementedInterfaces.Select(i => i.GetTypeInfo());

                foreach (TypeInfo handlerType in interfaces.Where(i => i.IsGenericType))
                {
                    services.AddTransient(handlerType.AsType(), type.AsType());
                }
            }
        }
    }
}

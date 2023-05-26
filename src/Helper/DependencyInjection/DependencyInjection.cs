﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Helper.DependencyInjection
{
    // Gets all classes that implement an interface whose type is within the serviceType array.
    // Then loop through the classes and find a matching interface name (e.g.match ToDoRepository with IToDoRepository) or use the generic interface for dependency injection.
    public static class DependencyInjection
    {
        public static void RegisterAllDependencies(this IServiceCollection services, Type[] serviceType, Assembly assembly)
        {
            var implementationTypes = assembly
                .GetTypes()
                .Where(p =>
                {
                    if (p.IsAbstract)
                    {
                        return false;
                    }

                    var interfaces = p.GetInterfaces();
                    return interfaces.Any(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() != null &&
                        serviceType.Contains(i.GetGenericTypeDefinition()));
                });
            foreach (var t in implementationTypes)
            {
                var interfaces = t.GetInterfaces();
                Type? currentInterface = null;
                var matchingClassNameInterface = interfaces.FirstOrDefault(i => i.Name == $"I{t.Name}");
                if (matchingClassNameInterface != null)
                {
                    currentInterface = matchingClassNameInterface;
                }
                else
                {
                    currentInterface = interfaces.FirstOrDefault(i =>
                       i.IsGenericType &&
                       i.GetGenericTypeDefinition() != null &&
                       serviceType.FirstOrDefault(i.GetGenericTypeDefinition()) == i.GetGenericTypeDefinition());
                }

                if (currentInterface != null)
                {
                    services.AddScoped(currentInterface, t);
                }
            }
        }
    }
}

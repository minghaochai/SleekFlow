using AutoMapper;
using System.Reflection;

namespace SleekFlow.Application.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            AllowNullCollections = true;
            ApplyMappers(Assembly.GetExecutingAssembly());
        }

        // Gets all the classes which implement the IMapFrom interface
        // Then loop through the classes and invoke the 'Mapping' method to create a map between the class and the generic type passed to IMapFrom
        private void ApplyMappers(Assembly assembly)
        {
            var argumentTypes = new Type[] { typeof(Profile) };

            var mappingMethodName = nameof(IMapFrom<object>.Mapping);

            var types = assembly
                .GetExportedTypes()
                .Where(t => !t.IsAbstract && t.GetInterfaces().Any(HasInterface));

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

                if (interfaces.Count > 0)
                {
                    foreach (var @interface in interfaces)
                    {
                        var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

                        interfaceMethodInfo?.Invoke(instance, new object[] { this });
                    }
                }
            }
        }

        private bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IMapFrom<>);
    }
}

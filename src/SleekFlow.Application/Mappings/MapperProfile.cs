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

                var methodInfo = type.GetMethod(mappingMethodName);

                if (methodInfo != null)
                {
                    methodInfo.Invoke(instance, new object[] { this });
                }
                else
                {
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
        }

        private bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IMapFrom<>);
    }
}

using System.Collections.Generic;
using System.Configuration;
using GPS.SimpleDI;
using GPS.SimpleDI.Configuration;

namespace GPS.SimpleID.Configuration.Demo
{
    partial class Program
    {
        internal class TestLoader : IDefinitionLoader<TestInjector>
        {
            public TestLoader() { }

            public TestInjector LoadDefintion()
            {
                //var config = SimpleDiConfigurationSection.GetCustomConfig(".\\GPS.SimpleDI.Configuration.dll", ".\\GPS.SimpleDI.Configuration.Tests.dll.config", "simpleDiConfigurationSection");
                var config =
                    ConfigurationManager.GetSection("simpleDiConfigurationSection")
                        as SimpleDiConfigurationSection;

                if (config != null)
                {
                    var objectDefinition = config.Objects["string"];

                    var injector = new TestInjector()
                    {
                        TypeName = objectDefinition.TypeName,
                        TypeNamespace = objectDefinition.TypeNamespace,
                    };


                    var constructors = new List<List<Parameter>>();
                    foreach (var c in objectDefinition.Constructors)
                    {
                        foreach (var p in c.ConstructorParameters)
                        {
                            constructors.Add(new List<Parameter>()
                            {
                                new Parameter()
                                {
                                    Name = p.Name,
                                    TypeName = p.TypeName,
                                    TypeNamespace = p.TypeNamespace,
                                    Value = p.Value,
                                }
                            });
                        }
                    }

                    injector.Constructors = constructors;


                    return injector;
                }

                return null;
            }
        }

    }
}

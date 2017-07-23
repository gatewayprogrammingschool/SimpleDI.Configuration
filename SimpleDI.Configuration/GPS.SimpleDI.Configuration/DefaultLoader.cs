using System.Collections.Generic;
using System.Configuration;

namespace GPS.SimpleDI.Configuration
{
    public class DefaultLoader : IDefinitionLoader<DefaultInjector>
    {
        protected string ObjectKey = "object";

        public DefaultLoader() { }

        public DefaultLoader(string objectKey)
        {
            ObjectKey = objectKey;
        }

        public virtual DefaultInjector LoadDefintion()
        {
            var config =
                ConfigurationManager.GetSection("simpleDiConfigurationSection")
                    as SimpleDiConfigurationSection;

            if (config != null)
            {
                var objectDefinition = config.Objects[ObjectKey];

                var injector = new DefaultInjector()
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

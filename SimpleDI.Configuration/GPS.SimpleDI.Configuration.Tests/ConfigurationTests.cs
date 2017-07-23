using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GPS.SimpleDI.Configuration.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void TestConfiguration()
        {
            var injector = SimpleDiFactory.Load<TestInjector>(typeof(TestLoader));

            Assert.IsNotNull(injector);

            injector.Constructors[0][0].Value = "test".ToCharArray();

            var output = injector.MakeObject(injector.Constructors[0]);

            Assert.IsInstanceOfType(output, typeof(System.String));
            Assert.AreEqual(output as string, "test");
        }

        internal class TestLoader : IDefinitionLoader<TestInjector>
        {
            public TestLoader() { }

            public TestInjector LoadDefintion()
            {
                var customConfig = SimpleDiConfigurationSection.GetCustomConfig(".\\GPS.SimpleDI.Configuration.dll", ".\\GPS.SimpleDI.Configuration.Tests.dll.config", "simpleDiConfigurationSection");
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

                    
                        var consturctors = new List<List<Parameter>>();
                        consturctors.AddRange(
                            objectDefinition.Constructors
                                .Select(c => new List<Parameter>
                                {
                                    new Parameter()
                                    {
                                        Name = c.Name,
                                        TypeNamespace = c.TypeNamespace,
                                        TypeName = c.TypeName,
                                        Value = c.Value
                                    }
                                }));

                        injector.Constructors = consturctors;
                    

                    return injector;
                }

                return null;
            }
        }

        internal class TestInjector : IInjectable
        {
            public object MakeObject(List<Parameter> parameters)
            {
                var assm = System.Reflection.Assembly.Load(TypeNamespace);

                var itype = System.Type.GetType(TypeName,
                    null,
                    (assembly, name, b) =>
                        assm.GetType(name, true, b),
                    true, false);


                var passm = System.Reflection.Assembly.Load(parameters[0].TypeNamespace);

                var ptype = System.Type.GetType(
                    parameters[0].TypeName,
                    null,
                    (assembly, name, b) =>
                        passm.GetType(name, true, b),
                    true,
                    false);

                var parms = new List<object>();
                foreach (var p in parameters)
                {
                    if (p != null)
                    {
                        var param1 = Activator.CreateInstance(
                            ptype,
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
                            null,
                            new[] { p },
                            System.Globalization.CultureInfo.CurrentCulture
                        );

                        parms.Add(param1);
                    }
                }

                var obj = Activator.CreateInstance(
                    itype,
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                    null,
                    parms.ToArray(),
                    System.Globalization.CultureInfo.CurrentCulture);

                return obj;

            }

            public object MakeObject()
            {
                var assm = System.Reflection.Assembly.Load(TypeNamespace);

                var itype = System.Type.GetType(TypeName,
                    null,
                    (assembly, name, b) =>
                        assm.GetType(name, true, b),
                    true, false);


                var obj = Activator.CreateInstance(
                    itype,
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                    null,
                    new object[0] ,
                    System.Globalization.CultureInfo.CurrentCulture);

                return obj;

            }

            public string TypeNamespace { get; set; }

            public string TypeName { get; set; }

            public List<List<Parameter>> Constructors { get; set; }

            public List<Method> Methods { get; set; }
        }
    }
}

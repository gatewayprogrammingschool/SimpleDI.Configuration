using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPS.SimpleDI;
using GPS.SimpleDI.Configuration;
using GPS.SimpleExtensions;

namespace GPS.SimpleID.Configuration.Demo
{
    partial class Program
    {
        static void Main(string[] args)
        {
            new Program().TestConfiguration();
        }

        public void TestConfiguration()
        {
            try
            {
                var injector = SimpleDiFactory.Load<DefaultInjector>(typeof(Configuration.Program.GenericTestLoader));

                injector.AssertParameterNotNull(nameof(injector), "inject was not created.");

                if (injector.Constructors.Count > 0 && injector.Constructors[0].Count > 0)
                {
                    injector.Constructors[0][0].Value = "test".ToCharArray();

                    var output = injector.MakeObject(injector.Constructors[0]);

                    output.GetType()
                        .AssertEquals<ApplicationException>(typeof(System.String), "output is not a System.String");

                    ((System.String) output).AssertEquals<ApplicationException>("test", $"output has wrong value ({output}).");
                }
                else
                {
                    throw new ApplicationException("Could not load configuration into injector.");
                }

                Console.WriteLine("Successfully instantiated the object defined in the config file!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace GPS.SimpleDI.Configuration
{
    public class DefaultInjector : IInjectable
    {
        public virtual object MakeObject(List<Parameter> parameters)
        {
            var assm = System.Reflection.Assembly.Load(TypeNamespace);

            var itype = System.Type.GetType(TypeName,
                null,
                (assembly, name, b) =>
                    assm.GetType(name, true, b),
                true, false);

            var parms = new List<object>();

            foreach (var parm in parameters)
            {
                parms.Add(parm.Value);
            }

            var obj = Activator.CreateInstance(
                itype,
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                null,
                parms.ToArray(),
                System.Globalization.CultureInfo.CurrentCulture);

            return obj;

        }

        public virtual object MakeObject()
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
                new object[0],
                System.Globalization.CultureInfo.CurrentCulture);

            return obj;

        }

        public string TypeNamespace { get; set; }

        public string TypeName { get; set; }

        public List<List<Parameter>> Constructors { get; set; }

        public List<Method> Methods { get; set; }
    }
}

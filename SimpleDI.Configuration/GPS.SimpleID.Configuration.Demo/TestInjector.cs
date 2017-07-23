using System;
using System.Collections.Generic;
using GPS.SimpleDI;

namespace GPS.SimpleID.Configuration.Demo
{
    partial class Program
    {
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

                var parms = new List<object>();

                foreach (var parm in parameters)
                {
                    parms.Add(parm.Value);
                    //var passm = System.Reflection.Assembly.Load(parm.TypeNamespace);

                    //var ptype = System.Type.GetType(
                    //    parameters[0].TypeName,
                    //    null,
                    //    (assembly, name, b) =>
                    //        passm.GetType(name, true, b),
                    //    true,
                    //    false);

                    //if (!ptype.IsArray)
                    //{
                    //    parms.Add(Activator.CreateInstance(ptype,
                    //        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                    //        null, new object[0],
                    //        System.Globalization.CultureInfo.CurrentCulture));
                    //}
                    //else
                    //{
                    //    if (parm.Value.GetType().IsArray)
                    //    {
                    //        var fi = parm.GetType().GetField("Value");

                    //        var arr = fi.GetValue(parm) as Array;
                    //        var p = Activator.CreateInstance(ptype, new object[] {arr.Length});
                    //        p = parm.Value;
                    //        parms.Add();
                    //    }
                    //}
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
}

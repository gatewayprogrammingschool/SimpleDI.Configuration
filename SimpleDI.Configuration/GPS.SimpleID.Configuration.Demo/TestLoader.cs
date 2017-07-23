using System.Collections.Generic;
using System.Configuration;
using GPS.SimpleDI;
using GPS.SimpleDI.Configuration;

namespace GPS.SimpleID.Configuration
{
    partial class Program
    {
        internal class GenericTestLoader : DefaultLoader
        {
            public GenericTestLoader()
            {
                ObjectKey = "string";
            }

            public GenericTestLoader(string objectKey)
            {
                ObjectKey = objectKey;
            }
        }

    }
}

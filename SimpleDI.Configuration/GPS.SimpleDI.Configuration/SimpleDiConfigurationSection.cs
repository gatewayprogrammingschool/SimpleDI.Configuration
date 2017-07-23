using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GPS.SimpleDI.Configuration
{
    public class SimpleDiConfigurationSection : ConfigurationSection
    {
        public SimpleDiConfigurationSection()
        {
            base["objects"] = new ObjectsCollection();
        }

        private static Assembly _configurationDefiningAssembly;

        public static SimpleDiConfigurationSection GetCustomConfig(string configDefiningAssemblyPath,
            string configFilePath, string sectionName)
        {
            AppDomain.CurrentDomain.AssemblyResolve += ConfigResolveEventHandler;
            _configurationDefiningAssembly = Assembly.LoadFrom(configDefiningAssemblyPath);
            var exeFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            var customConfig = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap,
                ConfigurationUserLevel.None);
            var returnConfig = customConfig.GetSection(sectionName) as SimpleDiConfigurationSection;
            AppDomain.CurrentDomain.AssemblyResolve -= ConfigResolveEventHandler;
            return returnConfig;
        }

        protected static Assembly ConfigResolveEventHandler(object sender, ResolveEventArgs args)
        {
            return _configurationDefiningAssembly;
        }

        [ConfigurationProperty("objects", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ObjectsCollection),
            AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ObjectsCollection Objects
        {
            get => base["objects"] as ObjectsCollection;
            set 
                {
                if(base["objects"] != null) base["objects"] = value;
            } 
        }

    }

    public class ObjectsCollection : ConfigurationElementCollection, IEnumerable<ParameterElement>,
        IEnumerator<ParameterElement>
    {
        private ConfigurationElementCollectionType _collectionType;
        private ParameterElement _current;

        protected override ConfigurationElement CreateNewElement()
        {
            return new ObjectElement();
        }

        protected override object GetElementKey(ConfigurationElement ObjectElement)
        {
            return ((ObjectElement)ObjectElement).Key;
        }

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType
            .AddRemoveClearMap;

        public ObjectElement this[int index]
        {
            get => (ObjectElement)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new ObjectElement this[string name] =>
            (ObjectElement)BaseGet(name);

        public int IndexOf(ObjectElement Object)
        {
            return BaseIndexOf(Object);
        }

        public void Add(ObjectElement Object)
        {
            BaseAdd(Object);
        }

        public void Remove(ObjectElement Object)
        {
            if (BaseIndexOf(Object) >= 0)
            {
                BaseRemove(Object.Key);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }

        public IEnumerator<ParameterElement> GetEnumerator()
        {
            return this;
        }

        private int currentIndex = -1;

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            currentIndex++;

            var result = currentIndex < BaseGetAllKeys().Length;

            if(result)
            _current = BaseGet(currentIndex) as ParameterElement;

            return result;
        }

        public void Reset()
        {
            currentIndex = -1;
        }

        public ParameterElement Current
        {
            get { return _current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

    }

    public class ObjectElement : ConfigurationElement
    {
        public ObjectElement()
        {
            base["constructors"] = new ConstructorsCollection();
        }

        [ConfigurationProperty("key", DefaultValue = "System.String",
            IsRequired = true)]
        public string Key
        {
            get => this["key"] as string;
            set => this["key"] = value;
        }

        [ConfigurationProperty("typeName", DefaultValue = "System.Object",
            IsRequired = true)]
        public string TypeName
        {
            get => this["typeName"] as string;
            set => this["typeName"] = value;
        }

        [ConfigurationProperty("typeNamespace", DefaultValue = "System",
            IsRequired = true)]
        public string TypeNamespace
        {
            get => this["typeNamespace"] as string;
            set => this["typeNamespace"] = value;
        }

        [ConfigurationProperty("constructors", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ConstructorsCollection),
            AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ConstructorsCollection Constructors
        {
            get => base["constructors"] as ConstructorsCollection;
        }
    }

    public class 
        ConstructorsCollection : ConfigurationElementCollection, IEnumerable<ConstructorElement>,
        IEnumerator<ConstructorElement>
    {
        private ConfigurationElementCollectionType _collectionType;
        private ConstructorElement _current;

        protected override ConfigurationElement CreateNewElement()
        {
            return new ConstructorElement();
        }

        protected override object GetElementKey(ConfigurationElement constructorElement)
        {
            return ((ConstructorElement)constructorElement).Key;
        }

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType
            .AddRemoveClearMap;

        public ConstructorElement this[int index]
        {
            get => (ConstructorElement)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new ConstructorElement this[string name] =>
            (ConstructorElement)BaseGet(name);

        public int IndexOf(ConstructorElement constructor)
        {
            return BaseIndexOf(constructor);
        }

        public void Add(ConstructorElement constructor)
        {
            BaseAdd(constructor);
        }

        public void Remove(ConstructorElement constructor)
        {
            if (BaseIndexOf(constructor) >= 0)
            {
                BaseRemove(constructor.Key);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }

        public IEnumerator<ConstructorElement> GetEnumerator()
        {
            return this;
        }

        private int currentIndex = -1;

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            currentIndex++;

            var result = currentIndex < BaseGetAllKeys().Length;

            if(result)
            _current = BaseGet(currentIndex) as ConstructorElement;

            return result;
        }

        public void Reset()
        {
            currentIndex = -1;
        }

        public ConstructorElement Current
        {
            get { return _current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

    }

    public class ConstructorElement : ConfigurationElement
    {
        public ConstructorElement()
        {
            base["constructorParameters"] = new ConstructorParametersCollection();
        }
        [ConfigurationProperty("constructorParameters", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ConstructorParametersCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ConstructorParametersCollection ConstructorParameters => 
            base["constructorParameters"] as ConstructorParametersCollection;

        [ConfigurationProperty("key", DefaultValue = "name", IsKey = true, IsRequired = true)]
        public string Key
        {
            get => base["key"] as string;
            set => base["key"] = value;
        }
    }

    public class ConstructorParametersCollection : ConfigurationElementCollection, IEnumerable<ParameterElement>,
        IEnumerator<ParameterElement>
    {
        private ConfigurationElementCollectionType _collectionType;

        protected override ConfigurationElement CreateNewElement()
        {
            return new ParameterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ParameterElement)element).Name;
        }

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType
            .AddRemoveClearMap;

        public ParameterElement this[int index]
        {
            get => (ParameterElement)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new ParameterElement this[string name] =>
            (ParameterElement)BaseGet(name);

        public int IndexOf(ParameterElement parameter)
        {
            return BaseIndexOf(parameter);
        }

        public void Add(ParameterElement parameter)
        {
            BaseAdd(parameter);
        }

        public void Remove(ParameterElement parameter)
        {
            if (BaseIndexOf(parameter) >= 0)
            {
                BaseRemove(parameter.Name);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }

        private int currentIndex = -1;
        private ParameterElement _current;

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            currentIndex++;

            var result = currentIndex < BaseGetAllKeys().Length;

            if (result)
                _current = BaseGet(currentIndex) as ParameterElement;

            return result;
        }

        public void Reset()
        {
            currentIndex = -1;
        }

        public ParameterElement Current
        {
            get { return _current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public IEnumerator<ParameterElement> GetEnumerator()
        {
            return this;
        }
    }

    public class ParameterElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "parameter",
            IsRequired = true)]
        public string Name
        {
            get => this["name"] as string;
            set => this["name"] = value;
        }

        [ConfigurationProperty("value", DefaultValue = null,
            IsRequired = false)]
        public string Value
        {
            get => this["value"] as string;
            set => this["value"] = value;
        }

        [ConfigurationProperty("typeName", DefaultValue = "System.Object",
            IsRequired = true)]
        public string TypeName
        {
            get => this["typeName"] as string;
            set => this["typeName"] = value;
        }

        [ConfigurationProperty("typeNamespace", DefaultValue = "System",
            IsRequired = true)]
        public string TypeNamespace
        {
            get => this["typeNamespace"] as string;
            set => this["typeNamespace"] = value;
        }

    }
}

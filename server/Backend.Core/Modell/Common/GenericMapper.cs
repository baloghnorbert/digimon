using System.Collections.Generic;

namespace Backend.Core.Modell.Common
{
    public class ScalarMapper<T>
    {
        public T Value { get; set; }

        public ScalarMapper()
        { }

        public ScalarMapper(T value)
        {
            Value = value;
        }
    }

    public class StructMapper<T> where T : struct
    {
        public T Value { get; set; }

        public StructMapper()
        { }

        public StructMapper(T value)
        {
            Value = value;
        }
    }

    public class ObjectMapper<T> where T : class, new()
    {
        public T ObjectValue { get; set; }

        public ObjectMapper()
        { }

        public ObjectMapper(T value)
        {
            ObjectValue = value ?? new T();
        }
    }

    public class CollectionMapper<T>
    {
        public ICollection<T> ObjectValue { get; set; }

        public CollectionMapper()
        { }

        public CollectionMapper(ICollection<T> value)
        {
            ObjectValue = value;
        }
    }

    public class CollectionObjectMapper<T> where T : class, new()
    {
        public ICollection<T> ObjectValue { get; set; }

        public CollectionObjectMapper()
        { }

        public CollectionObjectMapper(ICollection<T> value)
        {
            ObjectValue = value ?? new List<T>();
        }
    }
}

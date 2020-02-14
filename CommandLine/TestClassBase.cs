using System;

namespace CommandLine
{
    public abstract class TestClassBase<T> : IDisposable
    {
        public string GetTypeName()
        {
            return typeof(T).Name;
        }

        public abstract void Dispose();
    }
}

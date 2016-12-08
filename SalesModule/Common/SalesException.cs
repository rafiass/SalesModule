using System;

namespace SalesModule
{
    internal class SalesException : Exception
    {
        public SalesException(string msg) : base(msg) { }
    }
}

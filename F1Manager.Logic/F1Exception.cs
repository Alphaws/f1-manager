using System;

namespace F1Manager.Logic
{
    public class F1Exception : Exception
    {
        public F1Exception() { }
        public F1Exception(string message) : base(message) { }
    }
}

using System;

namespace NinjaGame.Exceptions
{
    public class NullParameterException: Exception
    {
        public NullParameterException() : base("A parameter passed into this method is null, but was expected to have a value!")
        { }
    }
}

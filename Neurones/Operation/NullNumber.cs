using System;

namespace Neurones
{
    public class NullNumber : Number
    {
        public double value()
        {
            throw new Exception("THIS NEURONE HAS NO VALUE");
        }
    }

}

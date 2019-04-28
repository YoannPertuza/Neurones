namespace Neurones
{
    public class PreLU : Number
    {
        public PreLU(double toSigmoid) : this(new DefaultNumber(toSigmoid))
		{		
		}

        public PreLU(Number toSigmoid)
		{
			this.toSigmoid = toSigmoid;
		}

		private Number toSigmoid;
		public double value()
		{
			return this.toSigmoid.value();
		}
    }

}

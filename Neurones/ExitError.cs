namespace Neurones
{
	public class ExitError : Error
    {
        public ExitError(int index, Number output, Number expected)
        {
            this.index = index;
            this.output = output;
			this.expected = expected;
		}

		private Number output;
		private Number expected;
        private int index;

		// partial derivative of the total error with respect to concerned output,
		public Number asNumber()
        {
			return new Substr(output, expected);
        }

		public Number derive()
		{
			return new Mult(
				output,
				new Substr(1, output)
			);
		}

		public int neuroneIndex()
        {
            return this.index;
        }
    }

}

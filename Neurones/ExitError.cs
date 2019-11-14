namespace Neurones
{
	public class ExitError 
    {
        public ExitError(int index, double expected)
        {
            this.index = index;
			this.expected = expected;
		}

		private double expected;
        private int index;

		// partial derivative of the total error with respect to concerned output,
		public double expectedResult()
        {
			return this.expected ;
        }

		public int neuroneIndex()
        {
            return this.index;
        }
    }

}

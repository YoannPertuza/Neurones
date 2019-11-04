namespace Neurones
{
    public class OutputExpected : Error
    {
        public OutputExpected(int index, double number) : this(index, new DefaultNumber(number))
        {
        }

        public OutputExpected(int index, Number number)
        {
            this.index = index;
            this.number = number;
        }

        private Number number;
        private int index;

        public Number asNumber()
        {
            return this.number;
        }

        public int neuroneIndex()
        {
            return this.index;
        }

		public Number derive()
		{
			throw new System.NotImplementedException();
		}
	}

}

namespace Neurones
{
    public class DefaultNumber : Number
	{
		public DefaultNumber(double number)
		{
			this.number = number;
		}

		private double number;

		public double value()
		{
			return this.number;
		}
	}

}

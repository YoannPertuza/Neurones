namespace Neurones
{
	public class Div : Number
	{
		public Div(Number numerator, Number denominator)
		{
			this.numerator = numerator;
			this.denominator = denominator;
		}

		private Number numerator;
		private Number denominator;

		public double value()
		{
			return numerator.value() / denominator.value();
		}
	}

}

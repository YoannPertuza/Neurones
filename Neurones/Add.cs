using System.Linq;

namespace Neurones
{
    public class Add : Number
	{
		public Add(params Number[] numbers)
		{
			this.numbers = numbers;
		}

		private Number[] numbers;

		public double value()
		{
			return numbers.ToList().Aggregate(0d, (acc, n) => acc + n.value());
		}
	}

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

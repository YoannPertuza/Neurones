using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class Mult : Number
	{
		public Mult(params Number[] numbers)
		{
			this.numbers = numbers;
		}

		private IEnumerable<Number> numbers;

		public double value()
		{
			return numbers.ToList().Aggregate(1d, (acc, n) => acc * n.value());
		}
	}

}

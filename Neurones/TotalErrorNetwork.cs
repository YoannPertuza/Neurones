using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
	public class TotalErrorNetwork : Number
	{
		public TotalErrorNetwork(Layer outputLayer, IEnumerable<Error> expectedResult)
		{
			this.outputLayer = outputLayer;
			this.expectedResult = expectedResult;
		}

		private Layer outputLayer;
		private IEnumerable<Error> expectedResult;

		public double value()
		{
			return new Add(
				this.expectedResult.Select(
					e =>
					new Div(
						new Power2(
							new Substr(
								e.asNumber(),
								this.outputLayer.neuroneValue(e.neuroneIndex())
							)
						),
						new DefaultNumber(2)
						)
					).ToArray()
				).value(); 
		}
	}

}

using System;

namespace Neurones
{
	public class TanH : Number
	{
		public TanH(double toTanH) : this(new DefaultNumber(toTanH))
		{
		}

		public TanH(Number toTanH)
		{
			this.toTanH = toTanH;
		}

		private Number toTanH;
		public double value()
		{
			return (2 / (1 + Math.Exp(-2 * toTanH.value()))) - 1;
		}
	}
}

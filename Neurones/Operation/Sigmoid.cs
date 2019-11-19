using System;

namespace Neurones
{
	public class Sigmoid : Number
	{
		public Sigmoid(double toSigmoid) : this(new DefaultNumber(toSigmoid))
		{		
		}

		public Sigmoid(Number toSigmoid)
		{
			this.toSigmoid = toSigmoid;
		}

		private Number toSigmoid;
		public double value()
		{
			return 1 / (1 + Math.Exp(-toSigmoid.value()));
		}
	}
}

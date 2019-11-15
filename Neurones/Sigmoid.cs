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

	public interface ActivationFnc
	{
		Number apply(Number x);
		Number derive(Number x);
	}

	public class SigmoidFnc : ActivationFnc
	{
		public Number apply(Number x)
		{
			return new Sigmoid(x);
		}

		public Number derive(Number x)
		{
			return new Mult(new Sigmoid(x), new Substr(1, new Sigmoid(x)));
		}
	}

	public class TanHFnc : ActivationFnc
	{
		public Number apply(Number x)
		{
			return new TanH(x);
		}

		public Number derive(Number x)
		{
			return new Substr(1, new Exp(new TanH(x), 2));
		}
	}
}

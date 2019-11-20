namespace Neurones
{
    public class ReLU : Number
    {
        public ReLU(double x) : this(new DefaultNumber(x))
		{		
		}

        public ReLU(Number x)
		{
			this.x = x;
		}

		private Number x;
		public double value()
		{
			return this.x.value() < 0 ? 0 : this.x.value();
		}
    }

	public class ReLUFnc : ActivationFnc
	{
		public Number apply(Number x)
		{
			return new ReLU(x);
		}

		public Number derive(Number x)
		{
			return x.value() < 0 ? new DefaultNumber(0) : new DefaultNumber(1);
		}
	}

	public class LeakyReLU : Number
	{
		public LeakyReLU(double x, double alpha) : this(new DefaultNumber(x), new DefaultNumber(alpha))
		{
		}

		public LeakyReLU(Number x, double alpha) : this(x, new DefaultNumber(alpha))
		{
		}

		public LeakyReLU(Number x, Number alpha)
		{
			this.x = x;
			this.alpha = alpha;
		}

		private Number x;
		private Number alpha;

		public double value()
		{
			return x.value() < 0 ? new Mult(this.alpha, this.x).value() : this.x.value();
		}
	}

	public class LeakyReLUFnc : ActivationFnc
	{
		public Number apply(Number x)
		{
			return new LeakyReLU(x, 0.3);
		}

		public Number derive(Number x)
		{
			return x.value() < 0 ? new DefaultNumber(0.3) : new DefaultNumber(1);
		}
	}



}

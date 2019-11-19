using System;

namespace Neurones
{
	public class Exp : Number
	{
		public Exp(Number numberToExp, double exp)
		{
			this.numberToExp = numberToExp;
			this.exp = exp;
		}

		private Number numberToExp;
		private double exp;

		public double value()
		{
			return Math.Pow(this.numberToExp.value(), exp);
		}
	}

	public class Power2 : Number
	{
		public Power2(Number numberToExp)
		{
			this.numberToExp = new Exp(numberToExp, 2);
		}

		private Number numberToExp;
	

		public double value()
		{
			return this.numberToExp.value();
		}
	}

}

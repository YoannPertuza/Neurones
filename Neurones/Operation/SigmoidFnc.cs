namespace Neurones
{
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
}

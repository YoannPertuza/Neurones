namespace Neurones
{
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

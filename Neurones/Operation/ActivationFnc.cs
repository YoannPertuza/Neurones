namespace Neurones
{
	public interface ActivationFnc
	{
		Number apply(Number x);
		Number derive(Number x);
	}
}

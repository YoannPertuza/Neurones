namespace Neurones
{
	public class ForwardPropagation : Propagation
	{
		public ForwardPropagation(Layer inputLayer, params Layer[] layers)
		{
			this.linkedLayers = new LinkedPrevLayer(inputLayer, layers);
		}

		private LinkedPrevLayer linkedLayers;

		public Layer execute()
		{
			return linkedLayers.link().lastLayer().propagate();
		}
	}

}

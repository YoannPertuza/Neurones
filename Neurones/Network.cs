using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class Network
    {
        public Network(IEnumerable<Layer> layers)
        {
            this.layers = layers;
        }

        private IEnumerable<Layer> layers;

		public Layer train(IEnumerable<TrainingValue> trainingValues)
		{	
			if (trainingValues.Any())
			{
				return
					new Network(
						new LinkNextLayer(
							new LinkedPrevLayer(
								trainingValues.First().InputLayer, this.layers.ToArray()
							)
								.linkWithPrevLayers()
								.lastLayer()
								.propagate().layerListFromLast().Reverse().ToArray()
							).linkedLayers()
							.firstLayer()
							.backProp(trainingValues.First().Errors)
							.layerListFromFirst().Reverse().Skip(1)
						).train(trainingValues.Skip(1));
			} else
			{
				return new LinkedPrevLayer(this.layers).linkWithPrevLayers().lastLayer();
			}									
		}

	

    }

	public class TrainingValue
	{
		public TrainingValue(Layer inputLayer, IEnumerable<ExitError> errors)
		{
			this.InputLayer = inputLayer;
			this.Errors = errors;
		}

		public Layer InputLayer;
		public IEnumerable<ExitError> Errors;
	}

}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class InputLayer : Layer
	{
		public InputLayer(params Neurone[] neurones) : this(new NullLayer(), neurones)
		{
		}

		public InputLayer(Layer nextLayer, params Neurone[] neurones)
		{
            this.neurones = neurones;
			this.nextLayer = nextLayer;
		}


        private IEnumerable<Neurone> neurones;
		private Layer nextLayer;

		public Number neuroneValue(int originNeurone)
		{
            return neurones.ToList().Find(n => n.find(originNeurone)).outputValue(new NullLayer());
		}

        public Layer withPrevLayer(Layer layer, int index)
        {
            throw new Exception("CANNOT LINKED LAYER ON INPUT LAYER");
        }

		public Layer withNextLayer(Layer layer, int index)
		{
			return new InputLayer(layer, this.neurones.ToArray());
		}

		public Layer backProp(IEnumerable<ExitError> errors)
        {
			return new InputLayer(
					this.nextLayer.backProp(errors),
					this.neurones.ToArray()
				);
        }

        public Layer propagate()
        {
            return new InputLayer(this.neurones.ToArray());
        }

        public Layer withNewSet(Layer inputLayer)
        {
            return inputLayer;
        }

		
		public int index()
		{
			return 0;
		}

		public Neurone neuroneInLayer(int indexLayer, int indexNeurone)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Layer> layerList()
		{
			return new List<Layer>() { this };
		}

		public Number deriveRespectToOut(IEnumerable<ExitError> errors, Layer nextLayer, int indexNeuroneFrom)
		{
			throw new NotImplementedException();
		}
	}

}

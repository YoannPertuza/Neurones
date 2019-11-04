using System;
using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class InputLayer : Layer
	{
        public InputLayer(params Neurone[] neurones)
		{
            this.neurones = neurones;
		}


        private IEnumerable<Neurone> neurones;

		public Number neuroneValue(int originNeurone)
		{
            return neurones.ToList().Find(n => n.find(originNeurone)).outputValue(new NullLayer());
		}

        public Layer withPrevLayer(Layer layer, int index)
        {
            throw new Exception("CANNOT LINKED LAYER ON INPUT LAYER");
        }

        public Layer backProp(IEnumerable<Error> errors)
        {
			return new InputLayer(
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

		public Layer applyCorrections()
		{
			return new InputLayer(
				this.neurones.ToArray()
			);
		}

		public int index()
		{
			return 0;
		}

		public Neurone neuroneInLayer(int indexLayer, int indexNeurone)
		{
			throw new NotImplementedException();
		}
	}

}

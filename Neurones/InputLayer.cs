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

		public Number outputValue(int originNeurone)
		{
            return neurones.ToList().Find(n => n.find(originNeurone)).outputValue(new NullLayer());
		}

        public Layer withPrevLayer(Layer layer)
        {
            throw new Exception("CANNOT LINKED LAYER ON INPUT LAYER");
        }

        public Layer backProp(IEnumerable<Error> errors, IEnumerable<Synapse> synapses)
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
	}

}

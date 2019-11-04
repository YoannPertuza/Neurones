using System;
using System.Collections.Generic;

namespace Neurones
{
    public class NullLayer : Layer 
    {
        public Number neuroneValue(int targetNeurone) {
            throw new Exception("YOU HAVE TO LINK YOUR LAYERS");
        }

        public Layer withPrevLayer(Layer layer, int index)
        {
            throw new Exception("CANNOT LINK NULL LAYER");
        }

        public Layer backProp(IEnumerable<Error> errors)
        {
            throw new Exception("CANNOT LINK NULL LAYER");
        }

        public Layer propagate()
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }

        public Layer withNewSet(Layer inputLayer)
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }

        public Layer backProp(IEnumerable<Error> errors, IEnumerable<Synapse> synapses)
        {
            throw new NotImplementedException();
        }

		public Layer applyCorrections()
		{
			throw new NotImplementedException();
		}

		public int index()
		{
			throw new NotImplementedException();
		}

		public Neurone neuroneInLayer(int indexLayer, int indexNeurone)
		{
			throw new NotImplementedException();
		}
	}

}

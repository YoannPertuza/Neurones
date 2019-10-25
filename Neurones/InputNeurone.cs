using System;
using System.Collections.Generic;

namespace Neurones
{
    public class InputNeurone : Neurone
    {
        public InputNeurone(int index, double value)
		{
			this.index = index;
            this.value = new DefaultNumber(value);
		}

        private int index;
        private Number value;

        public Number outputValue(Layer prevLayer)
        {
            return value;
        }

        public bool find(int targetNeurone)
        {
            return targetNeurone == this.index;
        }

		public IEnumerable<Synapse> synapsesFrom()
        {
            throw new Exception("INPUT NEURONE DOES NOT HAVE INPUT SYNAPSE");
        }

        public Error error(IEnumerable<Error> nextErrors, IEnumerable<Synapse> synapses)
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }

        public Neurone withNewSynapse(IEnumerable<Error> nextErrors, Layer prev)
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }

        public Neurone withValue(Layer layer)
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }

        public double val()
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }


    }

}

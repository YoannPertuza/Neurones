using System;
using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class InputNeurone : Neurone
    {
        public InputNeurone(int index, double value) : this(index, new DefaultNumber(value), new List<Error>())
		{
		}

		

		public InputNeurone(int index, Number value, IEnumerable<Error> errors)
		{
			this.index = index;
			this.value = value;
			this.errors = errors;
		}

		private int index;
        private Number value;
		private IEnumerable<Error> errors;

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

        public Neurone withValue(Layer layer)
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }

        public double val()
        {
			return this.value.value();
        }

		public Neurone withError(IEnumerable<Error> nextErrors)
		{
			return new InputNeurone(
				this.index,
				this.value,
				new List<Error>(this.errors) { nextErrors.FirstOrDefault(e => e.neuroneIndex() == this.index) }
			);
		}

		public Neurone applyCorrections(Layer prevLayer)
		{
			throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
		}
	}

}

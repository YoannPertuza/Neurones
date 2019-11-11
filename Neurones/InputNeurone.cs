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

        public Neurone withValue(Layer layer)
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }

        public double val()
        {
			return this.value.value();
        }

		public Neurone withError(IEnumerable<ExitError> nextErrors, Layer prevLayer, Layer nextLayer)
		{
			return new InputNeurone(
				this.index,
				this.value,
				new List<Error>() {  }
			);
		}

		public Neurone applyCorrections(Layer prevLayer)
		{
			throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
		}

		public Synapse synapseFrom(int indexPreviousNeurone)
		{
			throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
		}

		public Error lastError()
		{
			throw new NotImplementedException();
		}

		public Neurone withError(ExitError error, Layer prevLayer)
		{
			throw new NotImplementedException();
		}

		public Number deriveRespectToOut(IEnumerable<ExitError> errors, Layer nextLayer)
		{
			throw new NotImplementedException();
		}


		public Number deriveRespectToIn()
		{
			throw new NotImplementedException();
		}

		public Number deriveRespectToWeight(IEnumerable<ExitError> errors, Layer nextLayer, int indexNeuroneFrom)
		{
			throw new NotImplementedException();
		}
	}

}

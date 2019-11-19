using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Neurone withValue(Layer layer)
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }


		public Neurone withNewSynapses(IEnumerable<ExitError> nextErrors, Layer prevLayer, Layer nextLayer)
		{
			return new InputNeurone(
				this.index,
				this.value,
				new List<Error>() {  }
			);
		}

		public Synapse synapseFrom(int indexPreviousNeurone)
		{
			throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
		}

		public Number deriveRespectToWeight(IEnumerable<ExitError> errors, Layer nextLayer, int indexNeuroneFrom)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("[");
			sb.Append($"{this.value.value()}");
			sb.Append("]\r\n");

			return sb.ToString();
		}
	}

}

using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class DeepNeurone : Neurone
    {

        public DeepNeurone(int index, params Synapse[] synapses)
		{
			this.index = index;
			this.synapses = synapses;
            this.value = new NullNumber();
		}

        public DeepNeurone(int index, Number value, params Synapse[] synapses)
        {
            this.index = index;
            this.synapses = synapses;
            this.value = value;
        }

		private int index;
		private IEnumerable<Synapse> synapses;
        private Number value;

        public IEnumerable<Synapse> synapsesFrom()
        {
            return this.synapses;
        }

		public Number outputValue(Layer prevLayer)
		{
             return
                 new Sigmoid(
                    new Add(
                        this.synapses.ToList().Select(s => s.value(prevLayer)).ToArray()
                    )
                );
		}

        public double val()
        {
            return this.value.value();
        }

        public bool find(int targetNeurone)
        {
            return targetNeurone == this.index;
        }

        public Error error(IEnumerable<Error> nextErrors, IEnumerable<Synapse> synapses)
        {
            return
                new ExitError(
                    this.index,
                    new Mult(
                        this.value,
                        new Substr(1, this.value),
                        new Add(
                            synapses
                            .Where(s => s.isFromNeurone(this.index))
                            .Select(s => s.error(nextErrors)).ToArray()
                        )
                    )
                );               
        }

        public Neurone withValue(Layer prevLayer)
        {
            return new DeepNeurone(this.index, this.outputValue(prevLayer), this.synapses.ToArray());
        }

        public Neurone withNewSynapse(IEnumerable<Error> nextErrors, Layer prev)
        {
            var neuroneError = nextErrors.ToList().Find(e => e.neuroneIndex() == this.index).asNumber();
          
            return
                new DeepNeurone(
                    this.index,
                    this.synapses.Select(s => s.withAdjustedWeight(neuroneError, prev)).ToArray()
                );
                    
        }
	}

}

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

        public Error error(IEnumerable<Error> errors, IEnumerable<Synapse> synapses)
        {
			var syn = synapses.Where(s => s.isFromNeurone(this.index));

			var concernedErrors = errors.Where(es => syn.Any(s => s.isToNeurone(es.neuroneIndex())));

			var error = new ExitError(
					this.index, 
					new Add(
						concernedErrors.Select(
							err =>
								new Div(
									new Mult(
										err.asNumber(),
										syn.FirstOrDefault(s => s.isToNeurone(err.neuroneIndex()) && s.isFromNeurone(this.index)).weight
									),
									new Add(
										synapses.Where(s => s.isToNeurone(err.neuroneIndex())).Select(sv => sv.weight).ToArray()
									)
								)
						).ToArray()
					));

			return error;
		}

        public Neurone withValue(Layer prevLayer)
        {
            return new DeepNeurone(this.index, this.outputValue(prevLayer), this.synapses.ToArray());
        }

        public Neurone withNewSynapse(IEnumerable<Error> nextErrors, Layer prev)
        {   
            return
                new DeepNeurone(
                    this.index,
                    this.synapses.Select(s => s.withAdjustedWeight(nextErrors, prev, this.value)).ToArray()
                );
                    
        }
	}

}

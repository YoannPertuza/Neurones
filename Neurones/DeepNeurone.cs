using System;
using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class DeepNeurone : Neurone
    {

        public DeepNeurone(int index, params Synapse[] synapses) : this(index, new NullNumber(), new List<Error>(), synapses)
		{
		}

		public DeepNeurone(int index, Number value, params Synapse[] synapses) : this(index, value, new List<Error>(), synapses)
		{
		}

		public DeepNeurone(int index, Number value, IEnumerable<Error> errors, params Synapse[] synapses)
        {
            this.index = index;
            this.synapses = synapses;
            this.value = value;
			this.errors = errors;
        }

		private int index;
		private IEnumerable<Synapse> synapses;
        private Number value;
		private IEnumerable<Error> errors;

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
				new Mult(
						new DefaultNumber(this.val()),
						new Substr(1, this.val()),
						new Add(
							concernedErrors.Select(
								err =>
								new Mult(
									err.asNumber(),
									syn.FirstOrDefault(s => s.isToNeurone(err.neuroneIndex()) && s.isFromNeurone(this.index)).weight
								)
							).ToArray()
						)
					)
				);

			return error;
		}

        public Neurone withValue(Layer prevLayer)
        {
            return new DeepNeurone(this.index, this.outputValue(prevLayer), this.errors, this.synapses.ToArray());
        }

        
		public Neurone withError(IEnumerable<Error> nextErrors, Layer prevLayer)
		{
			return new DeepNeurone(
				this.index,
				this.value,
				new List<Error>(this.errors) { },
				this.synapses.Select(
					s =>
					s.withError(
						new Mult(
							new Mult(
								new DefaultNumber(-1),
								nextErrors.First().asNumber(),
								s.weight
							),
							this.value,
							new Substr(1, this.value)
						),
						prevLayer
					)
				).ToArray()
			);
		}

		public Neurone applyCorrections(Layer prevLayer)
		{
			return new DeepNeurone(
				this.index,
				this.value,
				this.errors,
				this.synapses.Select(s => s.withAdjustedWeight(null, prevLayer)).ToArray()
			);
		}

		public Synapse synapseFrom(int indexPreviousNeurone)
		{
			return this.synapses.FirstOrDefault(s => s.isFromNeurone(indexPreviousNeurone));
		}

		public Error lastError()
		{
			return this.errors.Last();
		}
	}



	public class OutputNeurone : Neurone
	{

		public OutputNeurone(int index, params Synapse[] synapses) : this(index, new NullNumber(), new List<Error>(), synapses)
		{
		}

		public OutputNeurone(int index, Number value, params Synapse[] synapses) : this(index, value, new List<Error>(), synapses)
		{
		}

		public OutputNeurone(int index, Number value, IEnumerable<Error> errors, params Synapse[] synapses)
		{
			this.index = index;
			this.synapses = synapses;
			this.value = value;
			this.errors = errors;
		}

		private int index;
		private IEnumerable<Synapse> synapses;
		private Number value;
		private IEnumerable<Error> errors;

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
				new Mult(
						new DefaultNumber(this.val()),
						new Substr(1, this.val()),
						new Add(
							concernedErrors.Select(
								err =>
								new Mult(
									err.asNumber(),
									syn.FirstOrDefault(s => s.isToNeurone(err.neuroneIndex()) && s.isFromNeurone(this.index)).weight
								)
							).ToArray()
						)
					)
				);

			return error;
		}

		public Neurone withValue(Layer prevLayer)
		{
			return new OutputNeurone(this.index, this.outputValue(prevLayer), this.errors, this.synapses.ToArray());
		}


		public Neurone withError(IEnumerable<Error> nextErrors, Layer prevLayer)
		{
			return new OutputNeurone(
				this.index,
				this.value,
				new List<Error>(this.errors) {  },
				this.synapses.Select(
					s =>
					s.withError(
						new Mult(
							new Mult(
								new DefaultNumber(-1),
								nextErrors.First().asNumber()
							),
							this.value,
							new Substr(1, this.value)
						),
						prevLayer
					)
				).ToArray()
			);
		}

		public Neurone applyCorrections(Layer prevLayer)
		{
			return new OutputNeurone(
				this.index,
				this.value,
				this.errors,
				this.synapses.Select(s => s.withAdjustedWeight(null, prevLayer)).ToArray()
			);
		}

		public Synapse synapseFrom(int indexPreviousNeurone)
		{
			return this.synapses.FirstOrDefault(s => s.isFromNeurone(indexPreviousNeurone));
		}

		public Error lastError()
		{
			return this.errors.Last();
		}
	}

}

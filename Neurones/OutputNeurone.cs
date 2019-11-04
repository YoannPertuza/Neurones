﻿using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
	public class OutputNeurone : Neurone
	{

		public OutputNeurone(int index, params Synapse[] synapses) : this(index, new NullNumber(), new List<Error>(), 0, synapses)
		{
		}

		public OutputNeurone(int index, double bias, params Synapse[] synapses) : this(index, new NullNumber(), new List<Error>(), bias, synapses)
		{
		}

		public OutputNeurone(int index, Number value, params Synapse[] synapses) : this(index, value, new List<Error>(), 0, synapses)
		{
		}

		public OutputNeurone(int index, Number value, IEnumerable<Error> errors, double bias, params Synapse[] synapses)
		{
			this.index = index;
			this.synapses = synapses;
			this.value = value;
			this.errors = errors;
			this.bias = bias;
		}

		private int index;
		private IEnumerable<Synapse> synapses;
		private Number value;
		private IEnumerable<Error> errors;
		private double bias;

		public IEnumerable<Synapse> synapsesFrom()
		{
			return this.synapses;
		}

		public Number outputValue(Layer prevLayer)
		{
			return
				 new Sigmoid(
					 new Add(
						new Add(
							this.synapses.ToList().Select(s => s.value(prevLayer)).ToArray()
						),
						new DefaultNumber(this.bias)
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

		public Neurone withValue(Layer prevLayer)
		{
			return new OutputNeurone(this.index, this.outputValue(prevLayer), this.errors, this.bias, this.synapses.ToArray());
		}


		public Neurone withError(IEnumerable<Error> nextErrors, Layer prevLayer)
		{
			return new OutputNeurone(
				this.index,
				this.value,
				new List<Error>(this.errors) {  },
				this.bias,
				this.synapses.Select(
					s =>
					s.withError(
						new Mult(							
							nextErrors.FirstOrDefault(e => e.neuroneIndex() == this.index).asNumber(),
							nextErrors.FirstOrDefault(e => e.neuroneIndex() == this.index).derive()
						),
						prevLayer
					)
				).ToArray()
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

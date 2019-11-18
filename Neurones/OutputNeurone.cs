using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
	public class OutputNeurone : Neurone
	{

		public OutputNeurone(int index, params Synapse[] synapses) : this(index, new NullNumber(), 0, new TanHFnc(), synapses)
		{
		}

		public OutputNeurone(int index, double bias, params Synapse[] synapses) : this(index, new NullNumber(), bias, new TanHFnc(), synapses)
		{
		}

	

		public OutputNeurone(int index, Number value,  double bias, ActivationFnc activation, params Synapse[] synapses)
		{
			this.index = index;
			this.synapses = synapses;
			this.value = value;
			this.bias = bias;
			this.activation = activation;
		}

		private int index;
		private IEnumerable<Synapse> synapses;
		private Number value;
		private double bias;
		private ActivationFnc activation;

		public Number outputValue(Layer prevLayer)
		{
			return
				 this.activation.apply(
					 new Add(
						new Add(
							this.synapses.ToList().Select(s => s.value(prevLayer)).ToArray()
						),
						new DefaultNumber(this.bias)
					)
				);
		}


		public bool find(int targetNeurone)
		{
			return targetNeurone == this.index;
		}

		public Neurone withValue(Layer prevLayer)
		{
			return new OutputNeurone(this.index, this.outputValue(prevLayer), this.bias, this.activation, this.synapses.ToArray());
		}


		public Neurone withNewSynapses(IEnumerable<ExitError> errors, Layer prevLayer, Layer nextLayer)
		{
			return new OutputNeurone(
				this.index,
				this.value,
				this.bias,
				this.activation,
				this.synapses.Select(
					s => 
						s.withAdjustedWeight(
							this.activation.derive(this.value),
							new Substr(this.value, errors.FirstOrDefault(e => e.neuroneIndex() == this.index).expectedResult()), 
							prevLayer)
						).ToArray()
					);
		}

		public Synapse synapseFrom(int indexPreviousNeurone)
		{
			return this.synapses.FirstOrDefault(s => s.isFromNeurone(indexPreviousNeurone));
		}


		public Number deriveRespectToWeight(IEnumerable<ExitError> errors, Layer nextLayer, int indexNeuroneFrom)
		{
			return this.synapseFrom(indexNeuroneFrom).deriveWeight(
					this.activation.derive(this.value),
					new Substr(this.value, errors.FirstOrDefault(e => e.neuroneIndex() == this.index).expectedResult())
				);
		}
	}

}

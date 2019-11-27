using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Neurones
{
	public class DeepNeurone : Neurone
    {

        public DeepNeurone(int index, params Synapse[] synapses) : this(index, new NullNumber(), 0, new TanHFnc(), synapses)
		{
		}

		public DeepNeurone(int index, ActivationFnc activation, params Synapse[] synapses) : this(index, new NullNumber(), 0, activation, synapses)
		{
		}
		public DeepNeurone(int index, double bias, params Synapse[] synapses) : this(index, new NullNumber(), bias, new TanHFnc(), synapses)
		{
		}

		public DeepNeurone(int index, double bias, ActivationFnc activation, params Synapse[] synapses) : this(index, new NullNumber(), bias, activation, synapses)
		{
		}

		public DeepNeurone(int index, Number value, params Synapse[] synapses) : this(index, value, 0, new TanHFnc(), synapses)
		{
		}

		public DeepNeurone(int index, Number value,  double bias, ActivationFnc activation, params Synapse[] synapses)
        {
            this.index = index;
            this.synapses = synapses;
            this.value = value;
			this.bias = bias;
			this.activation = activation;
			this.localCache = new DeriveCache();
	}

		private int index;
		private IEnumerable<Synapse> synapses;
        private Number value;
		private double bias;
		private ActivationFnc activation;
		private DeriveCache localCache;

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
            return 
				new DeepNeurone(
					this.index,
					new Add(
						new Add(
							this.synapses.ToList().Select(s => s.value(prevLayer)).ToArray()
						),
						new DefaultNumber(this.bias)
					), 
					this.bias, 
					this.activation, 
					this.synapses.ToArray()
				);
        }

        
		public Synapse synapseFrom(int indexPreviousNeurone)
		{
			return this.synapses.FirstOrDefault(s => s.isFromNeurone(indexPreviousNeurone));
		}


		public Neurone withNewSynapses(IEnumerable<ExitError> errors, Layer prevLayer, Layer nextLayer)
		{
			return new DeepNeurone(
				this.index, 
				this.value, 
				this.bias,
				this.activation,
				this.synapses.Select(
					s =>
						s.withAdjustedWeight(
							this.activation.derive(this.value),
							nextLayer.deriveRespectToOut(errors, nextLayer, this.index),
							prevLayer)
						).ToArray()
					);
		}

		public Number deriveRespectToWeight(IEnumerable<ExitError> errors, Layer nextLayer, int indexNeuroneFrom)
		{
			return this.localCache.get(
							indexNeuroneFrom, 
							() => this.synapseFrom(indexNeuroneFrom).deriveWeight(
								this.activation.derive(this.value),
								nextLayer.deriveRespectToOut(errors, nextLayer, this.index)
							)
						);
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("[");
			sb.Append($"{this.activation.apply(this.value).value()}, ");
			sb.Append(String.Join(", ", this.synapses.Select(s => s.ToString())));
			sb.Append("]\r\n");

			return sb.ToString();
		}
	}

}

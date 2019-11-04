using System;
using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class Synapse
	{
        public Synapse(int originNeurone, int destinNeurone, double weight) : this(originNeurone, destinNeurone, weight, new List<Number>())
		{

		}

		public Synapse(int originNeurone, int destinNeurone, double weight, List<Number> gradientErrors)
		{
			this.originNeurone = originNeurone;
			this.destinNeurone = destinNeurone;
			this.weight = new DefaultNumber(weight);
			this.gradientErrors = gradientErrors;

		}

		private int originNeurone;
        private int destinNeurone;
		public Number weight;
		public List<Number> gradientErrors;

		public bool isFromNeurone(int neuroneIndex)
		{
            return neuroneIndex == this.originNeurone;
		}

		public bool isToNeurone(int neuroneIndex)
		{
			return neuroneIndex == this.destinNeurone;
		}

		public Number value(Number input)
        {
            return new Mult(input, this.weight);
        }

		public Number value(Layer prevLayer)
		{
            return new DefaultNumber(this.value(prevLayer.neuroneValue(this.originNeurone)).value());
		}

        
		public Synapse withError(Number error, Layer prevLayer)
		{
			return new Synapse(
					this.originNeurone,
					this.destinNeurone,
					new Substr(
						this.weight, 
						new Mult(
							new DefaultNumber(0.5),
							new Mult(
								error,
								prevLayer.neuroneValue(this.originNeurone)
							)
						)
					).value(),
					new List<Number>(this.gradientErrors) {
						new Mult(error, prevLayer.neuroneValue(this.originNeurone)) 
					}
				);
		}
	}

}

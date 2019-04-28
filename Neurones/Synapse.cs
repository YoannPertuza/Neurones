using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class Synapse
	{
        public Synapse(int originNeurone, int destinNeurone, double weight)
		{
            this.originNeurone = originNeurone;
            this.destinNeurone = destinNeurone;
			this.weight = new DefaultNumber(weight);
		}

        private int originNeurone;
        private int destinNeurone;
		private Number weight;

		public bool isFromNeurone(int neuroneIndex)
		{
            return neuroneIndex == this.originNeurone;
		}

         public Number value(Number input)
        {
            return new Mult(input, this.weight);
        }

		public Number value(Layer prevLayer)
		{
            return new DefaultNumber(this.value(prevLayer.outputValue(this.originNeurone)).value());
		}

        public Number error(IEnumerable<Error> errors)
        {
            return this.value(errors.ToList().Find(e => e.neuroneIndex() == this.destinNeurone).asNumber());
        }

        public Synapse withAdjustedWeight(Number neuroneError, Layer prev)
        {
            return
                new Synapse(
                    this.originNeurone,
                    this.destinNeurone,
                    new Add(
                        this.weight,
                        new Mult(
                            new DefaultNumber(0.1),
                            neuroneError,
                            prev.outputValue(this.originNeurone)
                        )
                    ).value()
                );
        }
	}

}

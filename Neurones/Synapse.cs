﻿using System.Collections.Generic;
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
		public Number weight;


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
            return new DefaultNumber(this.value(prevLayer.outputValue(this.originNeurone)).value());
		}

        public Synapse withAdjustedWeight(Error neuroneError, Layer prev)
        {
            return
                new Synapse(
                    this.originNeurone,
                    this.destinNeurone,
                    new Add(
                        this.weight,
                        new Mult(
							new DefaultNumber(0.1),
							prev.outputValue(this.originNeurone),
							neuroneError.asNumber()
						)
                    ).value()
                );
        }
	}

}

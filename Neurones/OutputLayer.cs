using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neurones
{
	public class OutputLayer : Layer
    {
        public OutputLayer(params Neurone[] neurones) : this(0, new NullLayer(), neurones)
        {
        }

        public OutputLayer(Layer prevLayer, params Neurone[] neurones) : this(0, prevLayer, neurones)
        {
        }

		public OutputLayer(int indexLayer, Layer prevLayer, params Neurone[] neurones)
		{
			this.indexLayer = indexLayer;
			this.prevLayer = prevLayer;
			this.neurones = neurones;
		}


		private int indexLayer;
        private IEnumerable<Neurone> neurones;
        private Layer prevLayer;

        public Number neuroneValue(int originNeurone)
        {
            return neurones.ToList().Find(n => n.find(originNeurone)).outputValue(this.prevLayer);
        }

        public Layer withPrevLayer(Layer layer, int index)
        {
            return new OutputLayer(index, layer, this.neurones.ToArray());
        }

		public Layer withNextLayer(Layer layer)
		{
			throw new Exception("CANNOT LINKED LAYER ON NEXT LAYER");
		}

		public Layer propagate()
        {
            return
                new OutputLayer(
					this.indexLayer,
                    prevLayer.propagate(),
                    this.neurones.Select(n => n.withValue(this.prevLayer)).ToArray()
                );
        }

        public Layer backProp(IEnumerable<ExitError> errors)
        {
            return
				new OutputLayer(
					this.indexLayer,
					prevLayer,
					this.neurones.Select(n => n.withNewSynapses(errors, prevLayer, new NullLayer())).ToArray()
				);
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("--- OUTPUT LAYER --- \r\n");
			sb.Append(String.Join(string.Empty, this.neurones.Select(n => n.ToString()).ToArray()));

			sb.Append(this.prevLayer.ToString());

			return sb.ToString();
		}

		public Layer withNewSet(Layer inputLayer)
        {
            return new OutputLayer(
				this.indexLayer,
				this.prevLayer.withNewSet(inputLayer), 
				this.neurones.ToArray()
			);
        }


		public int index()
		{
			return this.indexLayer;
		}

		public Neurone neuroneInLayer(int indexLayer, int indexNeurone)
		{
			if (this.indexLayer == indexLayer)
			{
				return neurones.ToList().Find(n => n.find(indexNeurone));
			}
			else
			{
				return this.prevLayer.neuroneInLayer(indexLayer, indexNeurone);
			}
		}

		public IEnumerable<Layer> toListFromLast()
		{
			return new List<Layer>(prevLayer.toListFromLast()) { this }; 
		}

		public IEnumerable<Layer> toListFromFirst()
		{
			return new List<Layer>() { this };
		}

		public Number deriveRespectToOut(IEnumerable<ExitError> errors, Layer nextLayer, int indexNeuroneFrom)
		{
			return new Add(
				this.neurones.Select(n => n.deriveRespectToWeight(errors, new NullLayer(), indexNeuroneFrom)).ToArray()
			);
		}
	}

}

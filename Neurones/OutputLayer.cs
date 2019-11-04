using System.Collections.Generic;
using System.Linq;

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

        public Layer propagate()
        {
            return
                new OutputLayer(
					this.indexLayer,
                    prevLayer.propagate(),
                    this.neurones.Select(n => n.withValue(this.prevLayer)).ToArray()
                );
        }

        public Layer backProp(IEnumerable<Error> errors)
        {
            return
                new OutputLayer(
					this.indexLayer,
                    prevLayer.backProp(errors),
                    this.neurones.Select(n => n.withError(errors,  this.prevLayer)).ToArray()
                );
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
	}

}

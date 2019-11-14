using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
	public class DeepLayer : Layer
	{
        public DeepLayer(params Neurone[] neurones) : this(new NullLayer(), new NullLayer(), neurones)
		{
		}

		public DeepLayer(Layer prevLayer, params Neurone[] neurones) : this(0, prevLayer, new NullLayer(), neurones)
		{
		}

		public DeepLayer(Layer prevLayer, Layer nextLayer, params Neurone[] neurones) : this(0, prevLayer, nextLayer, neurones)
		{
		}

		public DeepLayer(int indexLayer, Layer prevLayer, Layer nextLayer, params Neurone[] neurones)
		{
			this.indexLayer = indexLayer;
			this.prevLayer = prevLayer;
			this.nextLayer = nextLayer;
			this.neurones = neurones;
		}

		private int indexLayer;
		private IEnumerable<Neurone> neurones;
		private Layer prevLayer;
		private Layer nextLayer;

		public Number neuroneValue(int originNeurone)
		{
            return neurones.ToList().Find(n => n.find(originNeurone)).outputValue(this.prevLayer);				
		}

        public Layer withPrevLayer(Layer layer, int index)
        {
            return new DeepLayer(index, layer, new NullLayer(), this.neurones.ToArray());
        }

		public Layer withNextLayer(Layer layer)
		{
			return new DeepLayer(this.indexLayer, this.prevLayer, layer, this.neurones.ToArray());
		}

		public Layer propagate()
        {
            return 
                new DeepLayer(
					this.indexLayer,
					prevLayer.propagate(),
					new NullLayer(),
					this.neurones.Select(n => n.withValue(this.prevLayer)).ToArray()
                );
        }

        public Layer backProp(IEnumerable<ExitError> errors)
        {            
			return
				new DeepLayer(
					this.indexLayer,
					this.prevLayer,
					nextLayer.backProp(errors),
					this.neurones.Select(n => n.withError(errors, this.prevLayer, this.nextLayer)).ToArray()
				);
		}

        public Layer withNewSet(Layer inputLayer)
        {
            return new DeepLayer(
				this.indexLayer, 
				this.prevLayer.withNewSet(inputLayer),
				new NullLayer(),
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
			} else
			{
				return this.prevLayer.neuroneInLayer(indexLayer, indexNeurone);
			}
		}

		public IEnumerable<Layer> layerListFromLast()
		{
			return new List<Layer>(prevLayer.layerListFromLast()) { this };
		}

		
		public Number deriveRespectToOut(IEnumerable<ExitError> errors, Layer nextLayer, int indexNeuroneFrom)
		{
			return new Add(
				this.neurones.Select(n => n.deriveRespectToWeight(errors, nextLayer, indexNeuroneFrom)).ToArray()
			);
		}

		public IEnumerable<Layer> layerListFromFirst()
		{
			return new List<Layer>(nextLayer.layerListFromFirst()) { this };
		}
	}

}

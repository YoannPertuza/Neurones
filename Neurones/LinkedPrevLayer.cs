using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class LinkedPrevLayer
    {
        public LinkedPrevLayer(Layer inputLayer) : this(inputLayer, new List<Layer>().ToArray())
        {
        }

        public LinkedPrevLayer(Layer inputLayer, params Layer[] layers)
        {
            this.inputLayer = inputLayer;
            this.layers = layers;
        }

		public LinkedPrevLayer(IEnumerable<Layer> layers) : this(layers.First(), layers.Skip(1).ToArray())
		{
		}

		private Layer inputLayer;
        private IEnumerable<Layer> layers;
       
        public Layer lastLayer()
        {
            return this.inputLayer;
        }

        public LinkedPrevLayer linkWithPrevLayers()
        {       
            if (this.layers.Any())
            {
                return new LinkedPrevLayer(
                    this.layers.First().withPrevLayer(this.inputLayer, this.inputLayer.index() + 1), 
                    this.layers.Skip(1).ToArray()
                ).linkWithPrevLayers();
            }
            else
            {
                return new LinkedPrevLayer(this.inputLayer);
            }            
        }

		
	}

	public class LinkNextLayer
	{
		public LinkNextLayer(Layer inputLayer, params Layer[] layers)
		{
			this.inputLayer = inputLayer;
			this.layers = layers;
		}

		public LinkNextLayer(params Layer[] layers) : this(layers.First(), layers.Skip(1).ToArray())
		{
		}

		private Layer inputLayer;
		private IEnumerable<Layer> layers;

		public LinkNextLayer linkedLayers()
		{
			if (this.layers.Any())
			{
				return new LinkNextLayer(
					this.layers.First().withNextLayer(this.inputLayer),
					this.layers.Skip(1).ToArray()
				).linkedLayers();
			}
			else
			{
				return new LinkNextLayer(this.inputLayer);
			}
		}

		public Layer firstLayer()
		{
			return this.inputLayer;
		}
	}

}

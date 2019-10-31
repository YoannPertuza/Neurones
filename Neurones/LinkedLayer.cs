using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class LinkedLayer
    {
        public LinkedLayer(Layer inputLayer) : this(inputLayer, new List<Layer>().ToArray())
        {
        }

        public LinkedLayer(Layer inputLayer, params Layer[] layers)
        {
            this.inputLayer = inputLayer;
            this.layers = layers;
        }

        private Layer inputLayer;
        private IEnumerable<Layer> layers;
       
        public Layer lastLayer()
        {
            return this.inputLayer;
        }

        public LinkedLayer linkLayers()
        {       
            if (this.layers.Any())
            {
                return new LinkedLayer(
                    this.layers.First().withPrevLayer(this.inputLayer, this.inputLayer.index() + 1), 
                    this.layers.Skip(1).ToArray()
                ).linkLayers();
            }
            else
            {
                return new LinkedLayer(this.inputLayer);
            }            
        }
    }

}

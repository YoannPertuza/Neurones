using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class LinkedLayer
    {
        public LinkedLayer(Layer inputLayer) : this(inputLayer, new List<Layer>(), new List<Layer>().ToArray())
        {
        }


        public LinkedLayer(Layer inputLayer, params Layer[] layers) : this(inputLayer, new List<Layer>(), layers)
        {
        }

        public LinkedLayer(Layer inputLayer, IEnumerable<Layer> outputLayers, params Layer[] layers)
        {
            this.inputLayer = inputLayer;
            this.layers = layers;
            this.outputLayers = outputLayers;
        }

        private Layer inputLayer;
        private IEnumerable<Layer> layers;
        private IEnumerable<Layer> outputLayers;
       
        public Layer lastLayer()
        {
            return this.inputLayer;
        }

        public LinkedLayer linkLayers()
        {       
            if (this.layers.Any())
            {
                return new LinkedLayer(
                    this.layers.First().withPrevLayer(this.inputLayer), 
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

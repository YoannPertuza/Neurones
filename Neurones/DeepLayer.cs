using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class DeepLayer : Layer
	{
        public DeepLayer(params Neurone[] neurones) : this(new NullLayer(), neurones)
		{
		}

        public DeepLayer(Layer prevLayer, params Neurone[] neurones)
        {
            this.prevLayer = prevLayer;
            this.neurones = neurones;
        }

		private IEnumerable<Neurone> neurones;
		private Layer prevLayer;

		public Number outputValue(int originNeurone)
		{
            return neurones.ToList().Find(n => n.find(originNeurone)).outputValue(this.prevLayer);				
		}

        public Layer withPrevLayer(Layer layer)
        {
            return new DeepLayer(layer, this.neurones.ToArray());
        }

        public IEnumerable<Error> errors(IEnumerable<Error> nextLayerErrors, IEnumerable<Synapse> synapses)
        {
            return this.neurones.Select(n => n.error(nextLayerErrors, synapses));
        }

        public Layer propagate()
        {
            return 
                new DeepLayer(
                    prevLayer.propagate(), 
                    this.neurones.Select(n => n.withValue(this.prevLayer)).ToArray()
                );
        }

        public Layer backProp(IEnumerable<Error> errors)
        {
            var newErrors = this.prevLayer.errors(errors, this.neurones.SelectMany(n => n.synapsesFrom()));
            return 
                new DeepLayer(
                    prevLayer.backProp(newErrors), 
                    this.neurones.Select(n => n.withNewSynapse(errors, this.prevLayer)).ToArray()
                );
        }

        public Layer withNewSet(Layer inputLayer)
        {
            return new DeepLayer(this.prevLayer.withNewSet(inputLayer), this.neurones.ToArray());
        }

	}

}

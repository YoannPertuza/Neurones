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

        public Layer backProp(IEnumerable<Error> errors, IEnumerable<Synapse> synapses)
        {
            var newErrors = this.errors(errors, synapses);
            return 
                new DeepLayer(
                    prevLayer.backProp(newErrors, this.neurones.SelectMany(n => n.synapsesFrom())), 
                    this.neurones.Select(n => n.withNewSynapse(newErrors, this.prevLayer)).ToArray()
                );
        }

        public Layer withNewSet(Layer inputLayer)
        {
            return new DeepLayer(this.prevLayer.withNewSet(inputLayer), this.neurones.ToArray());
        }

	}

    public class OutputLayer : Layer
    {
        public OutputLayer(params Neurone[] neurones) : this(new NullLayer(), neurones)
        {
        }

        public OutputLayer(Layer prevLayer, params Neurone[] neurones)
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
            return new OutputLayer(layer, this.neurones.ToArray());
        }

        public IEnumerable<Error> errors(IEnumerable<Error> nextLayerErrors, IEnumerable<Synapse> synapses)
        {
            return this.neurones.Select(n => n.error(nextLayerErrors, synapses));
        }

        public Layer propagate()
        {
            return
                new OutputLayer(
                    prevLayer.propagate(),
                    this.neurones.Select(n => n.withValue(this.prevLayer)).ToArray()
                );
        }

        public Layer backProp(IEnumerable<Error> errors, IEnumerable<Synapse> synapses)
        {
            return
                new OutputLayer(
                    prevLayer.backProp(errors, this.neurones.SelectMany(n => n.synapsesFrom())),
                    this.neurones.Select(n => n.withNewSynapse(errors, this.prevLayer)).ToArray()
                );
        }

        public Layer withNewSet(Layer inputLayer)
        {
            return new OutputLayer(this.prevLayer.withNewSet(inputLayer), this.neurones.ToArray());
        }

    }

}

using System.Collections.Generic;

namespace Neurones
{
    public interface Layer
	{
		Number outputValue(int targetNeurone);
        Layer withPrevLayer(Layer layer);
        Layer propagate();
        Layer backProp(IEnumerable<Error> errors);
        IEnumerable<Error> errors(IEnumerable<Error> nextLayerErrors, IEnumerable<Synapse> synapses);
        Layer withNewSet(Layer inputLayer);
	}

}

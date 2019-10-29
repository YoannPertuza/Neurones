using System.Collections.Generic;

namespace Neurones
{
    public interface Layer
	{
		Number outputValue(int targetNeurone);
        Layer withPrevLayer(Layer layer);
        Layer propagate();
        Layer backProp(IEnumerable<Error> errors, IEnumerable<Synapse> synapses);

		Layer applyCorrections();

        Layer withNewSet(Layer inputLayer);
	}

}

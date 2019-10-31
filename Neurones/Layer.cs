using System.Collections.Generic;

namespace Neurones
{
    public interface Layer
	{
		Number outputValue(int targetNeurone);
        
        Layer propagate();
        Layer backProp(IEnumerable<Error> errors, IEnumerable<Synapse> synapses);

		Layer applyCorrections();

        Layer withNewSet(Layer inputLayer);

		Layer withPrevLayer(Layer layer, int index);

		int index();

		Neurone neuroneInLayer(int indexLayer, int indexNeurone);
	}

}

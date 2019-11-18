using System.Collections.Generic;

namespace Neurones
{
    public interface Neurone
    {
        Number outputValue(Layer prevLayer);
        bool find(int targetNeurone);

		Neurone withNewSynapses(IEnumerable<ExitError> errors, Layer prevLayer, Layer nextLayer);

		Neurone withValue(Layer prevLayer);

		Synapse synapseFrom(int indexPreviousNeurone);

		Number deriveRespectToWeight(IEnumerable<ExitError> errors, Layer nextLayer, int indexNeuroneFrom);
	}

}

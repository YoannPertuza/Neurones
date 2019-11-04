using System.Collections.Generic;

namespace Neurones
{
    public interface Neurone
    {
        Number outputValue(Layer prevLayer);
        double val();
        bool find(int targetNeurone);

		IEnumerable<Synapse> synapsesFrom();

		Neurone withError(IEnumerable<Error> nextErrors, Layer prevLayer);

		Neurone withValue(Layer prevLayer);

		Synapse synapseFrom(int indexPreviousNeurone);

		Error lastError();
	}

}

using System.Collections.Generic;

namespace Neurones
{
    public interface Neurone
    {
        Number outputValue(Layer prevLayer);
        double val();
        bool find(int targetNeurone);

		IEnumerable<Synapse> synapsesFrom();

        Error error(IEnumerable<Error> nextErrors, IEnumerable<Synapse> synapses);

		Neurone withError(IEnumerable<Error> nextErrors, Layer prevLayer);

		Neurone withValue(Layer prevLayer);

		Neurone applyCorrections(Layer prevLayer);

		Synapse synapseFrom(int indexPreviousNeurone);

		Error lastError();
	}

}

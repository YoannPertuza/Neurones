using System.Collections.Generic;

namespace Neurones
{
    public interface Neurone
    {
        Number outputValue(Layer prevLayer);
        double val();
        bool find(int targetNeurone);

		IEnumerable<Synapse> synapsesFrom();

		Neurone withError(IEnumerable<ExitError> errors, Layer prevLayer, Layer nextLayer);

		Neurone withValue(Layer prevLayer);

		Synapse synapseFrom(int indexPreviousNeurone);

		Number deriveRespectToWeight(IEnumerable<ExitError> errors, Layer nextLayer, int indexNeuroneFrom);
	}

}

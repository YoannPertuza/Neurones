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
        Neurone withNewSynapse(IEnumerable<Error> nextErrors, Layer prev);
        Neurone withValue(Layer prevLayer);
    }

}

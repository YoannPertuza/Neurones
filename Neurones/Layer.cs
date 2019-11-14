using System.Collections.Generic;

namespace Neurones
{
    public interface Layer
	{
		int index();
		Number neuroneValue(int targetNeurone);
		Neurone neuroneInLayer(int indexLayer, int indexNeurone);

		Layer propagate();
        Layer backProp(IEnumerable<ExitError> errors);

        Layer withNewSet(Layer inputLayer);

		Layer withPrevLayer(Layer layer, int index);

		Layer withNextLayer(Layer layer);

		IEnumerable<Layer> layerListFromLast();

		IEnumerable<Layer> layerListFromFirst();

		Number deriveRespectToOut(IEnumerable<ExitError> errors, Layer nextLayer, int indexNeuroneFrom);


	}

}

using System.Collections.Generic;

namespace Neurones
{
    public interface Layer
	{
		Number neuroneValue(int targetNeurone);
        
        Layer propagate();
        Layer backProp(IEnumerable<Error> errors);

        Layer withNewSet(Layer inputLayer);

		Layer withPrevLayer(Layer layer, int index);

		int index();

		Neurone neuroneInLayer(int indexLayer, int indexNeurone);
	}

}

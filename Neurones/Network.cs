using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class Network
    {
        public Network(Layer lastLayer, IEnumerable<Error> expectedOutput)
        {
            this.lastLayer = lastLayer;
            this.expectedOutput = expectedOutput;
        }

        private Layer lastLayer;
        private IEnumerable<Error> expectedOutput;

        public IEnumerable<Error> errors()
        {
            return this.expectedOutput.Select(o => 
                new ExitError(
                    o.neuroneIndex(),
                    new Substr(o.asNumber(), lastLayer.outputValue(o.neuroneIndex()))
                )
            );
        }
    }

}

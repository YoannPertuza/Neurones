using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
	public class BackPropagation : Propagation
	{
		public BackPropagation(Propagation forwardPropagation, IEnumerable<ExitError> errors)
		{
			this.forwardPropagation = forwardPropagation;
			this.errors = errors;
		}

		private Propagation forwardPropagation;
		private IEnumerable<ExitError> errors;

		public Layer execute()
		{
			var forward = this.forwardPropagation.execute();

			return new LinkedNextLayer(forward.toListFromLast().Reverse()).link().firstLayer().backProp(this.errors);
		}
	}

}

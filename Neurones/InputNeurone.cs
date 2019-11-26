using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Neurones
{
    public class InputNeurone : Neurone, IDisposable
	{
        public InputNeurone(int index, double value) : this(index, new DefaultNumber(value), new List<ExitError>())
		{
		}

	
		public InputNeurone(int index, Number value, IEnumerable<ExitError> errors)
		{
			this.index = index;
			this.value = value;
			this.errors = errors;
		}

		private int index;
        private Number value;
		private IEnumerable<ExitError> errors;
		bool disposed = false;
		// Instantiate a SafeHandle instance.
		SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
		public Number outputValue(Layer prevLayer)
        {
            return value;
        }

        public bool find(int targetNeurone)
        {
            return targetNeurone == this.index;
        }

        public Neurone withValue(Layer layer)
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }


		public Neurone withNewSynapses(IEnumerable<ExitError> nextErrors, Layer prevLayer, Layer nextLayer)
		{
			return new InputNeurone(
				this.index,
				this.value,
				new List<ExitError>() {  }
			);
		}

		public Synapse synapseFrom(int indexPreviousNeurone)
		{
			throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
		}

		public Number deriveRespectToWeight(IEnumerable<ExitError> errors, Layer nextLayer, int indexNeuroneFrom)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				handle.Dispose();
			}

			disposed = true;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("[");
			sb.Append($"{this.value.value()}");
			sb.Append("]\r\n");

			return sb.ToString();
		}
	}

}

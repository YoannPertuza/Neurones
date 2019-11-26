using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Neurones
{
    public class NullLayer : Layer, IDisposable
	{

		bool disposed = false;
		// Instantiate a SafeHandle instance.
		SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

		public Number neuroneValue(int targetNeurone) {
            throw new Exception("YOU HAVE TO LINK YOUR LAYERS");
        }

        public Layer withPrevLayer(Layer layer, int index)
        {
            throw new Exception("CANNOT LINK NULL LAYER");
        }

        public Layer backProp(IEnumerable<ExitError> errors)
        {
            throw new Exception("CANNOT LINK NULL LAYER");
        }

        public Layer propagate()
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }

        public Layer withNewSet(Layer inputLayer)
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }

        public Layer backProp(IEnumerable<ExitError> errors, IEnumerable<Synapse> synapses)
        {
            throw new NotImplementedException();
        }

		public Layer applyCorrections()
		{
			throw new NotImplementedException();
		}

		public int index()
		{
			throw new NotImplementedException();
		}

		public Neurone neuroneInLayer(int indexLayer, int indexNeurone)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Layer> toListFromLast()
		{
			throw new NotImplementedException();
		}

		public Layer withNextLayer(Layer layer)
		{
			throw new NotImplementedException();
		}

	
		public Number deriveRespectToOut(IEnumerable<ExitError> errors, Layer nextLayer, int indexNeuroneFrom)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Layer> toListFromFirst()
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
	}

}

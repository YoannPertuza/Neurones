using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace Neurones
{
	public class ForwardPropagation : Propagation, IDisposable
	{
		public ForwardPropagation(Layer inputLayer, params Layer[] layers)
		{
			this.linkedLayers = new LinkedPrevLayer(inputLayer, layers);
		}

		private LinkedPrevLayer linkedLayers;
		bool disposed = false;
		// Instantiate a SafeHandle instance.
		SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

		public Layer execute()
		{
			return linkedLayers.link().lastLayer().propagate();
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

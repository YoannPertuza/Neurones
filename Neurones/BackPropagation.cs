using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Neurones
{
	public class BackPropagation : Propagation, IDisposable
	{
		public BackPropagation(Propagation forwardPropagation, IEnumerable<ExitError> errors)
		{
			this.forwardPropagation = forwardPropagation;
			this.errors = errors;
		}

		private Propagation forwardPropagation;
		private IEnumerable<ExitError> errors;

		bool disposed = false;
		// Instantiate a SafeHandle instance.
		SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

		public Layer execute()
		{
			var forward = this.forwardPropagation.execute();

			return new LinkedNextLayer(forward.toListFromLast().Reverse()).link().firstLayer().backProp(this.errors);
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

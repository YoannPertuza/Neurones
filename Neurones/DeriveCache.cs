using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Neurones
{
	public class DeriveCache : IDisposable
	{
		public DeriveCache()
		{
			this.cache = new Dictionary<int, Number>();
		}

		private Dictionary<int, Number> cache;
		bool disposed = false;
		// Instantiate a SafeHandle instance.
		SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

		public Number get(int indexNeuroneFrom, Func<Number> derive)
		{
			if (!cache.ContainsKey(indexNeuroneFrom))
			{
				this.cache.Add(indexNeuroneFrom, derive());			
			}
			return cache[indexNeuroneFrom];
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

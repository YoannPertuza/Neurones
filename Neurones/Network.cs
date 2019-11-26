using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Neurones
{
	public class Network : IDisposable
	{
        public Network(IEnumerable<Layer> layers)
        {
            this.layers = layers;
        }

        private IEnumerable<Layer> layers;
		bool disposed = false;
		// Instantiate a SafeHandle instance.
		SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

		public Network train(IEnumerable<TrainingValue> trainingValues)
		{	
			if (trainingValues.Any())
			{							
				return 
					new Network(
						new BackPropagation(
							new ForwardPropagation(
								trainingValues.First().InputLayer,
								this.layers.ToArray()
							), trainingValues.First().Errors
						).execute().toListFromFirst().Reverse().Skip(1)
					).train(trainingValues.Skip(1));
			} else
			{
				return new Network(this.layers);
			}									
		}

		public Layer predict(Layer inputLayer)
		{
			return new LinkedPrevLayer(inputLayer, this.layers.ToArray()).link().lastLayer().propagate();
		}

		public Layer layer()
		{
			return new LinkedPrevLayer(new InputLayer(), this.layers.ToArray()).link().lastLayer();
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

	public class TrainingValue
	{
		public TrainingValue(Layer inputLayer, IEnumerable<ExitError> errors)
		{
			this.InputLayer = inputLayer;
			this.Errors = errors;
		}

		public Layer InputLayer;
		public IEnumerable<ExitError> Errors;
	}

}

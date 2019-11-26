using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Neurones
{
	public class DeepLayer : Layer, IDisposable
	{
        public DeepLayer(params Neurone[] neurones) : this(new NullLayer(), new NullLayer(), neurones)
		{
		}

		public DeepLayer(Layer prevLayer, params Neurone[] neurones) : this(0, prevLayer, new NullLayer(), neurones)
		{
		}

		public DeepLayer(Layer prevLayer, Layer nextLayer, params Neurone[] neurones) : this(0, prevLayer, nextLayer, neurones)
		{
		}

		public DeepLayer(int indexLayer, Layer prevLayer, Layer nextLayer, params Neurone[] neurones)
		{
			this.indexLayer = indexLayer;
			this.prevLayer = prevLayer;
			this.nextLayer = nextLayer;
			this.neurones = neurones;
		}

		private int indexLayer;
		private IEnumerable<Neurone> neurones;
		private Layer prevLayer;
		private Layer nextLayer;

		bool disposed = false;
		// Instantiate a SafeHandle instance.
		SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

		public Number neuroneValue(int originNeurone)
		{
            return neurones.ToList().Find(n => n.find(originNeurone)).outputValue(this.prevLayer);				
		}

        public Layer withPrevLayer(Layer layer, int index)
        {
            return new DeepLayer(index, layer, new NullLayer(), this.neurones.ToArray());
        }

		public Layer withNextLayer(Layer layer)
		{
			return new DeepLayer(this.indexLayer, this.prevLayer, layer, this.neurones.ToArray());
		}

		public Layer propagate()
        {
            return 
                new DeepLayer(
					this.indexLayer,
					prevLayer.propagate(),
					new NullLayer(),
					this.neurones.Select(n => n.withValue(this.prevLayer)).ToArray()
                );
        }

        public Layer backProp(IEnumerable<ExitError> errors)
        {            
			return
				new DeepLayer(
					this.indexLayer,
					this.prevLayer,
					nextLayer.backProp(errors),
					this.neurones.Select(n => n.withNewSynapses(errors, this.prevLayer, this.nextLayer)).ToArray()
				);
		}

        public Layer withNewSet(Layer inputLayer)
        {
            return new DeepLayer(
				this.indexLayer, 
				this.prevLayer.withNewSet(inputLayer),
				new NullLayer(),
				this.neurones.ToArray()
				);
        }

		public int index()
		{
			return this.indexLayer;
		}

		public Neurone neuroneInLayer(int indexLayer, int indexNeurone)
		{
			if (this.indexLayer == indexLayer)
			{
				return neurones.ToList().Find(n => n.find(indexNeurone));
			} else
			{
				return this.prevLayer.neuroneInLayer(indexLayer, indexNeurone);
			}
		}

		public IEnumerable<Layer> toListFromLast()
		{
			return new List<Layer>(prevLayer.toListFromLast()) { this };
		}

		
		public Number deriveRespectToOut(IEnumerable<ExitError> errors, Layer nextLayer, int indexNeuroneFrom)
		{
			return new Add(
				this.neurones.Select(n => n.deriveRespectToWeight(errors, nextLayer, indexNeuroneFrom)).ToArray()
			);
		}

		public IEnumerable<Layer> toListFromFirst()
		{
			return new List<Layer>(nextLayer.toListFromFirst()) { this };
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
			sb.Append("--- DEEP LAYER --- \r\n");
			sb.Append(String.Join(string.Empty, this.neurones.Select(n => n.ToString()).ToArray()));

			sb.Append(this.prevLayer.ToString());

			return sb.ToString();
		}
	}

}

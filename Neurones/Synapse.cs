using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Neurones
{
    public class Synapse : IDisposable
	{
       
		

		public Synapse(int originNeurone, int destinNeurone, double weight) : this(originNeurone, destinNeurone, new DefaultNumber(weight))
		{
		}

		public Synapse(int originNeurone, int destinNeurone, Number weight)
		{
			this.originNeurone = originNeurone;
			this.destinNeurone = destinNeurone;
			this.weight = weight;
		}

		private int originNeurone;
        private int destinNeurone;
		private Number weight;

		bool disposed = false;
		// Instantiate a SafeHandle instance.
		SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

		public bool isFromNeurone(int neuroneIndex)
		{
            return neuroneIndex == this.originNeurone;
		}

		public bool isToNeurone(int neuroneIndex)
		{
			return neuroneIndex == this.destinNeurone;
		}

		public Number value(Number input)
        {
            return new Mult(input, this.weight);
        }

		public Number value(Layer prevLayer)
		{
            return this.value(prevLayer.neuroneValue(this.originNeurone));
		}

		public Number deriveWeight(Number deriveIn, Number deriveOut)
		{
			return new Mult(deriveIn, deriveOut, this.weight);
		}
        
		public Synapse withAdjustedWeight(Number deriveToIn, Number deriveToOut, Layer prevLayer)
		{
			return new Synapse(
					this.originNeurone,
					this.destinNeurone,
					new Substr(
						this.weight, 
						new Mult(
							new DefaultNumber(0.09),
							new Mult(
								deriveToIn,
								deriveToOut,
								prevLayer.neuroneValue(this.originNeurone)
							)
						)
					).value()
				);
		}

		public bool IsWeightEqualsTo(double expectedWeight)
		{
			return expectedWeight == this.weight.value();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("[");
			sb.Append($"from: {this.originNeurone}, to: {this.destinNeurone}, w:{this.weight.value()}");
			sb.Append("]");
			return sb.ToString();	
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

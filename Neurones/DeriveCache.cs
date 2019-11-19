using System;
using System.Collections.Generic;

namespace Neurones
{
	public class DeriveCache
	{
		public DeriveCache()
		{
			this.cache = new Dictionary<int, Number>();
		}

		private Dictionary<int, Number> cache;

		public Number get(int indexNeuroneFrom, Func<Number> derive)
		{
			if (!cache.ContainsKey(indexNeuroneFrom))
			{
				this.cache.Add(indexNeuroneFrom, derive());			
			}
			return cache[indexNeuroneFrom];
		}
	}

}

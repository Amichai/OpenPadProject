using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public class TwoWayDictionary<T1, T2> : Dictionary<T1, T2> {
		public T1 LookupByValue(T2 obj) {
			return reverseLookup[obj];
		}
		Dictionary<T2, T1> reverseLookup = new Dictionary<T2, T1>();
		public new void Add(T1 obj1, T2 obj2) {
			if (!reverseLookup.ContainsValue(obj1)) {
				reverseLookup.Add(obj2, obj1);
				base.Add(obj1, obj2);
			}
		}
	}
}

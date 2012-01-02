using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common	 {
	public class Sequence {
		Func<double,double> del;
		List<double> sequenceValues = new List<double>();
		public Sequence(Func<double,double> del, double x0) {
			this.del = del;
			sequenceValues.Add(del(x0));

		}

		public double GetElementAt(int idx) {
			if (idx < sequenceValues.Count())
				return sequenceValues[idx];
			for (int i = sequenceValues.Count() - 1; i < idx; i++) {
				var a = del(sequenceValues.Last());
				if (a == sequenceValues.Last())
					break;
				else sequenceValues.Add(a);
			}
			return sequenceValues.Last();
		}
	}
}

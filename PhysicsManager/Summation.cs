using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhysicsManager {
	public class Summation {
		public Summation(function method) {
			this.method = method;
		}
		double sum = double.MinValue;
		function method;
		public double GetSum(int startingIndex, int endingIndex) {
			if (sum == double.MinValue) {
				for (int i = startingIndex; i < endingIndex; i++) {
					sum += method(i);
				}
			}
			return sum;
		}
		public delegate double function(int i);
	}

}

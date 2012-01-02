using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	/// <summary>
	/// For finding the zero of a monotonically increasing function
	/// </summary>
	public class FindZero {
		public static double DichotomyMethod(Func<double, double> function, double min, double max, out int counter, double eps = .001) {
			double lowerBound = min,
				upperBound = max;
			counter = 0;
			double range = double.MaxValue;
			double tryIndex = double.MinValue, tryEval = double.MinValue;
			while (Math.Abs(tryEval) > eps && range > 1.0e-9) {
				counter++;
				range = upperBound - lowerBound;
				double maxEval = function(upperBound);
				double minEval = function(lowerBound);
				bool signMax = (maxEval > 0);
				bool signMin = (minEval > 0);
				if (signMax == signMin)
					throw new Exception();
				tryIndex = lowerBound + range / 2;
				tryEval = function(tryIndex);
				bool trySign = tryEval > 0;
				if (trySign == signMax) {
					upperBound = tryIndex;
				} else lowerBound = tryIndex;
			}
			return tryIndex;

		}
	}
}

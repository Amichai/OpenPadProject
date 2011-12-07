using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public static class MathExtensionMethods {
		public static bool WithinRange(this double val1, double val2, double epsilon){
			return Math.Abs(val1 - val2) < epsilon;
		}
	}
}

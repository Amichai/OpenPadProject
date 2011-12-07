using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public class Equations {
		
	}

	public class QuadraticEquation {
		public double A, B, C;

		public QuadraticEquation(double a, double b, double c) {
			this.A = a;
			this.B = b;
			this.C = c;
		}

		public double[] Roots(){
			if (Math.Pow(B, 2) < 4 * A * C) {
				return null;
			}
			double root1 = (-B + Math.Sqrt(Math.Pow(B, 2) - 4 * A * C)) / (2 * A);
			double root2 = (-B - Math.Sqrt(Math.Pow(B, 2) - 4 * A * C)) / (2 * A);
			return new double[2] { root1, root2 };
		}
	}
}

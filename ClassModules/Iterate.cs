using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ClassModules {
	public class Iterate {
		public delegate double functionToOptimize(double x, double x2, double x3, double x4);
		public int startIteration(double x, functionToOptimize f, double x0, double xmin, double xmax, double eps, int maxiter) {
			int i = 0;
			double x2 = x0;
			double x1;
			do {
				x1 = x2;
				Debug.Print(x1.ToString() + " " + xmax.ToString() + " " + xmin.ToString() + " " + i.ToString() + " " + maxiter.ToString());
				if ((x1 >= xmax) || (x1 <= xmin)) {
					return 0;
				}
				if (i > maxiter) {
					return 0;
				}
				//x1 = f(x1);
				i++;
			}
			while (Math.Abs(x2 - x1) >= eps);
			x = x2;
			return 1;
		}
	}
}

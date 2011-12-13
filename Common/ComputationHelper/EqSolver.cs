using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;


namespace Common {
	public static class Solvers {
		public static double NewtonRaphson(this SingleVariableEq function, double initialApprox, int numberOfIterations, SingleVariableEq derivative) {
			double approximation = initialApprox;
			for (int i = 0; i < numberOfIterations; i++) {
				Debug.Print(approximation.ToString());
				Debug.Print(function.Evaluate(approximation).ToString());
				Debug.Print(derivative.Evaluate(approximation).ToString() + "\n");
				approximation = approximation - (function.Evaluate(approximation) / derivative.Evaluate(approximation));
				if (approximation == double.NaN)
					return approximation;
			}
			return approximation;
		}
		public static double NewtonRaphson(this SingleVariableEq function, SingleVariableEq derivative, double approx,
		 double xMin, double xMax, double eps, int maxiter) {
			int i = 0;
			double x1 = approx;
			double x0;
			do {
				x0 = x1;
				if ((x0 >= xMax) || (x0 <= xMin)) return 0;
				if (i > maxiter) return 0;
				x1 = x0 - function.Evaluate(x0) / derivative.Evaluate(x0);
				i++;
				Debug.Print(Math.Abs(x1 - x0).ToString() + " " + eps.ToString());
			}
			while (Math.Abs(x1 - x0) >= eps);
			approx = x1;
			return approx;
		}
	}
}

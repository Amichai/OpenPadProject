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
			 return new SingleVariableEq(i => i - function.Evaluate(i) / derivative.Evaluate(i)
				 ).IterativeSolver(approx, xMin, xMax, eps, maxiter);
		}
		public static double IterativeSolver(this SingleVariableEq function, double approx, double xMin, double xMax,
			double eps, int maxiter) {
			int i = 0;
			double x2 = approx;
			double x1;
			do {
				x1 = x2;
				if (x1 >= xMax || x1 <= xMin) return double.MinValue;
				if (i > maxiter) return double.MinValue;
				x2 = function.Evaluate(x1);
				i++;
			}while (Math.Abs(x2 - x1) >= eps);
			approx = x2;
			return approx;
		}
	}
}

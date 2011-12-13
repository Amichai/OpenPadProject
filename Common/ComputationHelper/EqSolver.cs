using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;


namespace Common {
	public static class Solvers {
		public static double NewtonRalfson(this SingleVariableEq function, double initialApprox, int numberOfIterations, SingleVariableEq derivative) {
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
	}
}

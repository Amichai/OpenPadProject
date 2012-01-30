using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace Common {
	public static class MathExtensionMethods {
		public static bool WithinRange(this double val1, double val2, double epsilon){
			return Math.Abs(val1 - val2) < epsilon;
		}
		public static double Sqrd(this double v1) {
			return Math.Pow(v1, 2.0);
		}


		#region Func<double, double> extension methods
		
		public static Series GetSeriesForGraph(this Func<double, double> function, double start, double end, double dx) {
			Series series = new Series();
			for (double x = start; x < end; x += dx) {
				double y = function(x);
				Debug.Print(x.ToString() + ", " + y.ToString());
				if (!double.IsNaN(y) && y < 1.0e10 && y > -1.0e10) {
					series.Points.Add(new DataPoint(x, y));
				} else Debug.Print(x.ToString() + ", " + y.ToString() + ", " + "could not be graphed");
			}
			series.ChartType = SeriesChartType.Line;	
			series.XAxisType = AxisType.Primary;
			series.YAxisType = AxisType.Primary;

			series.ChartArea = "ChartArea1";
			series.Legend = "Legend1";
			return series;
		}
		
		public static double Derivative(this Func<double, double> function, double at, double epsilon = .01) {
			return (function(at + epsilon / 2) - function(at - epsilon / 2)) / epsilon;
		}

		#region Integration and summation
		public static double simpsonIntegralApproximation(this Func<double, double> function, double lowerBound, double upperBound, int iterations) {
			return (2 * function.rectIntegralApproximation(lowerBound, upperBound, iterations)
				+ function.trapIntegralApproximation(lowerBound, upperBound, iterations)) / 3;
		}

		public static double trapIntegralApproximation(this Func<double, double> function, double lowerBound, double upperBound, int numberOfTraps) {
			double A = ((upperBound - lowerBound) / numberOfTraps);
			double B = function(lowerBound) / 2;
			double C = function(upperBound) / 2;
			if (double.IsNaN(A) || double.IsNaN(B) || double.IsNaN(C)) {
				throw new Exception("NaN exception");
			}
			Func<int, double> argument = (i => lowerBound +
							(i * (upperBound - lowerBound)) / numberOfTraps);
			double sum = function.EvaluateAsASigmaSumFrom(1, numberOfTraps - 1, argument);
			if (double.IsNaN(sum)) throw new Exception("NaN exception");
			return A * (B + sum + C);
		}

		public static double rectIntegralApproximation(this Func<double, double> function, double lowerBound, double upperBound, int numberOfRects) {
			double A = ((upperBound - lowerBound) / numberOfRects);
			Func<int, double> argument = (i => lowerBound +
						((2 * i - 1) * (upperBound - lowerBound) / (2 * numberOfRects)));
			double sum = function.EvaluateAsASigmaSumFrom(1, numberOfRects, argument);
			if (double.IsNaN(A) || double.IsNaN(sum)) throw new Exception("NaN exception");
			return A * sum;
		}

		public static double EvaluateIntegral(this Func<double, double> function, double lowerBound, double upperBound, double accuracyEps = .0002, int maxIter = 1000) {
			int j;
			double s, st, ost = 0.0, os = 0.0;
			for (j = 1; j <= maxIter; j++) {
				st = function.trapIntegralApproximation(lowerBound, upperBound, j);
				s = (4.0 * st - ost) / 3.0;
				if (j > 5)
					if ((Math.Abs(s - os) < accuracyEps * Math.Abs(os)) || (s == 0.0 && os == 0.0)) {
						Debug.Print("%d\n", j);
						return s;
					}
				os = st;
				ost = st;
			}
			throw new Exception("Too many steps in routine\n");
		}

		
		public static double EvaluateAsASigmaSumFrom(this Func<double, double> function, int startIdx, int endIdx, Func<int, double> argument) {
			double sum = 0;
			for (int i = startIdx; i <= endIdx; i++) {
				sum += function(argument(i));
			}
			return sum;
		}
		#endregion
		#region solve for x where f(x) = 0
		/// <summary>
		/// Finds the value x where f(x) = 0
		/// </summary>
		public static double NewtonRaphson(this Func<double, double> function, Func<double, double> derivative, double approx,
		 double xMin, double xMax, double eps, int maxiter) {
			return new Func<double, double>(i => i - function(i) / derivative(i)
				).IterativeSolver(approx, xMin, xMax, eps, maxiter);
		}
		/// <summary>
		/// Finds the value x where f(x) = 0
		/// </summary>
		public static  double NewtonRaphson(this Func<double, double> function, double approx,
		 double xMin, double xMax, double eps, int maxiter) {
			Func<double, double> derivative = i => function.Derivative(i);
			return new Func<double,double>(i => i - function(i) / derivative(i)
				).IterativeSolver(approx, xMin, xMax, eps, maxiter);
		}
		/// <summary>
		/// Finds the value x where f(x) = 0
		/// </summary>
		public static double IterativeSolver(this Func<double, double> function, double approx, double xMin, double xMax,
			double eps, int maxiter, out int counter) {
			int i = 0;
			counter = i;
			double x2 = approx;
			double x1;
			do {
				x1 = x2;
				if (x1 >= xMax || x1 <= xMin) return double.MinValue;
				if (i > maxiter) return double.MinValue;
				x2 = function(x1);
				i++;
			} while (Math.Abs(x2 - x1) >= eps);
			approx = x2;
			counter = i;
			return approx;
		}

		public static double IterativeSolver(this Func<double, double> function, double approx, double xMin, double xMax,
											double eps, int maxiter) {
			int i = 0;
			double x2 = approx;
			double x1;
			do {
				x1 = x2;
				if (x1 >= xMax || x1 <= xMin) return double.MinValue;
				if (i > maxiter) return double.MinValue;
				x2 = function(x1);
				i++;
			} while (Math.Abs(x2 - x1) >= eps);
			approx = x2;
			return approx;
		}
		#endregion
		#endregion
	}
}

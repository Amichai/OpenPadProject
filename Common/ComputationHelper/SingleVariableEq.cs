using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace Common{
	public class SingleVariableEq {
		Func<double, double> function;
		public SingleVariableEq(Func<double, double> func) {
			this.function = func;
		}
		public double Evaluate(double at){
			return this.function(at);
		}

		public double Derivative(double at, double epsilon = .01) {
			return (Evaluate(at + epsilon / 2) - Evaluate(at - epsilon / 2)) / epsilon;
		}
		public void Graph(double start, double end, double dx) {
			Series series = new Series();
			for (double x = start; x < end; x += dx) {
				Debug.Print(x.ToString() + ", " + Evaluate(x).ToString());
				series.Points.Add(new DataPoint(x, Evaluate(x)));
			}
			series.ChartType = SeriesChartType.Line;
			series.XAxisType = AxisType.Primary;
			series.YAxisType = AxisType.Primary;

			series.ChartArea = "ChartArea1";
			series.Legend = "Legend1";
			Graph graph = new Graph(series);
			graph.ShowDialog();
		}
		//Graph, minimize, x-y intercepts, solve

		private double simpsonIntegralApproximation(double lowerBound, double upperBound, int approxConstant) {
			return (2 * rectIntegralApproximation(lowerBound, upperBound, approxConstant)
				+ trapIntegralApproximation(lowerBound, upperBound, approxConstant)) / 3;
		}

		private double trapIntegralApproximation(double lowerBound, double upperBound, int numberOfTraps) {
			double A = ((upperBound - lowerBound) / numberOfTraps);
			double B = Evaluate(lowerBound) / 2;
			double C = Evaluate(upperBound) / 2;
			Func<int, double> argument = (i => lowerBound + 
							(i*(upperBound - lowerBound)) / numberOfTraps);
			double sum = this.EvaluateAsASigmaSumFrom(1, numberOfTraps - 1, argument);
			return A * (B + sum + C);
		}

		private double rectIntegralApproximation(double lowerBound, double upperBound, int numberOfRects) {
			double A = ((upperBound - lowerBound) / numberOfRects);
			Func<int, double> argument = (i => lowerBound + 
						((2*i - 1)*(upperBound - lowerBound) / (2 * numberOfRects)));
			double sum = this.EvaluateAsASigmaSumFrom(1, numberOfRects, argument);
			return A * sum;
		}

		public double EvaluateIntegral(double lowerBound, double upperBound, double accuracyEps = .0002, int maxIter = 1000) {
			int j;
			double s, st, ost = 0.0, os = 0.0;
			for (j = 1; j <= maxIter; j++) {
				st = trapIntegralApproximation(lowerBound, upperBound, j);
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

		public double EvaluateAsASigmaSumFrom(int startIdx, int endIdx, Func<int, double> argument) {
			double sum = 0;
			for (int i = startIdx; i <= endIdx; i++) {
				sum += Evaluate(argument(i));
			}
			return sum;
		}
	}
}

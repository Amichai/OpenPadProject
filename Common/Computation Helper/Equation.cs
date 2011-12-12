using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace Common {
	public class SingleVariableEq {
		Func<double, double> function;
		public SingleVariableEq(Func<double, double> func) {
			this.function = func;
		}
		public double Evaluate(double at){
			return this.function(at);
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

		public double EvaluateIntegral(double lowerBound, double upperBound, double accuracyEps, int maxIter) {
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

	public class MultiVariableEq : Dictionary<string, Func<double>> {
		public double Get(string paramName) {
			return this[paramName]();
		}
		public void SetParameter(string paramName, double val) {
			if (this.ContainsKey(paramName)) {
				this[paramName] = () => val;
			} else this.Add(paramName, () => val);
		}
		/// <summary>
		/// Takes the partial derivative of one parameter with respect to another
		/// </summary>
		public double Partial(string dx, string dy, double at, double epsilon = .01) {
			//this[dy] = () => at + epsilon / 2;
			//double A = this[dx]();			
			//this[dy] = () => at - epsilon / 2;
			//double B = this[dx]();
			//return (A - B) / epsilon;
			if (dependentVariables.Contains(dy))
				throw new Exception("Cannot set a dependent variable");
			return (Evaluate(dy, at + epsilon / 2, dx) - Evaluate(dy, at - epsilon / 2, dx)) / epsilon;

		}
		HashSet<string> dependentVariables = new HashSet<string>();
		public void AddDependentVariable(string paramName, Func<double> del) {
			dependentVariables.Add(paramName);
			this.Add(paramName, del);
		}

		public double Evaluate(string varToSet, double val, string varToObserve) {
			if (dependentVariables.Contains(varToSet))
				throw new Exception("Cannot set a dependent variable");
			this[varToSet] = () => val;
			return this[varToObserve]();
		}

		public double Evaluate(string var1, double val1, string var2, double val2, string varToObserve) {
			this[var1] = () => val1;
			this[var2] = () => val2;
			if (dependentVariables.Contains(var1) || dependentVariables.Contains(var2))
				throw new Exception("Cannot set a dependent variable");
			return this[varToObserve]();
		}

		public SingleVariableEq Relate(string var1, string var2) {
			return new SingleVariableEq(
				i => Evaluate(var1, i, var2)
			);
		}

		public Series TrialData(string xAxis, string yAxis, double xMin, double xMax, double dx) {
			var series = new Series(xAxis + "-" + yAxis);
			var eq = Relate(xAxis, yAxis);
			for (double i = xMin; i <= xMax; i += dx) {
				series.Points.Add(new DataPoint(i, eq.Evaluate(i)));
			}
			series.ChartType = SeriesChartType.Line;
			series.XAxisType = AxisType.Primary;
			series.YAxisType = AxisType.Primary;
			
			series.ChartArea = "ChartArea1";
			series.Legend = "Legend1";
			return series;
		}
	}
}

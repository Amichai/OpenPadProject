using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace Common {
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
			if (dependentVariables.Contains(dy))
				throw new Exception("Cannot set a dependent variable");
			return (Evaluate(dy, at + epsilon / 2, dx) - Evaluate(dy, at - epsilon / 2, dx)) / epsilon;
		}

		HashSet<string> dependentVariables = new HashSet<string>();
		public void AddDependentVariable(string paramName, Func<double> del) {
			dependentVariables.Add(paramName);
			this.Add(paramName, del);
		}

		public void AddEqParameter(string param, double val) {
			if (dependentVariables.Contains(param))
				throw new Exception("Overriding a dependent variable");
			this[param] = () => val;
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

		public Series TrialData(string xAxis, string yAxis, double xMin, double xMax, double dx, string lineName = null) {
			Series series;
			if (lineName == null)
				series = new Series(xAxis + "-" + yAxis);
			else
				series = new Series(lineName);
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

		public Series ParemetricTrial(string xAxis, string yAxis, string paremetricParm, double pMin, double pMax, double dp, string lineName = null) {
			Series series;
			if (lineName == null)
				series = new Series(xAxis + "-" + yAxis);
			else
				series = new Series(lineName);
			var eq1 = Relate(paremetricParm, xAxis);
			var eq2 = Relate(paremetricParm, yAxis);
			for (double i = pMin; i <= pMax; i += dp) {
				series.Points.Add(new DataPoint(eq1.Evaluate(i), eq2.Evaluate(i)));
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

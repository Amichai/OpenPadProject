using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

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

		public double DerivativeAt(double at, double epsilon = .0002) {
			return (Evaluate(at + epsilon / 2) - Evaluate(at - epsilon / 2)) / epsilon;
		}

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
		HashSet<string> parameterNames = new HashSet<string>();
		int numberOfParameters = 0;
		public void IndependentParameters(params string[] parameters) {
			this.numberOfParameters = parameters.Count();
			foreach(string s in parameters)
				this.parameterNames.Add(s);
		}
		public double PartialDerivative(string paramName, double at) {
			SetParameter(paramName, at);
			throw new NotImplementedException();
		}
		public double Get(string paramName) {
			return this[paramName]();
		}
		public void SetParameter(string paramName, double val) {
			if (!parameterNames.Contains(paramName))
				throw new Exception("Undefined variable");
			if (this.ContainsKey(paramName)) {
				this[paramName] = () => val;
			} else this.Add(paramName, () => val);
		}
		/// <summary>
		/// Takes the partial derivative of one parameter with respect to another
		/// </summary>
		public double Partial(string p, string p_2) {
			throw new NotImplementedException();
		}
		public void AddDependentVariable(string paramName, Func<double> del) {
			this.Add(paramName, del);
		}
	}
}

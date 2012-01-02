using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	//Todo: Port to common library
	public class PolynomialEquation {
		public List<double> coefficients = new List<double>();
		/// <summary>Takes the coefficients for a polynomial equation. First parameter 
		/// is for the highest power term.</summary>
		public PolynomialEquation(params double[] coefficients) {
			this.coefficients = coefficients.ToList();
		}
		/// <summary>
		/// Evaluates and returns the derivative using double Horner Scheme
		/// </summary>
		/// <returns>Item1 f(x), item2 is f'(x).</returns>
		public Tuple<double, double> EvaluateAndEvaluateTheDerivative(double at){
			double evalResult = 0;
			double derivResult = 0;
			for (int i = 0; i < coefficients.Count() - 1; i++) {
				evalResult = evalResult * at + coefficients[i];
				derivResult = derivResult * at + evalResult;
			}
			evalResult = evalResult * at + coefficients.Last() ;

			return new Tuple<double, double>(evalResult, derivResult);
		}

		/// <summary>Evaluates the polynomial using http://en.wikipedia.org/wiki/Horner_scheme </summary>
		public double Evaluate(double at) {
			double result = 0;
			foreach(double coef in coefficients){
				result = result * at + coef;
			}
			return result;
		}
	}
}

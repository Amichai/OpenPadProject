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
		const int maxRand = 20;
		Random rand = new Random();

		/// <summary>Evaluates the polynomial using http://en.wikipedia.org/wiki/Horner_scheme </summary>
		public double Evaluate(double at) {
			double result = 0;
			foreach(double coef in coefficients){
				result = result * at + coef;
			}
			return result;
		}

		/// <summary>
		/// Uses Ruffini's rule
		/// </summary>
		/// <param name="root"></param>
		/// <returns></returns>
		public PolynomialEquation EliminateRoot(double root) {
			root = -root;
			List<double> tempList = new List<double>(coefficients.Count);
			List<double> newCoef = new List<double>(coefficients.Count); //last guy is the remainder
			newCoef.Add(coefficients[0]);
			for (int i = 1; i < coefficients.Count; i++) {
				tempList.Add( newCoef.Last()* root);
				newCoef.Add(coefficients[i-1] + newCoef.Last());
			}
			return new PolynomialEquation(newCoef.ToArray());
		}

		public List<double> GetRoots( int maxIter = 100, int maxAttempt = 100, double epsilon = 1.0e-8, double resolution = 1.0e-8) {
			double largestCoef = coefficients.Max();
			var absCoef = coefficients.Select(i => Math.Abs(i));
			var normalizedCoeficients = absCoef.Select(i => i / largestCoef).ToList();
			List<double> newlist = new List<double>();
			for (int i = 0; i < coefficients.Count; i++) {
				newlist.Add(Math.Pow(normalizedCoeficients[i], 1.0/(normalizedCoeficients.Count - i)));
			}
			double upperBound = newlist.Max() * 2;

			Func<double, double> eval = j => EvaluateAndEvaluateTheDerivative(j).Item1;
			var deriv = new SingleVariableEq(j => EvaluateAndEvaluateTheDerivative(j).Item2);
			for (int i = 0; i < coefficients.Count; i++) {
				double root = new SingleVariableEq(eval).NewtonRaphson(deriv, 0.0, -upperBound, upperBound, epsilon, maxIter);

			}
			throw new NotImplementedException();
		}
	}
}

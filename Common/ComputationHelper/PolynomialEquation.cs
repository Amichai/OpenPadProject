using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public class PolynomialEq {
		public List<double> coefficients = new List<double>();

		/// <summary>Takes the coefficients for a polynomial equation. First parameter 
		/// is for the highest power term.</summary>
		public PolynomialEq(params double[] coefficients) {
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

		/// <summary>Uses Ruffini's rule</summary>
		public PolynomialEq EliminateRootNoRemainder(double root) {
			List<double> tempList = new List<double>(coefficients.Count);
			List<double> newCoef = new List<double>(coefficients.Count); //last guy is the remainder
			newCoef.Add(coefficients[0]);
			for (int i = 1; i < coefficients.Count - 1; i++) {
				tempList.Add( newCoef.Last()* root);
				newCoef.Add(coefficients[i] + tempList.Last());
			}
			return new PolynomialEq(newCoef.ToArray());
		}

		public double UpperBound(){
			int n = this.coefficients.Count-1;
			List<double> candidates = new List<double>(n){1};
			for (int i = n; i > 0; i--) {
				candidates.Add(Math.Abs((coefficients[i] / coefficients[0]) * n)); 
			}
			return candidates.Max();
		}

		public List<double> GetRoots( int maxIter = 100, int maxAttempt = 100, double epsilon = 1.0e-8, double resolution = 1.0e-8) {
			double upperBound = UpperBound();
			PolynomialEq equationToSolve = this;
			List<double> roots = new List<double>(coefficients.Count);
			for (int i = 0; i < coefficients.Count - 1; i++) {
				Func<double, double> eval = j => equationToSolve.EvaluateAndEvaluateTheDerivative(j).Item1;
				Func<double, double> deriv = j => equationToSolve.EvaluateAndEvaluateTheDerivative(j).Item2;
				double root = double.MinValue;
				int counter = 0;
				while (root == double.MinValue) {
					double guess = upperBound * rand.NextDouble();
					root = eval.NewtonRaphson(deriv, guess, -upperBound, upperBound, epsilon, maxIter);
					if (++counter > 20)
						throw new Exception("We have not converged yet");
				}
				equationToSolve = EliminateRootNoRemainder(root);
				roots.Add(root);
			}
			return roots;
		}
	}
}

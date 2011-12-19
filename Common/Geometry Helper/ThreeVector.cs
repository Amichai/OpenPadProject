using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public class ThreeVector {
		//TODO: check to see if a multivariableeq exists. If so evaluate that.
		public double ci { get; set; }
		public double cj { get; set; }
		public double ck { get; set; }

		public MultiVariableEq x_t_i { get; set; }
		public MultiVariableEq y_t_j { get; set; }
		public MultiVariableEq z_t_k { get; set; }

		public ThreeVector() { }
		public ThreeVector(MultiVariableEq x, MultiVariableEq y, MultiVariableEq z) {
			this.x_t_i = x;
			this.y_t_j = y;
			this.z_t_k = z;
		}
		public ThreeVector(double xComp, double yComp, double zComp){
			this.ci = xComp;
			this.cj = yComp;
			this.ck = zComp;
		}
		/// <summary>
		/// ThreeVector in terms of unit angles
		/// </summary>
		/// <param name="alpha">angle to the x axis</param>
		/// <param name="beta">angle to the y axis</param>
		/// <param name="gamma">angle to the z axis</param>
		public ThreeVector(Angle alpha, Angle beta, Angle gamma, double magnitude) {
			this.ci = magnitude * Math.Cos(alpha.InRadians());
			this.cj = magnitude * Math.Cos(beta.InRadians());
			this.ck = magnitude * Math.Cos(gamma.InRadians());
		}


		#region unit vectors
		public static ThreeVector NullVector() {
			return new ThreeVector(0, 0, 0);
		}
		public static ThreeVector i_hat() {
			return new ThreeVector(1, 0, 0);
		}
		public static ThreeVector j_hat() {
			return new ThreeVector(0, 1, 0);
		}
		public static ThreeVector k_hat() {
			return new ThreeVector(0, 0, 1);
		}
		#endregion


		public ThreeVector VectorProduct(ThreeVector vec) {
			return new ThreeVector(this.cj * vec.ck - this.ck * vec.cj,
									this.ck * vec.ci - this.ci * vec.ck,
									this.ci * vec.cj - this.cj * vec.ci);
		}

		/// <summary>Dot product overload</summary>
		public static double operator *(ThreeVector a, ThreeVector b) {
			double sum = 0;
			foreach (List<double> comp in a.getAllComponentsForEachDimension(b)) {
				sum += comp.Aggregate((p1, p2) => p1 * p2);
			}
			return sum;
		}

		public static ThreeVector operator *(double a, ThreeVector b) {
			return new ThreeVector(a * b.ci, a * b.cj, a * b.ck);
		}

		private IEnumerable<List<double>> getAllComponentsForEachDimension(ThreeVector b) {
			for (int i = 0; i < 3; i++) {
				List<double> componentsToReturn = new List<double>();
				componentsToReturn.Add(this.ci);
				componentsToReturn.Add(b.ci);
				componentsToReturn.Add(this.cj);
				componentsToReturn.Add(b.cj);
				componentsToReturn.Add(this.ck);
				componentsToReturn.Add(b.ck);
				yield return componentsToReturn;
			}
		}

		public static ThreeVector operator -(ThreeVector a, ThreeVector b) {
			return new ThreeVector(a.ci - b.ci, a.cj - b.cj, a.ck - b.ck);
		}

		public static ThreeVector operator +(ThreeVector a, ThreeVector b) {
			return new ThreeVector(a.ci + b.ci, a.cj + b.cj, a.ck + b.ck);
		}

		public double Magnitude() {
			return Math.Sqrt(ci.Sqrd() + cj.Sqrd() + ck.Sqrd());
		}

		public Angle AngleBetweenVectors(ThreeVector vec) {
			double a = (this * vec) / (this.Magnitude() * vec.Magnitude());
			return new Angle(Math.Acos(a));
		}

		public ThreeVector ReflectOverLineThroughOrigin(ThreeVector lineToReflectOver) {
			return 2 * ((((this * lineToReflectOver) / (lineToReflectOver * lineToReflectOver)) * lineToReflectOver) - this);
		}

		public override string ToString() {
			string output = string.Empty;
			output += "x: " + this.ci.ToString();
			output += "y: " + this.cj.ToString();
			output += "z: " + this.ck.ToString();
			return output;
		}

		public ThreeVector Derivate(string withRespectTo, ThreeVector vec) {
			double x = 0, y = 0, z = 0;
			if(x_t_i.Contains("t", "x"))
				x = x_t_i.Relate("x", "t").Derivative(vec.ci);
			if (y_t_j.Contains("y", "t"))
				y = y_t_j.Relate("t", "y").Derivative(vec.cj);
			if (z_t_k.Contains("t", "z"))
				z = z_t_k.Relate("z", "t").Derivative(vec.ck);
			return new ThreeVector(x, y, z);
		}
		//TODO: Make a component class that can be either a constant or a MultiVariableEq and interpolate seamlessly.
	}
}

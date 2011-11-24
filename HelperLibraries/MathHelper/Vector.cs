using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;

namespace Common {
	/// <summary>For n dimensional space</summary>
	public class Vector {
		public Vector(double x, double y) {
			components.Add(x);
			components.Add(y);
		}

		public Vector(IEnumerable<double> comp) {
			this.components = comp.ToList();
		}

		public Vector(Angle angle, double magnitude) {
			components.Add(magnitude * Math.Cos(angle.InRadians()));
			components.Add(magnitude * Math.Sin(angle.InRadians()));
		}

		public int NumberOfDimensions { get { return components.Count(); } }
		private List<double> components = new List<double>();
		public double GetX() { return components[0]; }
		public double GetY() { return components[1]; }
		public double GetZ() { return components[2]; }

		public Angle Angle() {
			if (NumberOfDimensions != 2)
				throw new Exception("Not ready to handle this yet");
			return new Angle(GetY(), GetX());
		}

		public System.Windows.Point AsWindowsPoint() {
			return new System.Windows.Point(GetX(), GetY());
		}

		public Vector RotateAbout(Vector CenterPoint, Angle angle) {
			double startingX = this.GetX() - CenterPoint.GetX();
			double startingY = this.GetY() - CenterPoint.GetY();
			double xPos = (startingX) * Math.Cos(angle.InRadians()) - (startingY) * Math.Sin(angle.InRadians());
			double yPos = (startingX) * Math.Sin(angle.InRadians()) + (startingY) * Math.Cos(angle.InRadians());
			return new Vector(xPos + CenterPoint.GetX(), yPos + CenterPoint.GetY());
		}

		/// <summary>Multiplication overload</summary>
		public static Vector operator *(double a, Vector b) {
			return new Vector(b.Angle(), b.Magnitude() * a);
		}

		/// <summary>Dot product overload</summary>
		public static double operator *(Vector a, Vector b) {
			if (a.NumberOfDimensions != b.NumberOfDimensions)
				throw new Exception("Inconsistent dimensionalities");
			double sum = 0;
			foreach (List<double> comp in a.GetComponentsForEachDimension(b)) {
				sum += comp.Aggregate((p1, p2) => p1 * p2);
			}
			return sum;
		}

		public IEnumerable<double> GetAllComponents() {
			return components.AsEnumerable();
		}

		public IEnumerable<List<double>> GetComponentsForEachDimension(Vector b) {
			if (this.NumberOfDimensions != b.NumberOfDimensions)
				throw new Exception("Inconsistent dimensionalities");
			for (int i = 0; i < this.NumberOfDimensions; i++) {
				List<double> componentsToReturn = new List<double>();
				if (i == 0) {
					componentsToReturn.Add(this.GetX());
					componentsToReturn.Add(b.GetX());
				}
				if (i == 1) {
					componentsToReturn.Add(this.GetY());
					componentsToReturn.Add(b.GetY());
				}
				if (i == 2) {
					componentsToReturn.Add(this.GetZ());
					componentsToReturn.Add(b.GetZ());
				}
				yield return componentsToReturn;
			}
		}		

		public static Vector operator -(Vector a, Vector b) {
			if (a.NumberOfDimensions != b.NumberOfDimensions)
				throw new Exception("Inconsistent dimensionalities");
			return new Vector(a.GetX() - b.GetX(), a.GetY() - b.GetY());
			
		}

		public double Magnitude() {
			return Math.Sqrt(Math.Pow(GetX(), 2) +
									Math.Pow(GetY(), 2));
		}

		public double XProjection() {
			return Math.Cos(Angle().InRadians()) * Magnitude();
		}
		public double YProjection() {
			return Math.Sin(Angle().InRadians()) * Magnitude();
		}

		public Vector ReflectOverLineThroughOrigin(Vector lineToReflectOver) {
			return 2 * ((((this * lineToReflectOver) / (lineToReflectOver * lineToReflectOver)) * lineToReflectOver) - this);
		}

		public System.Windows.Point ToWindowsPt() {
			if(this.NumberOfDimensions != 2)
				throw new Exception("Too many dimensions");
			return new System.Windows.Point(this.GetX(), this.GetY());
		}

		public override string ToString() {
			string output = string.Empty;
			output += "x: " + this.GetX().ToString();
			output += " y: " + this.GetY().ToString();
			return output;
		}
	}
}

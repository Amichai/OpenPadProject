using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public class TwoVector : ThreeVector{
		public TwoVector(Angle angle, double magnitude) {
			this.ci = magnitude * Math.Cos(angle.InRadians());
			this.cj = magnitude * Math.Sin(angle.InRadians());
			this.ck = 0;
		}
		public TwoVector(double xComp, double yComp) {
			this.ci = xComp;
			this.cj = yComp;
			this.ck = 0;
		}

		public Angle Angle() {
			return new Angle(ci,cj);
		}
			
		public TwoVector RotateAbout(TwoVector CenterPoint, Angle angle) {
			double startingX = this.ci - CenterPoint.ci;
			double startingY = this.cj - CenterPoint.cj;
			double xPos = (startingX) * Math.Cos(angle.InRadians()) - (startingY) * Math.Sin(angle.InRadians());
			double yPos = (startingX) * Math.Sin(angle.InRadians()) + (startingY) * Math.Cos(angle.InRadians());
			return new TwoVector(xPos + CenterPoint.ci, yPos + CenterPoint.cj);
		}

		/// <summary>Scalar multiplication</summary>
		public static TwoVector operator *(double a, TwoVector b) {
			return new TwoVector(b.Angle(), b.Magnitude() * a);
		}

		public double XProjection() {
			return Math.Cos(Angle().InRadians()) * Magnitude();
		}
		public double YProjection() {
			return Math.Sin(Angle().InRadians()) * Magnitude();
		}

		public override string ToString() {
			string output = string.Empty;
			output += "x: " + this.ci.ToString();
			output += "y: " + this.cj.ToString();
			return output;
		}
	}
}

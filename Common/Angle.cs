using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public class Angle {
		public Angle(List<double> components) {
			throw new NotImplementedException();
		}

		public string ToString() {
			return this.InDegrees().ToString();
		}
		public Angle(double rise, double run) {
			//First Quadrant
			if (Math.Sign(rise) == 1 && Math.Sign(run) == 1) {
				this.theta = Math.Atan(rise / run);
			}
			//Second Quadrant
			if (Math.Sign(rise) == 1 && Math.Sign(run) == -1) {
				this.theta = Math.PI + Math.Atan(rise / run);
			}
			//Third Quadrant
			if (Math.Sign(rise) == -1 && Math.Sign(run) == -1) {
				this.theta = Math.PI + Math.Atan(rise / run);
			}
			//Fourth Quadrant
			if (Math.Sign(rise) == -1 && Math.Sign(run) == 1) {
				this.theta = Math.Atan(rise / run);
			}
			//TODO: This may be the bug for the ellipse collision detection problem
		}

		public static Angle operator -(Angle an1, Angle an2){
			return new Angle(an1.theta - an2.theta);
		}
		public static Angle operator +(Angle an1, Angle an2){
			return new Angle(an1.theta + an2.theta);
		}
		public static Angle operator -(Angle an1, double an2) {
			return new Angle(an1.InDegrees() - an2, true);
		}
		public static Angle operator -(double an1, Angle an2) {
			return new Angle( an1-an2.InDegrees(), true);
		}
		public static Angle operator +(Angle an1, double an2) {
			return new Angle(an1.InDegrees() + an2, true);
		}
		public static Angle operator *(Angle an1, double ans2) {
			return new Angle(an1.theta* ans2);
		}

		public Angle(double theta) {
			this.theta = theta;
		}
		public Angle(double theta, bool inDegrees) {
			if (inDegrees)
				this.theta = theta / _180OverPi;
			else this.theta = theta;
		}
		public Angle Adjust(Angle ang) {
			return new Angle(theta + ang.InRadians());
		}
		double theta;
		public double InDegrees() {
			return theta * _180OverPi;
		}
		public double InRadians() { return theta; }
		static double _180OverPi = 57.2957795131;

		public Angle Negate() {
			return new Angle(-theta);
		}
	}
}

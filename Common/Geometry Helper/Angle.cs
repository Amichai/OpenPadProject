using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public class Angle {
		public Angle(List<double> components) {
			throw new NotImplementedException();
		}

		public override string ToString() {
			return this.InDegrees().ToString();
		}

		public static int GetQuadrant(double angle) {
			while (angle >= Math.PI * 2)
				angle -= Math.PI * 2;
			while( angle< 0 )
				angle += Math.PI * 2;
			if (angle >= 0 && angle <= Math.PI / 2)
				return 1;
			if (angle > Math.PI / 2 && angle <= Math.PI)
				return 2;
			if (angle > Math.PI / 2 && angle <= 3* Math.PI / 2)
				return 3;
			if (angle >3 * Math.PI / 2 && angle <= 2*Math.PI)
				return 4;
			throw new Exception();
		}

		public static int GetQuadrant(System.Drawing.Point a, System.Drawing.Point b) {
			if (b.X - a.X >= 0 && b.Y - a.Y >= 0)
				return 1;
			if (b.X - a.X <= 0 && b.Y - a.Y >= 0)
				return 2;
			if (b.X - a.X <= 0 && b.Y - a.Y <= 0)
				return 3;
			if (b.X - a.X >= 0 && b.Y - a.Y <= 0)
				return 4;
			throw new Exception();
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

		public static bool operator <(Angle an1, Angle an2) {
			if (an1.theta < an2.theta)
				return true;
			else return false;
		}
		public static bool operator >(Angle an1, Angle an2) {
			if (an1.theta > an2.theta)
				return true;
			else return false;
		}
		public static Angle operator -(Angle an1, Angle an2){
			return new Angle(an1.theta - an2.theta);
		}
		public static Angle operator -(Angle an1) {
			return an1.Negate();
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

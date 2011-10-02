using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ClassModules {
	public class EntryPoint{
		public static void Main() {
			var t1 = new ProjectileLaunch(10, 9.8).FindAngleToTarget(new Position(2,2)).GetTrajectory();
			var t2 = new ProjectileLaunch(10, new Angle(45, true)).GetTrajectory();
			var t3 = t2.AdjustAngleBy(new Angle(1, true));
			//var maximize = new MaximizationIterator(0, 2 * Math.PI);
		}
	}

	public class Trajectory {
		private double gamma;
		private double func(double x, double xi0, double B, double A) {
			double Bs = Math.Sqrt(1 + x * x) * B;
			if (Bs >= 1) return double.MaxValue;
			else return xi0 - A * (Math.Log(1 - Bs) + Bs);
		}

		public Trajectory(double g, Vector velocity, double gamma, double x, double y, double eps, int maxiter, double tmax, double dt) {
			this.Velocity = velocity;
			this.g = g;
			this.gamma = gamma;

			double B = gamma * x / velocity.Magnitude();
			double A = g / (gamma * gamma * x);
			if (B >= 1)
				throw new Exception("No solutions");
			else {
				double xi = 0;
				double Bs, xib = Math.Sqrt(1 / (B * B) - 1);
				double xi1 = 0;
				Iterate iterate = new Iterate();
				Iterate.functionToOptimize f1 = new Iterate.functionToOptimize(func);
				int numberOfSolutions = iterate.startIteration(xi1, f1, xi, -xib, xib, eps, maxiter);
				double xi0;
				xi0 = y / x;
				if (numberOfSolutions == 1) {
					double vx0 = velocity.Magnitude() * Math.Cos(Math.Atan(xi1));
					double vy0 = velocity.Magnitude() * Math.Sin(Math.Atan(xi1));
					double t = 0;
					while (t < tmax) {
						double xt = vx0 * (1 - Math.Exp(-t * gamma)) / gamma;
						double yt = g * t / gamma + (g / gamma + vy0) * ((1 - Math.Exp(-t * gamma)) / gamma);
						if (xt > x) break;
						t += dt;
					}
				}

			}
				
		}
		public Trajectory(double g, Vector velocity) {
			this.Velocity = velocity;
			this.g = g;
		}
		public Trajectory AdjustAngleBy(Angle angle) {
			return new Trajectory(g, new Vector(GetLaunchAngle().Adjust(angle), GetMagnitude()));
		}
		private double g;
		public Angle GetLaunchAngle(){ return Velocity.Angle(); }
		public double GetMagnitude() { return Velocity.Magnitude();  }
		Vector Velocity;
		public Position PositionOfProjectileGivenX(double xVal) {
			double time = xVal / Velocity.XProjection();
			double yVal = .5 * g * Math.Pow(time, 2);
			return new Position(xVal, yVal);
		}
		public Position PositionOfProjectileGivenY(double yVal) {
			double time = Math.Sqrt((2 * yVal) / g);
			double xVal = Velocity.XProjection() * time;
			return new Position(xVal, yVal);
		}
		public Position PositionAtGivenTime(double time) {
			double yVal = .5 * g * Math.Pow(time, 2);
			double xVal = Velocity.XProjection() * time;
			return new Position(xVal, yVal);
		}
		public double TimeOfFlight() {
			return 2 * Velocity.YProjection() / g;
		}
		public double YMax() {
			double time = TimeOfFlight() /2;
			return PositionAtGivenTime(time).yPos;
		}
		public double XMax() {
			return PositionAtGivenTime(TimeOfFlight()).xPos;
		}
	}

	public class ProjectileLaunch {
		Angles launchAngles = null;
		public ProjectileLaunch(double _v0, Angle theta, double _g) {
			v0 = _v0;
			launchAngles = new Angles(theta);
			g = _g;
		}
		public ProjectileLaunch(double _v0, Angle theta) {
			v0 = _v0;
			launchAngles = new Angles(theta);
		}
		public ProjectileLaunch(double _v0, double _g) {
			v0 = _v0;
			g = _g;
		}
		public ProjectileLaunch(double _v0) {
			v0 = _v0;
		}
		double v0, g = 9.8;
		public ProjectileLaunch FindAngleToTarget(Position t) {
			double A, B, D;
			A = (g * t.xPos) / (v0 * v0);
			B = (g * t.yPos) / (v0 * v0);
			D = 1 - 2 * B - A * A;
			if (D < 0) {
				launchAngles = new Angles("No solutions");
			}
			D = Math.Sqrt(D);
			Angle theta1 = new Angle(Math.Atan((1 - D) / A));
			Angle theta2 = new Angle(Math.Atan((1 + D) / A));
			launchAngles = new Angles(theta1, theta2);
			return this;
		}
		public Trajectory GetTrajectory() {
			return new Trajectory(g, new Vector(launchAngles.GetSingleAngle(), v0));
		}
	}
	
	public class Position {
		public Position(double x, double y){
			this.xPos = x;
			this.yPos = y;
		}
		public double xPos;
		public double yPos;

	}
	
	public class Angles {
		public Angle GetSingleAngle() {
			if (angles.Count < 1)
				return null;
			else
				return angles.First();
		}
		private List<Angle> angles = new List<Angle>();
		public Angles(Angle theta1, Angle theta2){
			angles.Add(theta1);
			angles.Add(theta2);
		}
		public Angles(Angle theta1){
			angles.Add(theta1);
		}
		string errorMsg;
		public Angles(string errorMessage) {
			this.errorMsg = errorMessage;
		}
	}

	public class MaximizationIterator{
		class Pointer {
			public Pointer(double loc, double val) {
				this.PointerLocation = loc;
				this.PointerReturnVal = val;
			}
			public double PointerLocation;
			public double PointerReturnVal;
		}

		double minVal, maxVal;
		public MaximizationIterator(double min, double max) {
			this.maxVal = max;
			this.minVal = min;
		}
		bool MaxFound = false;
		double currentLocation;
		double currentReturnVal;
		Pointer bestLocationPointer;
		static int counter = 0;
		public double NextValToTry() {
			counter++;
			if (counter > 2) {
				currentLocation = (currentLocation + bestLocationPointer.PointerLocation) / 2;
				return currentLocation;
			} else if (counter == 1) {
				return minVal;
			} else if (counter == 2) {
				return maxVal;
			}
			return double.MinValue;
		}
		public void TriedValueReturned(double val) {
			currentReturnVal = val;
			if (currentReturnVal > bestLocationPointer.PointerReturnVal) {
				bestLocationPointer.PointerLocation = currentLocation;
				bestLocationPointer.PointerReturnVal = currentReturnVal;
			}
		}
	}

}


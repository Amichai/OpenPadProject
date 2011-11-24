using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Common {
	public class LineSegment {
		private Angle angle;
		private double magnitude;
		public Vector StartingPos, EndingPos;
		private List<double> components = new List<double>();
		public int NumberOfDimensions = 2;
		private double slope;
		public LineSegment(double magnitude, Angle angle) {
			this.magnitude = magnitude;
			this.angle = angle;
		}
		public LineSegment(double magnitude, Angle angle, Vector midpoint) {
			this.magnitude = magnitude;
			this.angle = angle;
			this.StartingPos = new Vector( midpoint.GetX() - .5*magnitude *Math.Cos(angle.InRadians()),
								midpoint.GetY() - .5*magnitude * Math.Sin(angle.InRadians()));
			this.EndingPos = new Vector(midpoint.GetX() + .5 * magnitude * Math.Cos(angle.InRadians()),
								midpoint.GetY() + .5 * magnitude * Math.Sin(angle.InRadians()));
			components.Add(EndingPos.GetX() - StartingPos.GetX());
			components.Add(EndingPos.GetY() - StartingPos.GetY());
		}
		public double Magnitude() {
			return Math.Sqrt(Math.Pow(XComponent(), 2) +
									Math.Pow(YComponent(), 2));
		}
		public Angle Angle(){
			if (NumberOfDimensions != 2)
				throw new Exception("Not ready to handle this yet");
			return new Angle(YComponent(), XComponent()); 
		}
		public double Slope() {
			if (NumberOfDimensions != 2)
				throw new Exception("Not ready to handle this yet");
			return YComponent() / XComponent();
		}
		public double XComponent() {
			return components[0];
		}
		public double YComponent() {
			return components[1];
		}
		public double ZComponent() {
			return components[2];
		}
		public LineSegment(Vector start, Vector end) {
			if (start.NumberOfDimensions != end.NumberOfDimensions)
				throw new Exception("Inconsistent dimensionalities");
			this.NumberOfDimensions = start.NumberOfDimensions;
			this.StartingPos = start;
			this.EndingPos = end;
			components.Add(EndingPos.GetX() - StartingPos.GetX());
			components.Add(EndingPos.GetY() - StartingPos.GetY());
			this.angle = Angle();
			this.slope = Slope();
			this.magnitude = Magnitude();
		}
		public LineSegment(Vector start, Angle angle, double length) {
			this.StartingPos = start;
			this.magnitude = length;
			this.angle = angle;
			components.Add(magnitude * Math.Cos(angle.InRadians())); 
			components.Add(magnitude * Math.Sin(angle.InRadians()));
			this.EndingPos = new Vector(StartingPos.GetX() + XComponent(),
										StartingPos.GetY() + YComponent());
			this.slope = Slope();
		}
		public LineGeometry AsLineGeometry() {
			return new LineGeometry(StartingPos.AsWindowsPoint(), EndingPos.AsWindowsPoint());
		}
		public LineSegment GenerateOrthogonalAtStartingPoint() {
			Angle nintyDegrees = new Angle(Math.PI / 2);
			return new LineSegment(this.Magnitude(), this.Angle().Adjust(nintyDegrees), this.StartingPos);
		}
		public IEnumerable<double> GetAllComponents() {
			return components.AsEnumerable();
		}
		public Vector TranlsateToVector() {
			return new Vector(GetAllComponents());
		}
		public Angle AngleBetweenPoints(Vector CenterPoint) {
			Vector endPointToCenter = new LineSegment(EndingPos, CenterPoint).TranlsateToVector();
			Vector incomingVector = new LineSegment(EndingPos, StartingPos).TranlsateToVector();
			if ((endPointToCenter.Magnitude() * incomingVector.Magnitude() == 0))
				throw new DivideByZeroException();
			double cosineOfTheAngle = (incomingVector * endPointToCenter) / (endPointToCenter.Magnitude() * incomingVector.Magnitude());
			if (cosineOfTheAngle > 1 || cosineOfTheAngle < -1)
				throw new Exception("Out of range of  acos");
			Angle angleToReturn = new Angle(Math.Acos(cosineOfTheAngle));
			return angleToReturn;
		}

		public System.Windows.Rect AsSystemRect() {
			return new System.Windows.Rect(EndingPos.GetX(), EndingPos.GetY(), .001, .001);
		}

		public LineSegment Flip() {
			return new LineSegment(this.EndingPos, this.StartingPos);
		}

		public string ToString() {
			return StartingPos.ToString() + " " + EndingPos.ToString();
		}

		public LineSegment reflectOverHorizontalMidLine(int boardHeight) {
			Vector start = new Vector(this.StartingPos.GetX(), boardHeight - this.StartingPos.GetY());
			Vector end = new Vector(this.EndingPos.GetX(), boardHeight - this.EndingPos.GetY());
			return new LineSegment(start, end);
		}
	}
}

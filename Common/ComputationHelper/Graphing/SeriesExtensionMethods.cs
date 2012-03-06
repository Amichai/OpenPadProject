using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace Common{
	public static class SeriesExtensionMethods {
		public static Series TransfomX(this Series ser, Func<double, double> transform) {
			var ser2 = new Series();
			for (int i = 0; i < ser.Points.Count(); i++) {
				ser2.Points.AddXY(transform(ser.Points[i].XValue), ser.Points[i].YValues.First());
			}
			return ser2;
		}
		public static Series TransfomY(this Series ser, Func<double, double> transformY) {
			var ser2 = new Series();
			for (int i = 0; i < ser.Points.Count(); i++) {
				ser2.Points.AddXY(ser.Points[i].XValue, transformY(ser.Points[i].YValues.First()));
			}
			return ser2;
		}
		public static Series TransfomXY(this Series ser, Func<double, double> transformX) {
			var ser2 = new Series();
			for (int i = 0; i < ser.Points.Count(); i++) {
				ser2.Points.AddXY(transformX(ser.Points[i].XValue), transformX(ser.Points[i].YValues.First()));
			}
			return ser2;
		}
		public static Series TransfomXY(this Series ser, Func<double, double> transformX, Func<double, double> transformY) {
			var ser2 = new Series();
			for (int i = 0; i < ser.Points.Count(); i++) {
				ser2.Points.AddXY(transformX(ser.Points[i].XValue), transformY(ser.Points[i].YValues.First()));
			}
			return ser2;
		}

		public static Series SortByXVals(this Series ser) {
			var ser2 = new Series();
			throw new NotImplementedException();
		}

		public static Series ListToRankSeries(this List<double> list) {
			var ser = new Series();
			list.Sort();
			list.Reverse();
			for (int i = 0; i < list.Count(); i++) {
				ser.Points.AddXY(i + 1, list[i]);
			}
			return ser;
		}
	}
}

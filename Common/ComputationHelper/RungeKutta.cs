using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace Common {
	//y'[n]  = f_i(x, y[n]) i => [0,n]
	public class RungeKutta {
		public RungeKutta() { }
		public RungeKutta(updateYVals deriv) {
			this.update = deriv;
		}
		updateYVals update;
		public delegate double[] updateYVals(double[] y, double x, double h , int n);
		double[] k1, k2, k3, k4;

		public double[] CurrentYs;

		public void UpdateYVals(double x,double[] y, double h, int n) {
			k1 = new double[n];
			k2 = new double[n];
			k3 = new double[n];
			k4 = new double[n];

			k1 = update(y, x, h, n);
			for (int i = 0; i < n; i++) {
				k1[i] *= h;
				k4[i] = y[i] + k1[i] * .5;
			}
			k2 = update(k4, x + h * .5, h, n);
			for (int i = 0; i < n; i++) {
				k2[i] *= h;
				k4[i] = y[i] + k2[i] * .5;
			}
			k3 = update(k4, x + h * .5, h, n);
			for (int i = 0; i < n; i++) {
				k3[i] *= h;
				k4[i] = y[i] + k3[i];
				y[i] += k1[i] * c6 + (k2[i] + k3[i]) * c3;
			}
			k1 = update(k4, x + h, h, n);
			for (int i = 0; i < n; i++) {
				y[i] += k1[i] * h * c6;
			}
			CurrentYs = y;
		}
		const double c3 = 1.0 / 3.0;
		double c6 = c3 * .5;

		public Series[] Evaluate(double xi, double xf, double h, int n,
			double[] initialYVals, updateYVals deriv) {
			this.update = deriv;
			double[] y = initialYVals;
			double x = xi;
			double dx = h;
			
			double[] k1 = new double[n];
			double[] k2 = new double[n];
			double[] k3 = new double[n];
			double[] k4 = new double[n];
			Series[] ser = new Series[4];
			for (int i = 0; i < n; i++) {
				ser[i] = new Series();
				ser[i].ChartType = SeriesChartType.Line;
			}
			for (int j = 0; x < xf; j++) {
				for (int l = 0; l < 4; l++) {
					ser[l].Points.Add(new DataPoint(x, y[l]));
				}
				UpdateYVals(x, y, h, n);
				y = CurrentYs;
				x += dx;
			}
			return ser;
		}
	}
}

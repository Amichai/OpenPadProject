using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	//y'[n]  = f_i(x, y[n]) i => [0,n]
	public class RungeKutta {
		updateYVals update;
		public delegate Tuple<double[], double[]> updateYVals(double[] y, double[] dy, double x, double h , int n);
		public RungeKutta(int n,
			double[] initialYVals,
			updateYVals update) {
			N = n;
			this.update = update;
			for (int i = 0; i < n; i++) {
				y[i] = initialYVals[i];
			}
		}
		static int N;
		double[] y = new double[N];

		public double[] Evaluate(double xi, double xf, double h) {
			double x = xi;
			double dx = h;
			double c3 = 1.0 / 3.0, c6 = c3 * .5;
			double[] k1 = new double[N];
			double[] k2 = new double[N];
			double[] k3 = new double[N];
			double[] k4 = new double[N];
			for (int j = 0; x < xf; j++) {
				x += dx;
				var temp = update(y, k1, x, h, N);
				y = temp.Item1;
				k1 = temp.Item2;
				for (int i = 0; i < N; i++) {
					k1[i] *= h;
					k4[i] = y[i] + k1[i] * .5;
				}
				temp = update(k4, k2, x + h*.5, h, N);
				k4 = temp.Item1;
				k2 = temp.Item2;
				for (int i = 0; i < N; i++) {
					k2[i] *= h;
					k4[i] = y[i] + k2[i] * .5;
				}
				temp = update(k4, k3, x + h*.5, h, N);
				k4 = temp.Item1;
				k3 = temp.Item2;
				for (int i = 0; i < N; i++) {
					k3[i] *= h;
					k4[i] = y[i] + k3[i];
					y[i] += k1[i] * c6 + (k2[i] + k3[i]) * c3;
				}
				temp = update(k4, k1, x + h, h, N);
				k4 = temp.Item1;
				k1 = temp.Item2;
				for (int i = 0; i < N; i++) {
					y[i] += k1[i] * h * c6;
				}
				return y;
			}
			throw new Exception();
		}
	}
}

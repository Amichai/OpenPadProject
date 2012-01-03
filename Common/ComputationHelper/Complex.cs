using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public class Complex {
		public Complex(double real, double complex) {
			this.real = real;
			this.complex = complex;
		}
		double real, complex;

		public double Abs() {
			return real * real + complex * complex;
		}

		public static Complex operator *(double var1, Complex var2) {
			return new Complex(var2.real * var1, var2.complex * var1);
		}

		public static Complex operator *(Complex var1, Complex var2) {
			return new Complex(var1.real * var2.real - var1.complex * var2.complex,
				var1.real * var2.complex + var1.complex * var2.real);
		}
		public static Complex c_div(Complex a, Complex b) {
			return new Complex(a.real * b.real + a.complex * b.complex,
				a.complex * b.real - b.complex * a.real);
		}
		public static Complex operator +(Complex a, Complex b) {
			return new Complex(a.real + b.real, a.complex + b.complex);
		}
		public static Complex PolarToComplex(double r, double phi) {
			return new Complex(r * Math.Cos(phi), r * Math.Sin(phi));
		}
		public static Complex Zero() {
			return new Complex(0, 0);
		}
	}
}

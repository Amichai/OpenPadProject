using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemValues {
	public class NumericalValue {
		public NumericalValue(string val) {
			this.val = decimal.Parse(val);
		}

		public NumericalValue(decimal num) {
			this.val = num;
		}
		public NumericalValue(double num) {
			this.val = (decimal)num;
		}

		public decimal AsDecimal() {
			return val;
		}
		public double AsDouble() {
			return (double)val;
		}
		decimal val;
		public override string ToString() {
			return this.val.ToString();
		}

	}
	///Handle negative signs, implicit multiplication
	///last value operations
}

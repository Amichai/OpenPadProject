using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler {
	public class Value {
		public Value(string val) {
			this.val = double.Parse(val);
		}

		public Value(double num) {
			this.val = num;
		}

		internal double AsNum() {
			return val;
		}
		double val;
		public override string ToString() {
			return this.val.ToString();
		}
	}
}
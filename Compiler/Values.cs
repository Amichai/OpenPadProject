using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler {
	public class Values {
		double val;
		public Values(string val) {
			this.val = double.Parse(val);
		}
	}
}
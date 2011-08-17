using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler {
	public class Expression {
		private string textOfCurrentLine;

		public Expression(string textOfCurrentLine) {
			// TODO: Complete member initialization
			this.textOfCurrentLine = textOfCurrentLine;
		}

		public string Output { get; set; }

		public Values ReturnValue { get; set; }
	}
}

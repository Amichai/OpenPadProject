using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler;
using LoggingManager;
using SystemValues;

namespace TabbedInterface {
	class TextualContent {
		private string currentLine = string.Empty;
		private Expression localExpression;
		private string outputString = string.Empty;
		private NumericalValue value;

		public void SetCurrentLine(string line){
			currentLine = line;
			if(currentLine.Count() > 0)
				localExpression = new Expression(currentLine);
			if (localExpression != null) {
				value = localExpression.Evaluate();
				outputString = localExpression.Output;
			}
		}

		internal string GetOutputString() {
			SystemLog.Add(GetValue(), LogObjectType.value);
			return outputString;
		}

		internal NumericalValue GetValue() {
			return value;
		}
	}
}

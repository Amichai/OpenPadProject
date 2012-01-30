using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler;
using System.Diagnostics;
using Common;

namespace TabbedInterface {
	class TextualContent {
		public enum LogOrPrint { log, print};
		private string currentLine = string.Empty;
		private Expression localExpression;
		public string OutputString {get; set;}
		public NumericalValue Value = null;
		string failureMessage = string.Empty;
		private static int lineNumber = 0;

		public LogOrPrint LogOrPrintResult() {
			if (OutputString == null)
				return TextualContent.LogOrPrint.log;
			else
				return TextualContent.LogOrPrint.print;
		}

		public void SetCurrentLine(string line){
			lineNumber++;
			currentLine = line;
			Value = null;
			if(currentLine.Count() > 0){
				localExpression = new Expression(currentLine);
				if (localExpression.NumericalEvaluation != null) {
					Value = localExpression.NumericalEvaluation;
					OutputString = localExpression.OutputString;
				} else {
					OutputString = "\""+line+"\" " + localExpression.OutputString;
				}
			}
		}

		public string GetCurrentLine() {
			if (currentLine != null)
				return currentLine;
			else return string.Empty;
		}
	}
}
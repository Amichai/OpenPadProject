using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Compiler {
	public class Expression {
		private string input;
		public Tokens Tokens;
		private ReturnMessage ReturnMessage;
		public List<Token> PostFixedTokens; 
		public ParseTree ParseTree = new ParseTree();
		public NumericalValue NumericalEvaluation;
		public string OutputString { get; set; }
		private string failureMessage { get; set; }

		public Expression(string textOfCurrentLine) {
			this.input = textOfCurrentLine;
			Tokens = new Tokens(input);
			ReturnMessage = new ShuntingYard().ConvertToPostFixed(Tokens.AsList());
			if (ReturnMessage.Success == false) {
				failureMessage = ReturnMessage.Message;
			} else {
				PostFixedTokens = (List<Token>)ReturnMessage.ReturnValue;
				//Put postfixed Tokens into the tree
				foreach (Token t in PostFixedTokens) {
					ReturnMessage = ParseTree.AddToken(t);
					if (ReturnMessage.Success == false) { 
						failureMessage = ReturnMessage.Message;
					}
				}
			}
			//Set the outputString
			if (ReturnMessage.Success) {
				NumericalEvaluation = ParseTree.SetRoot().Evaluate();
				SystemLog.Add(NumericalEvaluation, LogObjectType.value);
				OutputString = NumericalEvaluation.ToString();
			} else {
				NumericalEvaluation = null;
				SystemLog.Add(failureMessage, LogObjectType.failureMessage);
				OutputString = failureMessage;
			}
		}
	}
}

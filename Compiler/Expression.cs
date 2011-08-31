using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageHandling;
using SystemValues;

namespace Compiler {
	public class Expression {
		private string input;
		public Tokens Tokens;
		public ReturnMessage returnMessage;
		public List<Token> PostFixedTokens;
		public ParseTree ParseTree = new ParseTree();

		public Expression(string textOfCurrentLine) {
			this.input = textOfCurrentLine;
			Tokens = new Tokens(input);
			returnMessage = new ShuntingYard().ConvertToPostFixed(Tokens.AsList());
			if (returnMessage.Success == false) {
				//Report failed operation to UI
			} else {
				PostFixedTokens = (List<Token>)returnMessage.ReturnValue;
				//Put postfixed Tokens into the tree
				foreach (Token t in PostFixedTokens) {
					returnMessage = ParseTree.AddToken(t);
					if (returnMessage.Success == false) { 
						//Report the failure to the UI
					}
				}
				Output = ParseTree.SetRoot().Evaluate().ToString();
			}
		}

		public NumericalValue Evaluate() {
			return ParseTree.Evaluate();
		}

		public string Output { get; set; }
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageHandling;

namespace Compiler {
	/// <summary>Implements the shunting yard algorithm to convert a list of tokens into a postfixed list
	/// of tokens.</summary>
	public class ShuntingYard {
		Stack<Token> operatorStack = new Stack<Token>();
		private void handleOperatorPrecedence(Token token) {
			while (operatorStack.Count() > 0 && InfixOperators.GetOpInfo.ContainsKey(operatorStack.First().TokenString)
			&& stackOperatorPrecedence(operatorStack.First().TokenString, token.TokenString)) {
				postFixedTokens.Add(operatorStack.Pop());
			}
			operatorStack.Push(token);
		}
		private bool stackOperatorPrecedence(string op1, string op2) {
			int precedenceValue = InfixOperators.GetOpInfo[op1].PrecedenceValue -
								InfixOperators.GetOpInfo[op2].PrecedenceValue;
			if (precedenceValue > 0)
				return true; //Stack operator has precedence
			else if (precedenceValue < 0)
				return false; //Inspection operator has prcedence
			else if (precedenceValue == 0)
				if (InfixOperators.GetOpInfo[op2].RightAssociative)
					return false; //Right associative so tie goes right
				else return true;
			else throw new Exception("Unhandled");
		}
		public ReturnMessage ConvertToPostFixed(List<Token> tokens) {
			foreach (Token token in tokens) {
				switch (token.TokenType) {
					case TokenType.identifier:
						if (Functions.FunctionLookup.ContainsKey(token.TokenString.ToLower()))
							operatorStack.Push(token);
						else return new ReturnMessage("We only know how to handle functions right now!");
						//This needs to handle keywords and identifiers as well!
						//TODO: make a keyword for the word ans and pi
						break;
					case TokenType.numberLiteral:
						postFixedTokens.Add(token);
						break;
					case TokenType.operatorOrPunctuation:
						if (InfixOperators.GetOpInfo.ContainsKey(token.TokenString))
							handleOperatorPrecedence(token);
						else if (token.TokenString == ",") {
							if (operatorStack.Count() == 0)
								return new ReturnMessage(token.TokenString + " comma and no arguments.");
							else
								while (operatorStack.First().TokenString != "(")
									postFixedTokens.Add(operatorStack.Pop());
						} else throw new Exception("Unhandled operator/punctuation mark.");
						break;
					case TokenType.atomicOperatorOrPunctuation:
						if (InfixOperators.GetOpInfo.ContainsKey(token.TokenString)) {
							handleOperatorPrecedence(token);
						} else if (token.TokenString == "(")
							operatorStack.Push(token);
						else if (token.TokenString == ")") {
							while (operatorStack.First().TokenString != "(")
								postFixedTokens.Add(operatorStack.Pop());
							//If no parenthesis found, mismatched parenthesis exception
							operatorStack.Pop();
						} else throw new Exception("Unhandled operator/punctuation mark.");
						break;
					default:
						throw new Exception("Unknown token type.");
				}
			}
			while (operatorStack.Count() > 0)
				postFixedTokens.Add(operatorStack.Pop());
			return new ReturnMessage(true, postFixedTokens);
		}
		private List<Token> postFixedTokens = new List<Token>();
	}
}

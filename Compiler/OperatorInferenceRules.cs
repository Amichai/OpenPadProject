using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoggingManager;

namespace Compiler {
	public static class OperatorInferenceRules {
		public static Token TestForTokenInference(List<Token> tokens, Token tokenToAdd) {
			if (tokens.Count() > 0) {
				//When a minus or plus sign is read as a negative number, add a plus sign before the number
				//IE: 4 -5 becomes 4 + -5
				if (tokenToAdd.TokenType == TokenType.numberLiteral
					&& (tokens.Last().TokenType == TokenType.numberLiteral 
							|| TokenizerRules.ClosedBraces.Contains(tokens.Last().TokenString))
					&& (tokenToAdd.TokenString[0] == '-' || tokenToAdd.TokenString[0] == '+')) {
					return new Token("+", TokenType.operatorOrPunctuation);
				}
				//Infer a multiplication sign between two sets of parenthesis
				//IE: (3)(5) becomes (3)*(5)
				if (TokenizerRules.OpenBraces.Contains(tokenToAdd.TokenString) 
					&& TokenizerRules.ClosedBraces.Contains(tokens.Last().TokenString)) {
						return new Token("*", TokenType.operatorOrPunctuation);
				}
				//Infer a multiplication sign between parenthesis and a number (that doesn't start with a minus sign)
				//IE: 3(4) becomes 3*(4)
				if (TokenizerRules.OpenBraces.Contains(tokenToAdd.TokenString) 
					&& tokens.Last().TokenType == TokenType.numberLiteral) {
					return new Token("*", TokenType.operatorOrPunctuation);
				}
				//IE: (4)3 becomes (4)*3
				if (tokenToAdd.TokenType == TokenType.numberLiteral
					&& TokenizerRules.ClosedBraces.Contains(tokens.Last().TokenString)
					&& tokenToAdd.TokenString[0] != '-') {
					return new Token("*", TokenType.operatorOrPunctuation);
				}
			}
			return null;
		}
		public static Token TestToChangeLastToken(Token lastToken, Token tokenToAdd) {
			if (lastToken != null
				&& (lastToken.TokenString == "+" || lastToken.TokenString == "-")
				&& tokenToAdd.TokenType == TokenType.numberLiteral) {
					return new Token(lastToken.TokenString + tokenToAdd.TokenString, TokenType.numberLiteral);
			}
			return null;
		}

		internal static Token TestForLastValueToken(int listCount, Token token) {
			if (listCount == 0 && Compiler.InfixOperators.GetOpInfo.ContainsKey(token.TokenString)) { 
				return new Token(SystemLog.GetLastValue().ToString(), TokenType.numberLiteral);
			}
			return null;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoggingManager;
using MessageHandling;

namespace Compiler {
	public static class OperatorInferenceRules {
		public static Token TestForTokenInference(List<Token> tokens, Token tokenToAdd) {
			if (tokens.Count() > 0) {
				//When a minus or plus sign is read as a negative number, add a plus sign before the number
				//IE: 4 -5 becomes 4 + -5
				TokenizerRules.CharInfo lastTknInfo = TokenizerRules.CharacterInfo.GetCharInfo(tokens.Last().TokenString);
				TokenizerRules.CharInfo currentTknInfo = TokenizerRules.CharacterInfo.GetCharInfo(tokenToAdd.TokenString);
				TokenizerRules.CharInfo lastCharInfo = TokenizerRules.CharacterInfo.GetCharInfo(tokenToAdd.TokenString[0].ToString());
				if ((tokenToAdd.TokenType == TokenType.numberLiteral)
					&& (tokens.Last().TokenType == TokenType.numberLiteral
							|| (lastTknInfo != null && lastTknInfo.Type == TokenizerRules.CharType.closedBrace))
							|| ((Keywords.KeywordLookup.ContainsKey(tokens.Last().TokenString.ToLower()) && Keywords.KeywordLookup[tokens.Last().TokenString.ToLower()].TokenType == TokenType.numberLiteral)
					&& (lastCharInfo != null && lastCharInfo.Type == TokenizerRules.CharType.plusMinus))) {
					return new Token("+", TokenType.operatorOrPunctuation);
				}
				//Infer a multiplication sign between two sets of parenthesis
				//IE: (3)(5) becomes (3)*(5)
				if ((TokenizerRules.CharacterInfo.ContainsKey(tokenToAdd.TokenString) && TokenizerRules.CharacterInfo.ContainsKey(tokens.Last().TokenString))
					&& TokenizerRules.CharacterInfo[tokenToAdd.TokenString].Type == TokenizerRules.CharType.openBrace
					&& TokenizerRules.CharacterInfo[tokens.Last().TokenString].Type == TokenizerRules.CharType.closedBrace) {
						return new Token("*", TokenType.operatorOrPunctuation);
				}
				//Infer a multiplication sign between parenthesis and a number (that doesn't start with a minus sign)
				//IE: 3(4) becomes 3*(4)
				if (TokenizerRules.CharacterInfo.ContainsKey(tokenToAdd.TokenString)
					&& TokenizerRules.CharacterInfo[tokenToAdd.TokenString].Type == TokenizerRules.CharType.openBrace
					&& tokens.Last().TokenType == TokenType.numberLiteral) {
					return new Token("*", TokenType.operatorOrPunctuation);
				}
				//IE: (4)3 becomes (4)*3
				if (TokenizerRules.CharacterInfo.ContainsKey(tokens.Last().TokenString)
					&& tokenToAdd.TokenType == TokenType.numberLiteral
					&& TokenizerRules.CharacterInfo[tokens.Last().TokenString].Type == TokenizerRules.CharType.closedBrace
					&& tokenToAdd.TokenString[0] != '-') {
					return new Token("*", TokenType.operatorOrPunctuation);
				}
			}
			return null;
		}
		public static Token TestToChangeLastToken(Token lastToken, Token tokenToAdd) {
			if (lastToken != null
				&& (TokenizerRules.CharacterInfo.ContainsKey(lastToken.TokenString) 
				&& TokenizerRules.CharacterInfo[lastToken.TokenString].Type == TokenizerRules.CharType.plusMinus)
				&& tokenToAdd.TokenType == TokenType.numberLiteral) {
					return new Token(lastToken.TokenString + tokenToAdd.TokenString, TokenType.numberLiteral);
			}
			return null;
		}

		internal static Token TestForLastValueToken(int listCount, Token token) {
			if (listCount == 0 && TokenizerRules.CharacterInfo.ContainsKey(token.TokenString)
				&& (TokenizerRules.CharacterInfo[token.TokenString].Type == TokenizerRules.CharType.plusMinus
				|| TokenizerRules.CharacterInfo[token.TokenString].Type == TokenizerRules.CharType.infixOp)){
				return new Token(SystemLog.GetLastNumericalValue().ToString(), TokenType.numberLiteral);
			}
			return null;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler {
	public class Tokens {
		public Tokens(List<Token> tokens){
			list = tokens;
		}
		private void addToList(Token token) {
			if (list.Count > 0) {
				Token tokenToChangeTo = OperatorInferenceRules.TestToChangeLastToken(AsList().Last(), token);
				if (tokenToChangeTo != null) {
					list.RemoveAt(list.Count - 1);
					token = tokenToChangeTo;
				}
				Token inferredToken = OperatorInferenceRules.TestForTokenInference(AsList(),token);
				if (inferredToken != null)
					list.Add(inferredToken);
			} else if (list.Count == 0) {
				Token inferredToken = OperatorInferenceRules.TestForLastValueToken(AsList().Count, token);
				if (inferredToken != null)
					list.Add(inferredToken);
			}

			list.Add(token);
		}
		public List<Token> AsList() {return list;}
		private List<Token> list = new List<Token>();
		public Tokens(string input) {
			tokenFactory tokenFactory = new tokenFactory();
			string[] inputWords = input.Split(' ', '\n', '\t');
			foreach (var word in inputWords) {
				foreach (var c in word) {
					if(tokenFactory.PublishTest(c)){
						addToList(tokenFactory.PublishToken());
					}
					tokenFactory.Add(c);
				}
				Token tokenToAdd = tokenFactory.PublishToken();
				addToList(tokenToAdd);
			}
		}
		private class tokenFactory {
			private string tokenString = string.Empty;
			private TokenType currentTokenType = TokenType.none;
			internal bool PublishTest(char c) {
				if (TokenizerRules.GetTokenInfo[currentTokenType].BreakTest(c)){
					return true;
				} else return false;
			}
			internal Token PublishToken() {
				Token tokenToReturn = new Token(tokenString, currentTokenType);
				tokenString = string.Empty;
				currentTokenType = TokenType.none;
				return tokenToReturn;
			}
			internal void Add(char c) {
				tokenString += c;
				currentTokenType = TokenizerRules.GetTokenInfo[currentTokenType].TokenUpdate(c);
			}
		}
	}
}

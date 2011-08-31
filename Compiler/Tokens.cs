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
			tokenConstructor tokenConstructor = new tokenConstructor();
			string[] inputWords = input.Split(' ', '\n', '\t');
			foreach (var word in inputWords) {
				foreach (var c in word) {
					if(tokenConstructor.PublishTest(c)){
						addToList(tokenConstructor.PublishToken());
					}
					tokenConstructor.Add(c);
				}
				Token tokenToAdd = tokenConstructor.PublishToken();
				addToList(tokenToAdd);
			}
		}
		private class tokenConstructor {
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

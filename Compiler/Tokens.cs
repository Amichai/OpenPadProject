using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler {
	public class Tokens {
		public Tokens(List<Token> tokens){
			list = tokens;
		}


		public List<Token> AsList() {
			return list;
		}
		private List<Token> list = new List<Token>();
		public Tokens(string input) {
			tokenConstructor tokenConstructor = new tokenConstructor();
			string[] inputWords = input.Split(' ', '\n', '\t');
			foreach (var word in inputWords) {
				foreach (var c in word) {
					if(tokenConstructor.PublishTest(c)){
						list.Add(tokenConstructor.PublishToken());
					}
					tokenConstructor.Add(c);
				}
				list.Add(tokenConstructor.PublishToken());
			}
		}
		private class tokenConstructor {
			private string tokenString = string.Empty;
			private TokenType currentTokenType = TokenType.none;
			internal bool PublishTest(char c) {
				if (Tokenizer.GetTokenInfo[currentTokenType].BreakTest(c)){
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
				currentTokenType = Tokenizer.GetTokenInfo[currentTokenType].TokenUpdate(c);
			}
		}
	}
}

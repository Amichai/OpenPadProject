using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler {
	public class Token {
		public string TokenString;
		public TokenType TokenType;

		public Token(string tokenString, TokenType currentTokenType) {
			// TODO: Complete member initialization
			this.TokenString = tokenString;
			this.TokenType = currentTokenType;
		}
	}

}

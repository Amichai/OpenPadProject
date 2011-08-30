using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler {
	public enum TokenType {identifier, numberLiteral, operatorOrPunctuation, none }
	static class Tokenizer {
		private static HashSet<char> operationsAndPunctuationThatCombine = new HashSet<char>() { ',', '\'', '-', '+', '*', '\\', '/', '%', '^', '=', '&', '#', '@', '!', '.', ':', ';', '|', '?' };
		private static HashSet<char> atomicPunctuationMarks = new HashSet<char>() { '{', '(', ')', '}' };
		public delegate bool Test(char c);
		public delegate TokenType Update(char c);
		private static bool noneBreakTest(char c) {
			return false;
		}
		/// <summary>Given an identifier token, should I break on this new char c?</summary>
		private static bool identifierBreakTest(char c) {
			if (operationsAndPunctuationThatCombine.Contains(c)
				|| atomicPunctuationMarks.Contains(c)) {
				return true;
			} else return false;
		}
		private static bool numberLiteralBreakTest(char c) {
			if (c == '.')
				return false;
			else if (operationsAndPunctuationThatCombine.Contains(c)
				|| atomicPunctuationMarks.Contains(c)) {
				return true;
			} else return false;
		}
		private static bool operationOrPunctuationBreakTest(char c) {
			if (operationsAndPunctuationThatCombine.Contains(c))
				return false;
			else return true;
		}
		private static TokenType noneTokenUpdate(char c) {
			if (char.IsLetter(c) || c == '_')
				return TokenType.identifier;
			else if (char.IsNumber(c))
				return TokenType.numberLiteral;
			else if (operationsAndPunctuationThatCombine.Contains(c)
				|| atomicPunctuationMarks.Contains(c))
				return TokenType.operatorOrPunctuation;
			else throw new Exception("Character unidentified");
		}
		/// <summary>Given an identifier token what token is it if I add on char c?</summary>
		/// <param name="c">Char to add</param>
		/// <returns>New Token type</returns>
		private static TokenType identifierTokenUpdate(char c) {
			if (char.IsLetter(c) || c == '_' || char.IsNumber(c))
				return TokenType.identifier;
			else throw new Exception("Can't append this char to an identifier. This token should have been published.");
		}
		private static TokenType numberLiteralTokenUpdate(char c) {
			if (char.IsNumber(c) || c == '.')
				return TokenType.numberLiteral;
			else if (char.IsLetter(c) || c == '_')
				return TokenType.identifier;
			else throw new Exception("Can't append this char to a number. This token should have been published.");
		}
		private static TokenType operationTokenUpdate(char c) {
			if (operationsAndPunctuationThatCombine.Contains(c)
				|| atomicPunctuationMarks.Contains(c))
				return TokenType.operatorOrPunctuation;
			else throw new Exception("Can't append this char to on operator/punctuation mark. This token should have been published.");
		}
		public class TokenInfo {
			public TokenInfo(Test test, Update update) {
				this.BreakTest = test;
				this.TokenUpdate = update;
			}
			public Test BreakTest;
			public Update TokenUpdate;
		}
		/// <summary>
		/// Returns a boolean to inform the client if a token of a given type should be published before the next character 'c'
		/// is appended.
		/// </summary>
		public static Dictionary<TokenType, TokenInfo> GetTokenInfo = new Dictionary<TokenType, TokenInfo>() {
			{TokenType.none,					new TokenInfo (new Test(noneBreakTest), 
														new Update(noneTokenUpdate)) },
			{TokenType.identifier,				new TokenInfo (new Test(identifierBreakTest), 
														new Update(identifierTokenUpdate)) },
			{TokenType.numberLiteral,			new TokenInfo (new Test(numberLiteralBreakTest), 
														new Update(numberLiteralTokenUpdate)) },
			{TokenType.operatorOrPunctuation,	new TokenInfo (new Test(operationOrPunctuationBreakTest), 
														new Update(operationTokenUpdate))},
		};
	}
	static class Functions {
		public delegate Node function(LinkedList<Node> parameters);
		private static Node abs(LinkedList<Node> parameters) {
			if (parameters.Count != 1)
				throw new Exception("Wrong number of parameters");
			return new NumberNode(Math.Abs(parameters.First().GetValue().AsNum()));
		}
		private static Node cos(LinkedList<Node> parameters) {
			if (parameters.Count != 1)
				throw new Exception("Wrong number of parameters");
			return new NumberNode(Math.Cos(parameters.First().GetValue().AsNum()));
		}
		private static Node sin(LinkedList<Node> parameters) {
			if (parameters.Count != 1)
				throw new Exception("Wrong number of parameters");
			return new NumberNode(Math.Sin(parameters.First().GetValue().AsNum()));
		}
		private static Node tan(LinkedList<Node> parameters) {
			if (parameters.Count != 1)
				throw new Exception("Wrong number of parameters");
			return new NumberNode(Math.Tan(parameters.First().GetValue().AsNum()));
		}
		public static Dictionary<string, FunctionInfo> FunctionLookup = new Dictionary<string, FunctionInfo>() {
			{"abs", new FunctionInfo( new function(abs), 1)},
			{"cos", new FunctionInfo( new function(cos), 1)},
			{"sin", new FunctionInfo( new function(sin), 1)},
			{"tan", new FunctionInfo( new function(tan), 1)},
		};
		public class FunctionInfo {
			public FunctionInfo(function func, int numOfParams) {
				this.Func = func;
				this.NumberOfParameters = numOfParams;
			}
			public function Func;
			public int NumberOfParameters;
		}
	}
	static class InfixOperators{
		public class OperatorInfo {
			public int PrecedenceValue;
			public bool RightAssociative;
			public op Compute;
			public OperatorInfo(int operatorPrecedenceValue, bool rightAssociative, op compute) {
				this.PrecedenceValue = operatorPrecedenceValue;
				this.RightAssociative = rightAssociative;
				this.Compute = compute;
			}
		}
		public static Dictionary<string, OperatorInfo> GetOpInfo = new Dictionary<string, OperatorInfo>() { 
			{"+", new OperatorInfo(1, false, new op(plus))},
			{"-", new OperatorInfo(1, false, new op(minus))},
			{"*", new OperatorInfo(2, false, new op(times))},
			{"/", new OperatorInfo(2, false, new op(dividedBy))},
			{"%", new OperatorInfo(2, false, new op(modulus))},
			{"^", new OperatorInfo(3, true, new op(power))},
		};
		public delegate Value op(Value p1, Value p2);
		static Value plus(Value p1, Value p2) { return new Value(p1.AsNum() + p2.AsNum()); }
		static Value minus(Value p1, Value p2) { return new Value(p1.AsNum() - p2.AsNum()); }
		static Value times(Value p1, Value p2) { return new Value(p1.AsNum() * p2.AsNum()); }
		static Value dividedBy(Value p1, Value p2) { return new Value(p1.AsNum() / p2.AsNum()); }
		static Value modulus(Value p1, Value p2) { return new Value(p1.AsNum() % p2.AsNum()); }
		static Value power(Value p1, Value p2) { return new Value(Math.Pow(p1.AsNum(), p2.AsNum())); }
	}

}
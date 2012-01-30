using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Common;

namespace Compiler {
	//Compiler rules
	//Dictionary with lookup method to classify individual characters
	//


	public enum TokenType {identifier, numberLiteral, operatorOrPunctuation, atomicOperatorOrPunctuation, none }
	static class TokenizerRules {
		public enum CharType {infixOp, syntaxChar, openBrace, closedBrace, postfixOp, plusMinus, digit, letterOrUnderscore };
		/// <summary>returns null if the passed string isn't found in the dictionary</summary>
		public static CharInfo GetCharInfo(this Dictionary<string, CharInfo> dictionary, string lookupString) {
			char asChar = lookupString.ToCharArray().First();
			if (Char.IsDigit(asChar))
				return new CharInfo(CharType.digit, false);
			else if (Char.IsLetter(asChar) || asChar == '_')
				return new CharInfo(CharType.letterOrUnderscore, false);
			else if (dictionary.ContainsKey(lookupString)) {
				return dictionary[lookupString];
			} else return null;
		}
		public static Dictionary<string, CharInfo> CharacterInfo = new Dictionary<string, CharInfo>() {
			{ ",", new CharInfo(CharType.syntaxChar, false)},
			{ "\'", new CharInfo(CharType.syntaxChar, false)},
			{ "*", new CharInfo(CharType.infixOp, false)},
			{ "\\", new CharInfo(CharType.syntaxChar, false)},
			{ "/", new CharInfo(CharType.infixOp, false)},
			{ "%", new CharInfo(CharType.infixOp, false)},
			{ "^", new CharInfo(CharType.infixOp, false)},
			{ "=", new CharInfo(CharType.syntaxChar, false)},
			{ "&", new CharInfo(CharType.syntaxChar, false)},
			{ "#", new CharInfo(CharType.syntaxChar, false)},
			{ "@", new CharInfo(CharType.syntaxChar, false)},
			{ "!", new CharInfo(CharType.postfixOp, false)},
			{ ".", new CharInfo(CharType.syntaxChar, false)},
			{ ":", new CharInfo(CharType.syntaxChar, false)},
			{ ";", new CharInfo(CharType.syntaxChar, false)},
			{ "|", new CharInfo(CharType.syntaxChar, false)},
			{ "?", new CharInfo(CharType.syntaxChar, false)},
			{ "{", new CharInfo(CharType.openBrace, true)},
			{ "(", new CharInfo(CharType.openBrace, true)},
			{ "}", new CharInfo(CharType.closedBrace, true)},
			{ ")", new CharInfo(CharType.closedBrace, true)},
			{ "+", new CharInfo(CharType.plusMinus, true)},
			{ "-", new CharInfo(CharType.plusMinus, true)},
		};

		public class CharInfo {
			public CharInfo(CharType type, bool atomic) {
				this.atomic = atomic;
				this.Type = type;
			}
			/// <summary>Does this char combine with other chars</summary>
			public bool atomic;
			public CharType Type;
		}
		public delegate bool Test(char c);
		public delegate TokenType Update(char c);
		private static bool noneBreakTest(char c) {
			return false;
		}
		/// <summary>Given an identifier token, should I break on this new char c?</summary>
		private static bool identifierBreakTest(char c) {
			if(CharacterInfo.ContainsKey(c.ToString())){
				return true;
			} else return false;
		}
		private static bool numberLiteralBreakTest(char c) {
			if (c == '.')
				return false;
			else if (CharacterInfo.ContainsKey(c.ToString())) {
				return true;
			} else return false;
		}
		private static bool operationOrPunctuationBreakTest(char c) {
			if (CharacterInfo.ContainsKey(c.ToString()) && !CharacterInfo[c.ToString()].atomic)
				return false;
			else return true;
		}
		private static bool atomicOperatorOrPunctuationBreakTest(char c) { return true;  }
		private static TokenType noneTokenUpdate(char c) {
			if(CharacterInfo.GetCharInfo(c.ToString()).Type == CharType.letterOrUnderscore)
				return TokenType.identifier;
			else if (char.IsNumber(c))
				return TokenType.numberLiteral;
			else if (!CharacterInfo[c.ToString()].atomic)
				return TokenType.operatorOrPunctuation;
			else if (CharacterInfo[c.ToString()].atomic) {
				return TokenType.atomicOperatorOrPunctuation;
			} else throw new Exception("Character unidentified");
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
			if (CharacterInfo.ContainsKey(c.ToString()))
				return TokenType.operatorOrPunctuation;
			else throw new Exception("Can't append this char to on operator/punctuation mark. This token should have been published.");
		}
		private static TokenType atomicOperationOrPunctuationUpdate(char c) {
			throw new Exception("Can't append this char to on operator/punctuation mark. This token should have been published.");
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
			{TokenType.atomicOperatorOrPunctuation, new TokenInfo(new Test(atomicOperatorOrPunctuationBreakTest), 
														new Update(atomicOperationOrPunctuationUpdate))}
		};

		//TODO: key word to call computational physics library
		/// <summary>
		/// Contains the logic for converting infix tokens to post fixed tokens
		/// </summary>
	}
	static class Functions {
		public delegate Node function(LinkedList<Node> parameters);
		private static Node abs(LinkedList<Node> parameters) {
			if (parameters.Count != 1)
				throw new Exception("Wrong number of parameters");
			return new NumberNode(Math.Abs(parameters.First().GetValue().AsDouble()));
		}
		private static Node cos(LinkedList<Node> parameters) {
			if (parameters.Count != 1)
				throw new Exception("Wrong number of parameters");
			return new NumberNode(Math.Cos(parameters.First().GetValue().AsDouble()));
		}
		private static Node sin(LinkedList<Node> parameters) {
			if (parameters.Count != 1)
				throw new Exception("Wrong number of parameters");
			return new NumberNode(Math.Sin(parameters.First().GetValue().AsDouble()));
		} 
		private static Node tan(LinkedList<Node> parameters) {
			if (parameters.Count != 1)
				throw new Exception("Wrong number of parameters");
			//Use a numerics library that can do decimal type math
			return new NumberNode((decimal)Math.Tan((double)parameters.First().GetValue().AsDecimal()));
		}
		public static Dictionary<string, FunctionInfo> FunctionLookup = new Dictionary<string, FunctionInfo>() {
			{"sin", new FunctionInfo( new function(sin), 1)},
			{"tan", new FunctionInfo( new function(tan), 1)},
			{"cos", new FunctionInfo( new function(cos), 1)},
			{"abs", new FunctionInfo( new function(abs), 1)},
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
	//TODO: Fix the order of operations bug by functions
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
		public delegate NumericalValue op(NumericalValue p1, NumericalValue p2);
		static NumericalValue plus(NumericalValue p1, NumericalValue p2) { return new NumericalValue(p1.AsDecimal() + p2.AsDecimal()); }
		static NumericalValue minus(NumericalValue p1, NumericalValue p2) { return new NumericalValue(p1.AsDecimal() - p2.AsDecimal()); }
		static NumericalValue times(NumericalValue p1, NumericalValue p2) { return new NumericalValue(p1.AsDecimal() * p2.AsDecimal()); }
		static NumericalValue dividedBy(NumericalValue p1, NumericalValue p2) { return new NumericalValue(p1.AsDecimal() / p2.AsDecimal()); }
		static NumericalValue modulus(NumericalValue p1, NumericalValue p2) { return new NumericalValue(p1.AsDecimal() % p2.AsDecimal()); }
		static NumericalValue power(NumericalValue p1, NumericalValue p2) {
				return new NumericalValue((decimal)Math.Pow((double)p1.AsDecimal(), (double)p2.AsDecimal()));
			//TODO: Handle all overflow exceptions
			//TODO: Catch overflow exceptions
			//Get a mathematics library that can do computations without overflow or divide by zero
			//Report to UI or save the value without evaluating
		}
	}
	static class Keywords {
		public static Dictionary<string, KeywordInfo> KeywordLookup = new Dictionary<string, KeywordInfo>() {
			{"ans", new KeywordInfo(AppendTokenTo.child, TokenType.numberLiteral, new BuildToken(buildAnsToken)) },
			{"pi", new KeywordInfo(AppendTokenTo.child, TokenType.numberLiteral, new BuildToken(buildPiToken))}
		};
		public class KeywordInfo {
			public KeywordInfo(AppendTokenTo append, TokenType type, BuildToken buildMethod) {
				this.AppendTokenTo = append;
				this.TokenType = type;
				this.BuildMethod = buildMethod;
			}
			public AppendTokenTo AppendTokenTo;
			public TokenType TokenType;
			public BuildToken BuildMethod;
		}
		private static Node buildAnsToken() {
			return new NumberNode(SystemLog.GetLastNumericalValue());
		}
		private static Node buildPiToken(){
			return new NumberNode(Math.PI);
		}
		public delegate Node BuildToken();
	}
	public enum AppendTokenTo { parent, child }
	static class ParseTreeBuilder {
		public static Dictionary<TokenType, NodeInfo> NodeLookup = new Dictionary<TokenType, NodeInfo>() {
			{TokenType.numberLiteral,				new NodeInfo(buildNumberNode)},
			{TokenType.identifier,					new NodeInfo(buildIdentifierNode)},
			{TokenType.operatorOrPunctuation,		new NodeInfo(buildOperatorOrPunctuationNode)},
			{TokenType.atomicOperatorOrPunctuation, new NodeInfo(buildAtomicOperatorOrPunctuationNode)},
		};
		public class NodeInfo {
			public BuildParseTreeNode BuildNode;
			public NodeInfo(BuildParseTreeNode builder) {
				this.BuildNode = builder;
			}
		}
		private static Node buildAtomicOperatorOrPunctuationNode(Token token) {
			Node nodeToReturn = null;
			if (InfixOperators.GetOpInfo.ContainsKey(token.TokenString)) {
				nodeToReturn = new TwoParameterOperatorNode(token.TokenString);
				nodeToReturn.AppendMeTo = AppendTokenTo.parent;
			} return nodeToReturn;
		}
		private static Node buildOperatorOrPunctuationNode(Token token) {
			Node nodeToReturn = null;
			if (InfixOperators.GetOpInfo.ContainsKey(token.TokenString)) {
				nodeToReturn = new TwoParameterOperatorNode(token.TokenString);
				nodeToReturn.AppendMeTo = AppendTokenTo.parent;
			}
			return nodeToReturn;
		}
		private static Node buildIdentifierNode(Token token) {
			Node nodeToReturn = null;
			if (Functions.FunctionLookup.ContainsKey(token.TokenString.ToLower())) {
				nodeToReturn = new FunctionNode(token.TokenString);
				nodeToReturn.AppendMeTo = AppendTokenTo.parent;
			} else if (Keywords.KeywordLookup.ContainsKey(token.TokenString.ToLower())) {
				nodeToReturn = Keywords.KeywordLookup[token.TokenString.ToLower()].BuildMethod();
				nodeToReturn.AppendMeTo = Keywords.KeywordLookup[token.TokenString.ToLower()].AppendTokenTo;
			} 
			return nodeToReturn;
		}
		private static Node buildNumberNode(Token token) {
			double parsedVal;
			Node nodeToReturn;
			if (double.TryParse(token.TokenString, out parsedVal)) {
				nodeToReturn = new NumberNode(parsedVal);
				nodeToReturn.AppendMeTo = AppendTokenTo.child;
				return nodeToReturn;
			} else return null;
		}
		public delegate Node BuildParseTreeNode(Token token);
	}
}
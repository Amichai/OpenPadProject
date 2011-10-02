using Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PadTest
{
	
	
	/// <summary>
	///This is a test class for ExpressionTest and is intended
	///to contain all ExpressionTest Unit Tests
	///</summary>
	[TestClass()]
	public class ExpressionTest {


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for Expression Constructor
		///</summary>
		[TestMethod()]
		public void ExpressionConstructorTest() {
			Expression target = new Expression("3+4*334 - te 33z 34.3.4,() || ,");
			Tokens tokens = new Tokens(new System.Collections.Generic.List<Token>(){
				new Token("3", TokenType.numberLiteral),
				new Token("+", TokenType.operatorOrPunctuation),
				new Token("+4", TokenType.numberLiteral),
				new Token("*", TokenType.operatorOrPunctuation),
				new Token("334", TokenType.numberLiteral),
				new Token("-", TokenType.atomicOperatorOrPunctuation),
				new Token("te", TokenType.identifier),
				new Token("33z", TokenType.identifier),
				new Token("34.3.4", TokenType.numberLiteral),
				new Token(",", TokenType.operatorOrPunctuation),
				new Token("(", TokenType.atomicOperatorOrPunctuation),
				new Token(")", TokenType.atomicOperatorOrPunctuation),
				new Token("||", TokenType.operatorOrPunctuation),
				new Token(",", TokenType.operatorOrPunctuation)
			});
			for(int i=0; i < tokens.AsList().Count; i++){
				Assert.AreEqual<string>(tokens.AsList()[i].TokenString, target.Tokens.AsList()[i].TokenString);
				Assert.AreEqual<TokenType>(tokens.AsList()[i].TokenType, target.Tokens.AsList()[i].TokenType);
			}
		}
		/// <summary>
		///A test for Expression Constructor
		///</summary>
		[TestMethod()]
		public void ExpressionConstructorTest1() {
			string textOfCurrentLine = "4+-1(3)(-3)3--3";
			Expression target = new Expression(textOfCurrentLine);
			//Assert.AreEqual(target.OutputString, "34");
		}
	}
}

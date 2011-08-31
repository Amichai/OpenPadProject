using Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using MessageHandling;

namespace PadTest
{
    
    
    /// <summary>
    ///This is a test class for ShuntingYardTest and is intended
    ///to contain all ShuntingYardTest Unit Tests
    ///</summary>
	[TestClass()]
	public class ShuntingYardTest {


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
		///A test for ConvertToPostFixed
		///</summary>
		[TestMethod()]
		public void ConvertToPostFixedTest() {
			ShuntingYard target = new ShuntingYard();
			Tokens tokens = new Tokens("3*4 - 5*(4-3*3)");
			ReturnMessage actual = target.ConvertToPostFixed(tokens.AsList());

			ReturnMessage expected = new ReturnMessage(true,
					new List<Token>() { 
					new Token("3", TokenType.numberLiteral), 
					new Token("4", TokenType.numberLiteral),
					new Token("*", TokenType.operatorOrPunctuation),
					new Token("-5", TokenType.numberLiteral), 
					new Token("4", TokenType.numberLiteral),
					new Token("-3", TokenType.numberLiteral),
					new Token("3", TokenType.numberLiteral), 
					new Token("*", TokenType.operatorOrPunctuation),
					new Token("+", TokenType.operatorOrPunctuation),
					new Token("*", TokenType.operatorOrPunctuation), 
					new Token("+", TokenType.operatorOrPunctuation),
					});
			List<Token> returnedTokens = (List<Token>)actual.ReturnValue;
			List<Token> expectedTokens = (List<Token>)expected.ReturnValue;
			for(int i=0;i < returnedTokens.Count;i++){
				Assert.AreEqual<string>(returnedTokens[i].TokenString, expectedTokens[i].TokenString);
				Assert.AreEqual(returnedTokens[i].TokenType, expectedTokens[i].TokenType);
			}
		}
	}
}

using W3b.Sine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PadTest
{
    
    
    /// <summary>
    ///This is a test class for BigIntTest and is intended
    ///to contain all BigIntTest Unit Tests
    ///</summary>
	[TestClass()]
	public class BigIntTest {


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
		///A test for GetFactors
		///</summary>
		[TestMethod()]
		public void GetFactorsTest() {
			BigInt target = new BigInt(1000);
			List<BigInt> expected = new List<BigInt>() {
				new BigInt(2),
				new BigInt(2),
				new BigInt(2),
				new BigInt(5),
				new BigInt(5),
				new BigInt(5),
			};
			List<BigInt> actual= target.GetFactors();
			for (int i = 0; i < actual.Count; i++) {
				Assert.AreEqual(expected[i].ToString(), actual[i].ToString());
			}
		}
	}
}

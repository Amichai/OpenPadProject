using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PadTest
{
    
    
    /// <summary>
    ///This is a test class for IntegerTest and is intended
    ///to contain all IntegerTest Unit Tests
    ///</summary>
	[TestClass()]
	public class IntegerTest {


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
		///A test for Add
		///</summary>
		[TestMethod()]
		public void AddTest() {
			Integer target = new Integer("91234");
			Integer b = new Integer("23769");
			Integer expected = new Integer("115003");
			Integer actual = target.Add(b);
			Assert.AreEqual(expected.ToString(), actual.ToString());
		}

		/// <summary>
		///A test for Subtract
		///</summary>
		[TestMethod()]
		public void SubtractTest() {
			Integer target = new Integer("7777");
			Integer b = new Integer("1111");
			Integer expected = new Integer("6666");
			Integer actual = target.Subtract(b);
			Assert.AreEqual(expected.ToString(), actual.ToString());

			target = new Integer("6666");
			b = new Integer("1111");
			expected = new Integer("5555");
			actual = target.Subtract(b);
			Assert.AreEqual(expected.ToString(), actual.ToString());

			target = new Integer("1111");
			b = new Integer("6666");
			expected = new Integer("5555", false);
			actual = target.Subtract(b);
			Assert.AreEqual(expected.ToString(), actual.ToString());

			target = new Integer("6234");
			b = new Integer("1976");
			expected = new Integer("4258");
			actual = target.Subtract(b);
			Assert.AreEqual(expected.ToString(), actual.ToString());

		}
	}
}

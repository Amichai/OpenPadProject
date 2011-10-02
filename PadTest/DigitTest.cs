using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PadTest
{
    
    
    /// <summary>
    ///This is a test class for DigitTest and is intended
    ///to contain all DigitTest Unit Tests
    ///</summary>
	[TestClass()]
	public class DigitTest {


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
		///A test for op_Subtraction
		///</summary>
		[TestMethod()]
		public void op_SubtractionTest() {
			Digit a = Digit.Eight;
			Digit b = Digit.Five;
			Tuple<Digit, bool> expected = new Tuple<Digit, bool>(Digit.Three, true);
			Tuple<Digit, bool> actual= (a - b);
			Assert.AreEqual(expected.Item1.ToChar(), actual.Item1.ToChar());
			Assert.AreEqual(expected.Item2, actual.Item2);


			a = Digit.One;
			b = Digit.Seven;
			expected = new Tuple<Digit, bool>(Digit.Six, false);
			actual = (a - b);
			Assert.AreEqual(expected.Item1.ToChar(), actual.Item1.ToChar());
			Assert.AreEqual(expected.Item2, actual.Item2);

			a = Digit.Nine;
			b = Digit.Nine;
			expected = new Tuple<Digit, bool>(Digit.Zero, true);
			actual = (a - b);
			Assert.AreEqual(expected.Item1.ToChar(), actual.Item1.ToChar());
			Assert.AreEqual(expected.Item2, actual.Item2);

			a = Digit.Two;
			b = Digit.Three;
			expected = new Tuple<Digit, bool>(Digit.One, false);
			actual = (a - b);
			Assert.AreEqual(expected.Item1.ToChar(), actual.Item1.ToChar());
			Assert.AreEqual(expected.Item2, actual.Item2);

			a = Digit.Eight;
			b = Digit.Nine;
			expected = new Tuple<Digit, bool>(Digit.One, false);
			actual = (a - b);
			Assert.AreEqual(expected.Item1.ToChar(), actual.Item1.ToChar());
			Assert.AreEqual(expected.Item2, actual.Item2);

			a = Digit.Nine;
			b = Digit.Eight;
			expected = new Tuple<Digit, bool>(Digit.One, true);
			actual = (a - b);
			Assert.AreEqual(expected.Item1.ToChar(), actual.Item1.ToChar());
			Assert.AreEqual(expected.Item2, actual.Item2);

			a = Digit.Zero;
			b = Digit.Seven;
			expected = new Tuple<Digit, bool>(Digit.Seven, false);
			actual = (a - b);
			Assert.AreEqual(expected.Item1.ToChar(), actual.Item1.ToChar());
			Assert.AreEqual(expected.Item2, actual.Item2);
		}

		/// <summary>
		///A test for op_Inequality
		///</summary>
		[TestMethod()]
		public void op_InequalityTest() {
			Digit a = Digit.Eight;
			Digit b = Digit.Three;
			bool expected = true; 
			bool actual;
			actual = (a != b);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for op_LessThan
		///</summary>
		[TestMethod()]
		public void op_LessThanTest() {
			Digit a = Digit.Three;
			Digit b = Digit.Five;
			bool expected = true; 
			bool actual = (a < b);
			Assert.AreEqual(expected, actual);

			a = Digit.Eight;
			b = Digit.One;
			expected = false;
			actual = (a < b);
			Assert.AreEqual(expected, actual);

			a = Digit.Three;
			b = Digit.Three;
			expected = false;
			actual = (a < b);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for op_GreaterThan
		///</summary>
		[TestMethod()]
		public void op_GreaterThanTest() {
			Digit a = Digit.Three;
			Digit b = Digit.Five;
			bool expected = false;
			bool actual = (a > b);
			Assert.AreEqual(expected, actual);

			a = Digit.Eight;
			b = Digit.One;
			expected = true;
			actual = (a > b);
			Assert.AreEqual(expected, actual);

			a = Digit.Four;
			b = Digit.Four;
			expected = false;
			actual = (a > b);
			Assert.AreEqual(expected, actual);
		}
	}
}

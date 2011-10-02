using System;
using System.Collections.Generic;
using System.Text;

using Oyster.Math;

namespace W3b.Sine {

	/// <summary>W3b.Sine-style wrapping Oyster's IntX class. Used by BigRational.</summary>
	public class BigInt {

		private IntX _v;

		public BigInt(long number) {

			_v = new IntX(number);
			_v.Normalize();
		}

		public BigInt(String number) {

			_v = new IntX(number);
			_v.Normalize();
		}

		private BigInt(BigInt copyThis) {

			_v = new IntX(copyThis._v);
			_v.Normalize();
		}

		private BigInt(IntX value) {
			_v = value;
			_v.Normalize();
		}

		public BigIntFactory Factory {
			get { return BigIntFactory.Instance; }
		}

		public BigInt Clone() {

			return new BigInt(this);
		}

		public int CompareTo(BigInt other) {

			BigInt o = E(other);

			return _v.CompareTo(o._v);
		}

		public bool Equals(BigInt other) {

			//			if( !other.Floor().Equals( other ) ) { // check it's an integer
			//				
			//				return false;
			//			}

			BigInt o = E(other);

			return _v.Equals(o._v);
		}

		public int GetHashCode() {
			return _v.GetHashCode();
		}

		public String ToString() {
			return _v.ToString();
		}

		public bool IsZero {
			get { return _v.Equals(0); }
		}

		public bool Sign {
			get {
				return _v.CompareTo(0) >= 0;
			}
		}

		protected BigInt Add(BigInt other) {
			BigInt o = E(other);
			return new BigInt(_v + o._v);
		}

		protected BigInt Multiply(BigInt multiplicand) {
			BigInt o = E(multiplicand);
			return new BigInt(_v * o._v);
		}

		protected BigInt Divide(BigInt divisor) {
			BigInt o = E(divisor);
			return new BigInt(_v / o._v);
		}

		protected BigInt Modulo(BigInt divisor) {

			if (divisor.IsZero)
				throw new DivideByZeroException("Cannot divide by zero");

			BigInt o = E(divisor);
			return new BigInt(_v % o._v);
		}

		protected BigInt Negate() {
			return new BigInt(-_v);
		}

		protected internal BigInt Absolute() {

			if (_v.CompareTo(0) >= 0) return Clone();

			return new BigInt(-1 * _v);
		}

		protected internal BigInt Floor() {
			// N/A, it's an integer
			return Clone();
		}

		protected internal BigInt Ceiling() {
			return Clone();
		}

		protected internal void Truncate(int significance) {

			// tempting as it is to query _v.GetInternalState, it's easier just to use ToString *cough*

			String s = _v.ToString(10);

			s = s.Substring(0, significance);

			_v = new IntX(s, 10);
			// I feel so dirty
		}

		/// <summary>Always returns a BigInt representation of the input. If the input is a BigInt it is simply returned.</summary>
		private BigInt E(BigInt other) {
			BigInt o = other as BigInt;
			if (o != null) return o;
			return (BigInt)Factory.Create(other);
		}

	}

	public class BigIntFactory {

		#region Singleton Pattern

		private static BigIntFactory _this;

		private BigIntFactory() {
		}

		public static BigIntFactory Instance {
			get {
				if (_this == null) _this = new BigIntFactory();
				return _this;
			}
		}

		#endregion

		public BigInt Create(long value) {

			return new BigInt(value);
		}

		public BigInt Create(decimal value) {
			throw new NotSupportedException();
		}

		public BigInt Create(string value) {

			return new BigInt(value);
		}

		public BigInt Create(BigInt value) {
			throw new NotImplementedException();
		}

	}
}

using System;
using System.Collections.Generic;
using System.Text;

using Oyster.Math;

namespace W3b.Sine {
	
	/// <summary>W3b.Sine-style wrapping Oyster's IntX class. Used by BigRational.</summary>
	public class BigInt : BigNum {

		//We don't want to store a list of ints
		//We want to store a list a numbers to be multiplied each defined as a power of a single base
		List<BigInt> factors = null;
		

		public List<BigInt> GetFactors() {
			if (factors == null) {
				factors = new List<BigInt>();
				//TODO: Run a sanity test to make sure this won't take forever
				IntX OrigionalNumber = _v;
				for (IntX i = 2; i < _v + 1; i++) {
					while (OrigionalNumber % i == 0) {
						factors.Add(new BigInt(i));
						OrigionalNumber /= i;
					}
				}
			} return factors;
		}
		
		private IntX _v;
		
		public BigInt(long number) {
			
			_v = new IntX( number );
			_v.Normalize();
		}
		
		public BigInt(String number) {
			
			_v = new IntX( number );
			_v.Normalize();
		}
		
		private BigInt(BigInt copyThis) {
			
			_v = new IntX( copyThis._v );
			_v.Normalize();
		}
		
		private BigInt(IntX value) {
			_v = value;
			_v.Normalize();
		}
		
		public override BigNumFactory Factory {
			get { return BigIntFactory.Instance; }
		}
		
		public override BigNum Clone() {
			
			return new BigInt( this );
		}
		
		public override int CompareTo(BigNum other) {
			
			BigInt o = E( other );
			
			return _v.CompareTo( o._v );
		}
		
		public override bool Equals(BigNum other) {
			
//			if( !other.Floor().Equals( other ) ) { // check it's an integer
//				
//				return false;
//			}
			
			BigInt o = E( other );
			
			return _v.Equals( o._v );
		}
		
		public override int GetHashCode() {
			return _v.GetHashCode();
		}
		
		public override String ToString() {
			return _v.ToString();
		}
		
		public override bool IsZero {
			get { return _v.Equals( 0 ); }
		}
		
		public override bool Sign {
			get {
				return _v.CompareTo( 0 ) >= 0;
			}
		}
		
		protected override BigNum Add(BigNum other) {
			BigInt o = E(other);
			return new BigInt( _v + o._v );
		}
		
		protected override BigNum Multiply(BigNum multiplicand) {
			BigInt o = E(multiplicand);
			return new BigInt( _v * o._v );
		}
		
		protected override BigNum Divide(BigNum divisor) {
			BigInt o = E(divisor);
			return new BigInt( _v / o._v );
		}
		
		protected override BigNum Modulo(BigNum divisor) {
			
			if( divisor.IsZero )
				throw new DivideByZeroException("Cannot divide by zero");
			
			BigInt o = E(divisor);
			return new BigInt( _v % o._v );
		}
		
		protected override BigNum Negate() {
			return new BigInt( -_v );
		}
		
		protected internal override BigNum Absolute() {
			
			if( _v.CompareTo(0) >= 0 ) return Clone();
			
			return new BigInt( -1 * _v );
		}
		
		protected internal override BigNum Floor() {
			// N/A, it's an integer
			return Clone();
		}
		
		protected internal override BigNum Ceiling() {
			return Clone();
		}
		
		protected internal override void Truncate(int significance) {
			
			// tempting as it is to query _v.GetInternalState, it's easier just to use ToString *cough*
			
			String s = _v.ToString( 10 );
			
			s = s.Substring(0, significance);
			
			_v = new IntX( s, 10 );
			// I feel so dirty
		}
		
		/// <summary>Always returns a BigInt representation of the input. If the input is a BigInt it is simply returned.</summary>
		private BigInt E(BigNum other) {
			BigInt o = other as BigInt;
			if( o != null ) return o;
			return (BigInt)Factory.Create( other );
		}
		
	}
}

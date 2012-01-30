using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Common {
	public class NumericalValue {
		enum valueType { integer, real, irrational, imaginary };
		enum structureType { singleValue, vector, matrix };

		private List<Complex32> value = new List<Complex32>();
		//private valueType valueType;

		public bool Overflow = false;
		public NumericalValue(string val) {
			this.value.Add(new Complex32(float.Parse(val), 0f));
			//TODO: set value type here


			this.val = decimal.Parse(val);
		}

		public NumericalValue(decimal num) {
			this.val = num;
		}
		public NumericalValue(double num) {
			this.val = (decimal)num;
		}

		public decimal AsDecimal() {
			return val;
		}
		public double AsDouble() {
			return (double)val;
		}
		decimal val;
		public override string ToString() {
			return this.val.ToString();
		}

	}
	///Handle negative signs, implicit multiplication
	///last value operations

	static class ValueExtensionMethods {
		public static ValueType GetType(this List<Complex32> val){
			throw new NotImplementedException();
		}
	}
}

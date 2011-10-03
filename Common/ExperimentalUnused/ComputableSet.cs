using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common{
	public abstract class ComputableSet {
		public abstract IEnumerable<ComputableSet> GetEnumerator();

		public abstract Rational Times(Rational val);

		public ProductSet Times(ComputableSet val) {
			return new ProductSet(val, this);
		}

	}

	public class ProductSet : ComputableSet {
		IEnumerable<ComputableSet> values;

		public ProductSet(ComputableSet val1, ComputableSet val2) {
			values.ToList().Add(val1);
			values.ToList().Add(val2);
		}
		public Rational Evaluate() {
			Rational product = new Rational();
			foreach (ComputableSet s in values) {
				product = s.Times(product);
			}
			//return the rational returned from multiplying all the values in the set
			throw new NotImplementedException();
		}
		public override IEnumerable<ComputableSet> GetEnumerator() {
			return values;
		}
		public override Rational Times(Rational val) {
			values.ToList().Add(val);
			return new Rational(this, val.denominator); 
		}
	}

	public class Rational : ComputableSet{
		public ComputableSet numerator, denominator;

		public override Rational Times(Rational val) {
			return new Rational(this.numerator.Times(val.numerator), this.denominator.Times(val.denominator));
		}

		public Rational(ComputableSet num, ComputableSet den) {
			this.numerator = num;
			this.denominator = den;
		}

		public Rational() { }
		public override IEnumerable<ComputableSet> GetEnumerator() {
			yield return numerator;
			yield return denominator;
		}
	}
}

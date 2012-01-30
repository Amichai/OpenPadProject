using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public class MyRandom {
		double mult = 1664525;
		double add = 1013904223;
		double m = 2e32;
		double rn = DateTime.Now.Second;
		public double NextDouble() {
			rn = Math.Abs(rn * mult + add) % m;
			return rn / m;
		}
	}
}

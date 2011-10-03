using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Common {
	[DebuggerDisplay("{(char)ToChar()}")]
	public class Digit : IEquatable<Digit> {
		public bool Equals(Digit b) {
			if (this.bitRep[0] == b.bitRep[0] &&
				this.bitRep[1] == b.bitRep[1] &&
				this.bitRep[2] == b.bitRep[2] &&
				this.bitRep[3] == b.bitRep[3])
				return true;
			else return false;
		}

		public static bool operator ==(Digit a, Digit b) {
			if (a.Equals(b))
				return true;
			else return
				false;
		}
		public static bool operator !=(Digit a, Digit b) {
			return !(a == b);
		}
		
		public char ToChar() {
			if (this == Zero)
				return '0';
			if (this == One)
				return '1';
			if (this == Two)
				return '2';
			if (this == Three)
				return '3';
			if (this== Four)
				return '4';
			if (this == Five)
				return '5';
			if (this == Six)
				return '6';
			if (this == Seven)
				return '7';
			if (this == Eight)
				return '8';
			if (this == Nine)
				return '9';
			throw new Exception();
		}
		#region Static Values
		public static Digit Zero = new Digit(false, false, false, false);
		public static Digit One = new Digit(false, false, false, true);
		public static Digit Two = new Digit(false, false, true, false);
		public static Digit Three = new Digit(false, false, true, true);
		public static Digit Four = new Digit(false, true, false, false);
		public static Digit Five = new Digit(false, true, false, true);
		public static Digit Six = new Digit(false, true, true, false);
		public static Digit Seven = new Digit(false, true, true, true);
		public static Digit Eight = new Digit(true, false, false, false);
		public static Digit Nine = new Digit(true, false, false, true);
		#endregion
		public Digit(bool p1, bool p2, bool p3, bool p4) {
			bitRep[3] = p1; bitRep[2] = p2; bitRep[1] = p3; bitRep[0] = p4;
		}
		public Digit(bool[] newDig) {
			this.bitRep = newDig;
		}
		public static Digit CharToDig(char c) {
			switch (c) {
				case '0': return Zero;
				case '1': return One;
				case '2': return Two;
				case '3': return Three;
				case '4': return Four;
				case '5': return Five;
				case '6': return Six;
				case '7': return Seven;
				case '8': return Eight;
				case '9': return Nine;
			}
			throw new Exception();
		}
		[DebuggerDisplay("{bitRep[3].ToString() + bitRep[2].ToString() + bitRep[1].ToString() + bitRep[0].ToString()}")]
		bool[] bitRep = new bool[4];

		public static Digit[] operator *(Digit a, Digit b) {
			return Multiply(a, b);
		}

		public static bool operator <(Digit a, Digit b) {
			for (int i = 3; i >= 0; i--) {
				if (!a.bitRep[i] && b.bitRep[i]) {
					return true;
				}
				if (a.bitRep[i] && !b.bitRep[i]) {
					return false;
				}
			}
			return false;
		}
		
		public static bool operator >(Digit a, Digit b) {
			for (int i = 3; i >= 0; i--) {
				if (a.bitRep[i] && !b.bitRep[i]) {
					return true;
				}
				if (!a.bitRep[i] && b.bitRep[i]) {
					return false;
				}
			}
			return false;
		}

		public Tuple<Digit, bool> Subtract(Digit b) {
			if (b > this) {
				return new Tuple<Digit, bool>((b - this).Item1, false);
			}

			bool[] newDig = new bool[4];
			bool borrowed;
			bool sign = true;
			bool[] adjustedFirstDig = new bool[4];
			for (int i = 0; i < 4; i++) {
				adjustedFirstDig[i] = this.bitRep[i];
			}
			for (int i = 0; i < 4; i++) {
				borrowed = false;
				//1 - 1 = 0
				if (adjustedFirstDig[i] && b.bitRep[i]) {
					newDig[i] = false;
				}
				//0 - 0 = 0 
				if (!adjustedFirstDig[i] && !b.bitRep[i]) {
					newDig[i] = false;
				}
				//1 - 0 = 1
				if (adjustedFirstDig[i] && !b.bitRep[i]) {
					newDig[i] = true;
				}
				//0 - 1 = 1
				if (!adjustedFirstDig[i] && b.bitRep[i]) {
					borrowed = true;
					newDig[i] = true;
				}

				if (borrowed) {
					//Find the index with a one to borrow from
					int idxToBorrowFrom = int.MinValue;
					for (int j = i + 1; j < 4; j++) {
						if (j < 4 && this.bitRep[j] == true) {
							idxToBorrowFrom = j;
							j = 4;
						}
					}
					if (idxToBorrowFrom != int.MinValue) {
						for (int j = i; j < idxToBorrowFrom; j++) {
							adjustedFirstDig[j] = true;
						}
						adjustedFirstDig[idxToBorrowFrom] = false;
					}
				}
			}
			Digit dig = new Digit(newDig);
			return new Tuple<Digit, bool>(dig, sign);
		}

		public static Tuple<Digit, bool> operator -(Digit a, Digit b) {
			return a.Subtract(b);
		}

		public static Digit[] operator +(Digit a, Digit b) {
			bool[] newDig = new bool[4];
			bool[] carriedOnes = new bool[5];
			for (int i = 0; i < 4; i++) {
				if (a.bitRep[i] && b.bitRep[i]) {
					carriedOnes[i + 1] = true;
					newDig[i] = carriedOnes[i];
				} else if (a.bitRep[i] || b.bitRep[i]) {
					if (!carriedOnes[i]) {
						newDig[i] = true;
					} else if (carriedOnes[i]) {
						newDig[i] = false;
						carriedOnes[i + 1] = true;
					}
				} else if (a.bitRep[i] == false && b.bitRep[i] == false) {
					if (!carriedOnes[i])
						newDig[i] = false;
					if (carriedOnes[i]) {
						newDig[i] = true;
					}
				}
			}
			Digit[] bothDigits = new Digit[2];
			bothDigits[1] = new Digit(newDig);
			if (carriedOnes[4]) {
				bothDigits[0] = Digit.One;
				bothDigits[1] = (bothDigits[1] + Digit.Six)[1];
			} else {
				bothDigits[0] = Digit.Zero;
				if (bothDigits[1].bitRep[3] && bothDigits[1].bitRep[1]) {
					bothDigits[0] = Digit.One;
					bothDigits[1].bitRep[3] = false;
					bothDigits[1].bitRep[1] = false;
				} else if (bothDigits[1].bitRep[3] && bothDigits[1].bitRep[2]) {
					bothDigits[0] = Digit.One;
					bothDigits[1].bitRep[3] = false;
					bothDigits[1].bitRep[2] = false;
					bothDigits[1].bitRep[1] = true;
				}
			}
			return bothDigits;
		}

		public Digit TenMinus() {
			if (this == One)
				return Nine;
			if (this == Two)
				return Eight;
			if (this == Three)
				return Seven;
			if (this == Four)
				return Six;
			if (this == Five)
				return Five;
			if (this == Six)
				return Four;
			if (this == Seven)
				return Three;
			if (this == Eight)
				return Two;
			if (this == Nine)
				return One;
			throw new Exception();
		}

		static Dictionary<Digit, Dictionary<Digit, Digit[]>> multiplicationTable = null;
		public static Digit[] Multiply(Digit a, Digit b) {
			Digit _0 = Digit.Zero;
			Digit _1 = Digit.One;
			Digit _2 = Digit.Two;
			Digit _3 = Digit.Three;
			Digit _4 = Digit.Four;
			Digit _5 = Digit.Five;
			Digit _6 = Digit.Six;
			Digit _7 = Digit.Seven;
			Digit _8 = Digit.Eight;
			Digit _9 = Digit.Nine;

			if (multiplicationTable != null)
				return multiplicationTable[a][b];

			multiplicationTable = new Dictionary<Digit, Dictionary<Digit, Digit[]>>() {
					{ _0, new Dictionary<Digit, Digit[]>() 				   {{_0, new Digit[]{_0, _0}},
																			{_1, new Digit[]{_0, _0}},
																			{_2, new Digit[]{_0, _0}},
																			{_3, new Digit[]{_0, _0}},
																			{_4, new Digit[]{_0, _0}},
																			{_5, new Digit[]{_0, _0}},
																			{_6, new Digit[]{_0, _0}},
																			{_7, new Digit[]{_0, _0}},
																			{_8, new Digit[]{_0, _0}},
																			{_9, new Digit[]{_0, _0}},}},
					{ _1, new Dictionary<Digit, Digit[]>() 				   {{_0, new Digit[]{_0, _0}},
																			{_1, new Digit[]{_0, _1}},
																			{_2, new Digit[]{_0, _2}},
																			{_3, new Digit[]{_0, _3}},
																			{_4, new Digit[]{_0, _4}},
																			{_5, new Digit[]{_0, _5}},
																			{_6, new Digit[]{_0, _6}},
																			{_7, new Digit[]{_0, _7}},
																			{_8, new Digit[]{_0, _8}},
																			{_9, new Digit[]{_0, _9}},}},
					{ _2, new Dictionary<Digit, Digit[]>() 				   {{_0, new Digit[]{_0, _0}},
																			{_1, new Digit[]{_0, _2}},
																			{_2, new Digit[]{_0, _4}},
																			{_3, new Digit[]{_0, _6}},
																			{_4, new Digit[]{_0, _8}},
																			{_5, new Digit[]{_1, _0}},
																			{_6, new Digit[]{_1, _2}},
																			{_7, new Digit[]{_1, _4}},
																			{_8, new Digit[]{_1, _6}},
																			{_9, new Digit[]{_1, _8}},}},
					{ _3, new Dictionary<Digit, Digit[]>() 				   {{_0, new Digit[]{_0, _0}},
																			{_1, new Digit[]{_0, _3}},
																			{_2, new Digit[]{_0, _6}},
																			{_3, new Digit[]{_0, _9}},
																			{_4, new Digit[]{_1, _2}},
																			{_5, new Digit[]{_1, _5}},
																			{_6, new Digit[]{_1, _8}},
																			{_7, new Digit[]{_2, _1}},
																			{_8, new Digit[]{_2, _4}},
																			{_9, new Digit[]{_2, _7}},}},
					{ _4, new Dictionary<Digit, Digit[]>() 				   {{_0, new Digit[]{_0, _0}},
																			{_1, new Digit[]{_0, _4}},
																			{_2, new Digit[]{_0, _8}},
																			{_3, new Digit[]{_1, _2}},
																			{_4, new Digit[]{_1, _6}},
																			{_5, new Digit[]{_2, _0}},
																			{_6, new Digit[]{_2, _4}},
																			{_7, new Digit[]{_2, _8}},
																			{_8, new Digit[]{_3, _2}},
																			{_9, new Digit[]{_3, _6}},}},
					{ _5, new Dictionary<Digit, Digit[]>() 				   {{_0, new Digit[]{_0, _0}},
																			{_1, new Digit[]{_0, _5}},
																			{_2, new Digit[]{_1, _0}},
																			{_3, new Digit[]{_1, _5}},
																			{_4, new Digit[]{_2, _0}},
																			{_5, new Digit[]{_2, _5}},
																			{_6, new Digit[]{_3, _0}},
																			{_7, new Digit[]{_3, _5}},
																			{_8, new Digit[]{_4, _0}},
																			{_9, new Digit[]{_4, _5}},}},
					{ _6, new Dictionary<Digit, Digit[]>() 				   {{_0, new Digit[]{_0, _0}},
																			{_1, new Digit[]{_0, _6}},
																			{_2, new Digit[]{_1, _2}},
																			{_3, new Digit[]{_1, _8}},
																			{_4, new Digit[]{_2, _4}},
																			{_5, new Digit[]{_3, _0}},
																			{_6, new Digit[]{_3, _6}},
																			{_7, new Digit[]{_4, _2}},
																			{_8, new Digit[]{_4, _8}},
																			{_9, new Digit[]{_5, _4}},}},
					{ _7, new Dictionary<Digit, Digit[]>() 				   {{_0, new Digit[]{_0, _0}},
																			{_1, new Digit[]{_0, _7}},
																			{_2, new Digit[]{_1, _4}},
																			{_3, new Digit[]{_2, _1}},
																			{_4, new Digit[]{_2, _8}},
																			{_5, new Digit[]{_3, _5}},
																			{_6, new Digit[]{_4, _2}},
																			{_7, new Digit[]{_4, _9}},
																			{_8, new Digit[]{_5, _6}},
																			{_9, new Digit[]{_6, _3}},}},
					{ _8, new Dictionary<Digit, Digit[]>() 				   {{_0, new Digit[]{_0, _0}},
																			{_1, new Digit[]{_0, _8}},
																			{_2, new Digit[]{_1, _6}},
																			{_3, new Digit[]{_2, _4}},
																			{_4, new Digit[]{_3, _2}},
																			{_5, new Digit[]{_4, _0}},
																			{_6, new Digit[]{_4, _8}},
																			{_7, new Digit[]{_5, _6}},
																			{_8, new Digit[]{_6, _4}},
																			{_9, new Digit[]{_7, _2}},}},
					{ _9, new Dictionary<Digit, Digit[]>() 				   {{_0, new Digit[]{_0, _0}},
																			{_1, new Digit[]{_0, _9}},
																			{_2, new Digit[]{_1, _8}},
																			{_3, new Digit[]{_2, _7}},
																			{_4, new Digit[]{_3, _6}},
																			{_5, new Digit[]{_4, _5}},
																			{_6, new Digit[]{_5, _4}},
																			{_7, new Digit[]{_6, _3}},
																			{_8, new Digit[]{_7, _2}},
																			{_9, new Digit[]{_8, _1}},}},
				};
			return multiplicationTable[a][b];
		}

	}
}

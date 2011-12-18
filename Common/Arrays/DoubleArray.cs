using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Common {
	public class DoubleArray {
		private int scaling(int val) {
			var scaledVal = (int)(Math.Pow((double)val / (double)maxValue, .4)*255);
			return scaledVal;
		}

		public Bitmap ToBitmap() {
			Func<int, int> scalingFunction = new Func<int, int>(scaling);
			return board.ConvertDoubleArrayToBitmap(Color.White, scalingFunction);
		}
		int maxValue = 0;
		List<List<int>> board = new List<List<int>>();
		int xMid, yMid, sideLength;
		public DoubleArray(int sideLength) {
			this.sideLength = sideLength;
			xMid = sideLength / 2;
			yMid = sideLength / 2;
			for (int i = 0; i < sideLength; i++) {
				board.Add(new List<int>());
				for (int j = 0; j < sideLength; j++) {
					board[i].Add(0);
				}
			}
		}
		public void IncrementAt(int x, int y) {
			if (x + xMid < sideLength && y + yMid < sideLength && x + xMid >= 0&& y + yMid >= 0) {
				int pxlVal = ++board[x + xMid][y + yMid];
				if (pxlVal > maxValue) {
					maxValue = pxlVal;
				}
			}
		}
	}
}

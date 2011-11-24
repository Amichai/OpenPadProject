using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Common {
	public class DoubleArray {
		public Bitmap ToBitmap() {
			return board.ConvertDoubleArrayToBitmap(Color.White);
		}
		List<List<int>> board = new List<List<int>>();
		int xMid, yMid;
		public DoubleArray(int sideLength) {
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
			try {
				board[x + xMid][y + yMid]++;
			} catch { 
			}
			//TODO: eliminate this catch statement
		}
	}
}

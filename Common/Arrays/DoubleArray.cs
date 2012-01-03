using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Common {
	public class DoubleArray<T> {
		List<List<T>> board = new List<List<T>>();
		int xMid, yMid;
		public int SideLength;

		public Bitmap ToBitmap(Color Color1, Color Color2) {
			int width = board.Count();
			int height = board[0].Count();
			Bitmap bitmapReturn = new Bitmap(width, height);
			Color pixelColor;
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					if (board[i][j].ToString() == "True")
						pixelColor = Color1;
					else pixelColor = Color2;
					bitmapReturn.SetPixel(i, j, pixelColor);
				}
			}
			return bitmapReturn;
		}

		public DoubleArray(int sideLength, T initialVal) {
			this.SideLength = sideLength;
			xMid = sideLength / 2;
			yMid = sideLength / 2;
			for (int i = 0; i < sideLength; i++) {
				board.Add(new List<T>());
				for (int j = 0; j < sideLength; j++) {
					board[i].Add(initialVal);
				}
			}
		}

		/// <summary>Periodic boundaries!!
		/// </summary>
		public List<T> this[int i] {
			get {
				if (i >= SideLength) 
					return board[i - SideLength];
				else if(i < 0)
					return board[i + SideLength];
				else return board[i];
			}
			set {
				if (i >= SideLength)
					board[i - SideLength] = value;
				if(i < 0)
					board[i + SideLength] = value;
				else board[i] = value;
			}
		}

		public void SetVal(int x, int y, T val){
			board[x][y] = val;
		}

	}

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

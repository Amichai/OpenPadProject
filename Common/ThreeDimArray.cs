using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public class ThreeDimArray {
		List<DoubleArray> allBoards = new List<DoubleArray>();
		int boardSize;
		public ThreeDimArray(int numberOfBoards, int boardSize) {
			this.boardSize = boardSize;
			for (int i = 0; i < numberOfBoards; i++) {
				allBoards.Add(new DoubleArray(boardSize));
			}
		}
		public void IncrementPixel(int x, int y, int z){
			allBoards[z].IncrementAt(x, y);
		}
		public void SaveToDisk() {
			for(int i=0; i < allBoards.Count();i++){
				DoubleArray mat = allBoards[i];
				mat.ToBitmap().Save("test" + i.ToString() + ".bmp");
			}
		}
	}
}

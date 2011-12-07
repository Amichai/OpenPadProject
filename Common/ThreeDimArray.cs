using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Common {
	public class ThreeDimArray {
		List<DoubleArray> allBoards = new List<DoubleArray>();
		int boardSize;
		int scaledStepSize;
		public ThreeDimArray(int numberOfBoards, int boardSize, int scaledStepSize) {
			this.boardSize = boardSize;
			this.scaledStepSize = scaledStepSize;
			for (int i = 0; i < numberOfBoards; i++) {
				allBoards.Add(new DoubleArray(boardSize));
			}
		}
		public void IncrementPixel(int x, int y, int z){
			allBoards[z].IncrementAt(x, y);
		}
		public void SaveToDisk() {
			//TODO: Make a new folder each time to save these images. Add a text file with info about: step size, steps taken, expansion coef, etc.
			for(int i=0; i < allBoards.Count() - 1;i++){
				DoubleArray mat = allBoards[i];
				Image bit = mat.ToBitmap() as Image;
				Graphics g = Graphics.FromImage(bit);
				int midPoint = boardSize / 2;
				g.DrawLine(new Pen(Color.Red, 1), new Point(midPoint, midPoint), new Point(midPoint + scaledStepSize, midPoint ));
				bit.Save("test" + i.ToString() + ".bmp");
			}
		}
	}
}

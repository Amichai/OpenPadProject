using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Common {
	public class UploadedImage {
		private Bitmap asBitmap;
		int[][] imageData = null;
		public int Width, Height;
		public UploadedImage(string filename) {
			asBitmap = Bitmap.FromFile(filename) as Bitmap;
			this.Width = asBitmap.Width;
			this.Height = asBitmap.Height;
		}

		public int[] this[int i] {
			get {return GetDoubleArray()[i];}
			set {GetDoubleArray()[i] = value;}
		}

		public UploadedImage(string filename, Rectangle crop) {
			asBitmap = Bitmap.FromFile(filename) as Bitmap;
			asBitmap = asBitmap.CropBitmap(crop);
			this.Width = asBitmap.Width;
			this.Height = asBitmap.Height;
		}

		public int[][] GetDoubleArray() {
			if(imageData == null)
				imageData = GetBitmap().BitmapToDoubleArray(".bmp", 0);
			return imageData;
		}

		public Bitmap GetBitmap() {
			return asBitmap;
		}
	}
}

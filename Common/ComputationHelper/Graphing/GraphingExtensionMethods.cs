using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace Common{
	public static class GraphingExtensionMethods {
		public static void Graph(this Series ser){
			if (ser == null)
				throw new NullReferenceException();
			new Graph(ser).ShowDialog();
		}

		public static void Graph(this Series ser, int numberOfItemsToGraph) {
			if (ser == null)
				throw new NullReferenceException();

			var ser2 = new Series();
			for(int i=0;i < numberOfItemsToGraph; i++){
				ser2.Points.AddXY(ser.Points[i].XValue, ser.Points[i].YValues.First());
			}
			new Graph(ser2).ShowDialog();
		}

		public static void Graph(this List<double> vals) {
			Series ser = new Series();
			for (int i = 0; i < vals.Count; i++) {
				ser.Points.AddXY(i + 1, vals[i]);
			}
			new Graph(ser).ShowDialog();
		}
	}
}

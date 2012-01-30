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
	}
}

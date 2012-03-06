using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace Common {
	public partial class Graph : Form {
		public Graph(Series p) {
			InitializeComponent(new PlotData(p));
		}
		public Graph(PlotData p) {
			InitializeComponent(p);
		}

		public Graph(Func<double, Series> variableParameter) {
			throw new NotImplementedException();
		}
		//TODO: Add an overload that implies a variable control which will edit and reload the graph
	}
}

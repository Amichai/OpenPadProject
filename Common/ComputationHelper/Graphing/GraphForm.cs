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
	public partial class Chart2 : Form {
		public Chart2(Series p) {
			InitializeComponent(new PlotData(p));
		}
		public Chart2(PlotData p) {
			InitializeComponent(p);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public class StatusReport {
		public int ProgressValue;
		public double CompletionPercentage;
		public object LastStep;
		public HashSet<string> Tags = new HashSet<string>();
		public StatusReport(int progressVal, object lastStep) {
			this.ProgressValue = progressVal;
			this.LastStep = lastStep;
		}
		public StatusReport(int progressVal, object lastStep, params string[] tags) {
			this.ProgressValue = progressVal;
			this.LastStep = lastStep;
			foreach(string s in tags)
			this.Tags.Add(s);
		}

		public string ToString() {
			string output = string.Empty;
			output += "Progress value: " + ProgressValue.ToString() + " Tags: \n";
			foreach (string s in Tags) {
				output += s;
				output += "\n";
			}
			return  output;
		}
	}
}

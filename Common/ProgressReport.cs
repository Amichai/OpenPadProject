using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public class StatusReport {
		public int ProgressValue;
		public double CompletionPercentage;
		public object LastStep;
		public StatusReport(int progressVal, object lastStep) {
			this.ProgressValue = progressVal;
			this.LastStep = lastStep;
		}
	}
}

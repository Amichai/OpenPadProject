using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageHandling {
	public class ReturnMessage {
		public bool Success = false;
		public string Message = string.Empty;
		public ReturnMessage(string message) {
			this.Message = message;
		}
		public ReturnMessage(bool success) {
			Success = success;
		}
		public ReturnMessage(bool success, object returnVal) {
			Success = success;
			ReturnValue = returnVal;
		}
		public object ReturnValue;
	}
}

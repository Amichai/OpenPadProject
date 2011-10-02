using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SystemValues;

namespace LoggingManager {
	public static class SystemLog {
		private static List<systemLogObject> allObjects = new List<systemLogObject>();
		public static void Add(object obj, LogObjectType type) {
			allObjects.Add(new systemLogObject(obj, type));
		}

		public static NumericalValue GetLastNumericalValue() {
			var query = allObjects.Where(i => i.Type == LogObjectType.value).LastOrDefault().ObjectToLog as NumericalValue;
			if (query != null)
				return query;
			else
				throw new Exception("No results found");
		}
	}
	public enum LogObjectType { value, failureMessage };

	class systemLogObject {
		private static Stopwatch stopwatch = Stopwatch.StartNew();
		DateTime time;
		public object ObjectToLog;
		public LogObjectType Type;
		long elapsedTicks;
		public systemLogObject(object objToLog, LogObjectType type) {
			this.ObjectToLog = objToLog;
			this.Type = type;
			this.time = DateTime.Now;
			this.elapsedTicks = stopwatch.ElapsedTicks;
		}
	}
}

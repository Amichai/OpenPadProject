using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common {
	public static class SystemLog {
		[Serializable()]
		private class serializeMe {
			//object serializeMe = null;
			public serializeMe(object me) {
				//this.serializeMe = me;
				throw new NotImplementedException();
			}
		}
		public static void SerializeAndSave(this object serializeMe) {
			var ser = new serializeMe(serializeMe);
			Stream stream = File.Open("EmployeeInfo.osl", FileMode.Create);
			BinaryFormatter bformatter = new BinaryFormatter();
			bformatter.Serialize(stream, ser);
			stream.Close();
		}

		public static object OpenSavedObject() {
			object mp = null;

			//Open the file written above and read values from it.
			Stream stream = File.Open("EmployeeInfo.osl", FileMode.Open);
			BinaryFormatter bformatter = new BinaryFormatter();

			Console.WriteLine("Reading Employee Information");
			mp = bformatter.Deserialize(stream);
			stream.Close();
			return mp;
		}

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

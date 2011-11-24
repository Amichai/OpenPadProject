using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
	public static class LoggingManager {
		static Dictionary<Guid, object> allObjects = new Dictionary<Guid, object>();
		static Dictionary<string, List<Guid>> objectLog = new Dictionary<string, List<Guid>>();

		public static void Add(this object objectToAdd, string[] labels){
			var id = Guid.NewGuid();
			allObjects.Add(id, objectToAdd);
			foreach (string lbl in labels) {
				objectLog[lbl].Add(id);
			}
		}

		public static void Save(string filename) {
			throw new NotImplementedException();
		}
		//Visualize the log
		//Create an easy data dump
		//Open and close option
	}
}

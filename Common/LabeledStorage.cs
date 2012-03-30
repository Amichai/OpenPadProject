using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common {
	public class LabeledStorage<T> : INotifyPropertyChanged {
		public void Add(T obj, params string[] labels) {
			Guid labelID, objectID;
			foreach(var label in labels){
				objectID = getObjId(obj);
				labelID = getLabelID(label);
				if (!GuidRelationships.ContainsKey(objectID)) {
					GuidRelationships.Add(objectID, new HashSet<Guid>() { labelID });
				} else {
					if (!GuidRelationships[objectID].Contains(labelID)) {
						GuidRelationships[objectID].Add(labelID);
					} else throw new Exception();
				}
				if (!GuidRelationships.ContainsKey(labelID)) {
					GuidRelationships.Add(labelID, new HashSet<Guid>() { objectID });
				} else {
					if (!GuidRelationships[labelID].Contains(objectID)) {
						GuidRelationships[labelID].Add(objectID);
					} else throw new Exception();
				}
			}
		}

		public IEnumerable<T> Enteries() {
			return allObjects.Keys;
		}

		private Guid getObjId(T obj) {
			if (allObjects.ContainsKey(obj)) {
				return allObjects[obj];
			} else {
				Guid id = Guid.NewGuid();
				allObjects.Add(obj, id);
				return id;
			}
		}

		private Guid getLabelID(string label) {
			if (allLabels.ContainsKey(label)) {
				return allLabels[label];
			} else {
				Guid id = Guid.NewGuid();
				allLabels.Add(label, id);
				return id;
			}
		}

		TwoWayDictionary<T, Guid> allObjects = new TwoWayDictionary<T, Guid>();
		TwoWayDictionary<string, Guid> allLabels = new TwoWayDictionary<string, Guid>();
		/// <summary>The key is for the object, the Enumerable is for the list of labels</summary>
		Dictionary<Guid, HashSet<Guid>> GuidRelationships = new Dictionary<Guid, HashSet<Guid>>();

		public List<T> LookupLabel(string label) {
			List<T> returnMe = new List<T>();
			foreach (var a in GuidRelationships[allLabels[label]]) {
				returnMe.Add(allObjects.LookupByValue(a));
			}
			return returnMe;
		}
		
		public List<string> LookupObject(T obj) {
			var a = allObjects[obj];
			var b = GuidRelationships[a];
			List<string> labels = new List<string>();
			foreach (Guid c in b) {
				labels.Add(allLabels.LookupByValue(c));
			}
			return labels;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(string info) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}

		private string _name;

		public string Name {
			get { return _name; }
			set {
				_name = value;
				NotifyPropertyChanged("Name");
			}
		}
	}


	public class TwoWayDictionary<T1, T2> : Dictionary<T1, T2> {
		public T1 LookupByValue(T2 obj) {
			return reverseLookup[obj];
		}
		Dictionary<T2, T1> reverseLookup = new Dictionary<T2, T1>();
		public new void Add(T1 obj1, T2 obj2) {
			if(!reverseLookup.ContainsValue(obj1)){
				reverseLookup.Add(obj2, obj1);
				base.Add(obj1, obj2);
			}
		}
	}
}

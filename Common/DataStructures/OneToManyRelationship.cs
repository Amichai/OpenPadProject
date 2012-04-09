using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Common {
	//TODO: replace GUID with a custom lightweight version and
	//have data structure keep track of the largest counter values.
	public class OneToManyRelationship<T1, T2> {
		public bool Add(T1 key) {
			Guid keyID = getKeyId(key);
			if (GuidRelationships.ContainsKey(keyID)){
				//Do absolutely nothing, we already have an object at this address
				return false;
			} else {
				GuidRelationships.Add(keyID, new HashSet<Guid>());
				return true;
			}
		}

		public void Add(T1 key, params T2[] values) {
			Guid valueID, keyID;
			foreach (var value in values) {
				keyID = getKeyId(key);
				valueID = getValueID(value);
				if (!GuidRelationships.ContainsKey(keyID)) {
					GuidRelationships.Add(keyID, new HashSet<Guid>() { valueID });
				} else {
					if (!GuidRelationships[keyID].Contains(valueID)) {
						GuidRelationships[keyID].Add(valueID);
					} else throw new Exception();
				}
				if (!GuidRelationships.ContainsKey(valueID)) {
					GuidRelationships.Add(valueID, new HashSet<Guid>() { keyID });
				} else {
					if (!GuidRelationships[valueID].Contains(keyID)) {
						GuidRelationships[valueID].Add(keyID);
					} else throw new Exception();
				}
			}
		}

		public IEnumerable<T1> Keys() {
			return allKeys.Keys;
		}

		/// <summary>
		/// Tests the object address against known address to determine newness.</summary>
		private Guid getKeyId(T1 key) {
			if (allKeys.ContainsKey(key)) {
				return allKeys[key];
			} else {
				Guid id = Guid.NewGuid();
				allKeys.Add(key, id);
				return id;
			}
		}

		private Guid getValueID(T2 value) {
			if (allvalues.ContainsKey(value)) {
				return allvalues[value];
			} else {
				Guid id = Guid.NewGuid();
				allvalues.Add(value, id);
				return id;
			}
		}

		TwoWayDictionary<T1, Guid> allKeys = new TwoWayDictionary<T1, Guid>();
		TwoWayDictionary<T2, Guid> allvalues = new TwoWayDictionary<T2, Guid>();
		/// <summary>The key is for the Key, the Enumerable is for the list of values</summary>
		Dictionary<Guid, HashSet<Guid>> GuidRelationships = new Dictionary<Guid, HashSet<Guid>>();

		public List<T1> LookupValue(T2 value) {
			if (!allvalues.ContainsKey(value))
				throw new Exception();
			List<T1> returnMe = new List<T1>();
			foreach (var a in GuidRelationships[allvalues[value]]) {
				returnMe.Add(allKeys.LookupByValue(a));
			}
			return returnMe;
		}

		public List<T2> LookupKey(T1 key) {
			var a = allKeys[key];
			var b = GuidRelationships[a];
			List<T2> values = new List<T2>();
			foreach (Guid c in b) {
				values.Add(allvalues.LookupByValue(c));
			}
			return values;
		}

		public void Visualize() {
			int size = allKeys.Keys.Count() * 5;
			Bitmap b = new Bitmap(size, size);
			Graphics g = Graphics.FromImage(b);
			foreach (var key in allKeys.Keys) {
				
			}
		}

		public List<T2> this[T1 i]{
			get{
				return LookupKey(i);
			}
			set {
				Add(i);
			}		
		}
	}
}

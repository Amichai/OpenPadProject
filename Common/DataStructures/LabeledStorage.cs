using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common {
	public class LabeledStorage<T> : OneToManyRelationship<T, string> {
	}
}

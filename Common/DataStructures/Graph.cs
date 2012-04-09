using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DataStructure{
	public class Graph {
		public class Node{
			public Node() {
				this.id = Guid.NewGuid();
			}
			public Node(string name) {
				this.Label = name;
				this.id = Guid.NewGuid();
				LookupByLabel.Add(name, this);
			}
			public double Value { get; set; }
			public string Label { get; set; }
			Guid id { get; set; }

			public override bool Equals(object obj) {
				return this.id == ((Node)obj).id;
			}

			public static bool operator ==(Node a, Node b) {
				return a.id == b.id;
			}

			public static bool operator !=(Node a, Node b) {
				return a.id != b.id;
			}
			public override int GetHashCode() {
				return this.id.GetHashCode();
			}

			public static Dictionary<string, Node> LookupByLabel = new Dictionary<string, Node>();

			public void SetValue(double val) {
				this.Value = val;
			}
		}

		class Edge {
			public Edge(double connectionStrength, Node node) {
				this.Node = node;
				this.ConnectionStrength = connectionStrength;
			}
			public double ConnectionStrength;
			public Node Node;
		}
		
		void propagateExcitation(double excitationVal) {
			
		}

		OneToManyRelationship<Node, Edge> graph = new OneToManyRelationship<Node, Edge>();

		static Random rand = new Random();

		public void AddFullyConnectedNode() {
			if (graph.Keys().Count() == 0)
				graph.Add(new Node());
			else {
				var list = graph.Keys().ToList();
				var node = new Node();
				for(int i=0; i < list.Count(); i++){
					var a = list[i];
					graph.Add(node, new Edge(rand.NextDouble(), a));
				}
			}
		}

		public void FullyConnectedGraph(int numberOfNodes) {
			for (int i = 0; i < numberOfNodes;i ++ ) {
				graph.Add(new Node(i.ToString()));
			}
			connectAllNodes();
		}

		public static int ExcitationCounter = 0;

		public string PrintAllNodes() {
			string output = string.Empty;
			foreach (var a in graph.Keys()) {
				output += "Label: " + a.Label;
				output += " value: " + a.Value;
				output += "\n";
			}
			return output;
		}

		public void ExiteNode(string label, double val) {
			ExcitationCounter++;
			var node = Node.LookupByLabel[label];
			node.SetValue(val);
			foreach (var a in graph[node]) {
				var c = a.ConnectionStrength * val;
				if (c > 1.2) {
					ExiteNode(a.Node.Label, c);
				}
			}
		}

		private void connectAllNodes() {
			foreach (var a in graph.Keys()) {
				foreach (var b in graph.Keys()) {
					if (a != b) {
						graph.Add(a, new Edge(rand.NextDouble(), b));
					}
				}
			}
		}

		public void PropegateExcitation() {
			//Propagate all excitation values waiting to be propagated.
			throw new NotImplementedException();
		}
	}
}

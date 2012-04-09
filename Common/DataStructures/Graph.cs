using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DataStructure{
	public class Graph {
		public class Node {
			double value { get; set; }
			string label { get; set; }
		}

		class Edge {
			public Edge(double connectionStrength, Node node) {
				this.node = node;
				this.connectionStrength = connectionStrength;
			}
			double connectionStrength;
			Node node;
		}
		
		void propagateExcitation(double excitationVal) {

		}

		OneToManyRelationship<Node, Edge> graph = new OneToManyRelationship<Node, Edge>();

		public void AddFullyConnectedNode() {
			if (graph.Keys().Count() == 0)
				graph.Add(new Node());
			else {
				foreach (var a in graph.Keys()) {
					graph.Add(new Node(), new Edge(1, a));
				}
			}
		}

		public void FullyConnectedGraph(int numberOfNodes) {
			foreach (var a in graph.Keys()) {
				
			}
		}
	}
}

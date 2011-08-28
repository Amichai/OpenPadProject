using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageHandling;

namespace Compiler {
	public class ParseTree {
		Node root = new KeywordNode();
		ReturnMessage message;
		/// <summary>Adds a (post fixed)token to the ParseTree, return a message specifying if the addition was successful</summary>
		/// <param name="token">Token to add</param>
		/// <returns>Message about the success of the addition</returns>
		public ReturnMessage AddToken(Token token) {
			if (!Tokenizer.GetTokenInfo.ContainsKey(token.TokenType)) {
				throw new Exception("This token type is not possible here");
			} else switch (token.TokenType) {
				case TokenType.identifier:
					if (Functions.FunctionLookup.ContainsKey(token.TokenString.ToLower())) {
						root.AddParent(new FunctionNode(token.TokenString));
					}
					break;
				case TokenType.numberLiteral:
					double parsedVal;
					if (double.TryParse(token.TokenString, out parsedVal)) {
						root.AddChild(new NumberNode(parsedVal));
					} else return new ReturnMessage("The string: " + token.TokenString + " could not be parsed as a number.");
					break;
				case TokenType.operatorOrPunctuation:
					if (InfixOperators.GetOpInfo.ContainsKey(token.TokenString)) {
						message = root.AddParent(new TwoParameterOperatorNode(token.TokenString));
					} else return new ReturnMessage("I don't know how to append this operator/punctuation mark.");
					break;
				default:
					throw new Exception("Unknown token type");
			}
			return new ReturnMessage(true);
		}

		/// <summary>For adding a list of postfixed tokens</summary>
		public ReturnMessage AddListOfTokens(List<Token> tokensToAdd) {
			foreach (Token t in tokensToAdd) {
				message = AddToken(t);
				if (message.Success == false)
					return message;
			}
			root = root.GetChild(1);
			return new ReturnMessage(true, this);
		}

		public string Visualize() { return root.Visualize(" ", false); }
	}

	delegate void TreeVisitor(Node nodeData);
	/// <summary>Abstract Syntax Tree Data stucture node</summary>
	abstract class Node {
		public LinkedList<Node> Children = new LinkedList<Node>();
		public int NumberOfChildren;
		public void AddChild(Node node) {
			Children.AddFirst(node);
			visualization = string.Empty;
		}
		public ReturnMessage AddParent(Node node) {
			Node parent = node;
			LinkedList<Node> nodesToMove = new LinkedList<Node>();
			for (int i = 0; i < parent.NumberOfChildren; i++) {
				if (Children.Count() == 0) return new ReturnMessage("Post fixed token notation error.");
				nodesToMove.AddLast(GetChild(1));
				Children.RemoveFirst();
			}
			parent.SetValue(nodesToMove);
			AddChild(parent);
			visualization = string.Empty;
			return new ReturnMessage(true);
		}
		public Node GetChild(int i) {
			foreach (Node n in Children)
				if (--i == 0) return n;
			return null;
		}
		public void traverse(Node node, TreeVisitor visitor) {
			visitor(node);
			foreach (Node kid in node.Children)
				traverse(kid, visitor);
		}
		private static string visualization = "\n";
		public string Visualize(string indent, bool last) {
			visualization += indent;
			if (last) {
				visualization += "\\ ";
				indent += "  ";
			} else {
				visualization += "| ";
				indent += "| ";
			}
			visualization += GetValueAsString() + "\n";

			int i = 0;
			foreach (Node c in Children) {
				c.Visualize(indent, i == Children.Count - 1);
				i++;
			}
			return visualization;
		}
		public abstract string GetValueAsString();
		public abstract void SetValue(LinkedList<Node> childrenToAdd);
		public abstract object GetValue();
	}

	class NumberNode : Node {
		public NumberNode(double val) {
			this.value = val;
		}
		private double value;
		public override string GetValueAsString() {
			return this.value.ToString();
		}
		public override void SetValue(LinkedList<Node> childrenToAdd) {
			throw new Exception("Cannot add children to a number");
		}
		public override object GetValue() {
			return value;
		}
	}

	class TwoParameterOperatorNode : Node {
		double value = double.MinValue;
		public TwoParameterOperatorNode(string op) {
			this.op = op;
			NumberOfChildren = 2;
		}
		string op;
		public override string GetValueAsString() {
			if (value == double.MinValue)
				return this.op;
			else
				return this.op + " [" + value.ToString() + "]";

		}
		public override object GetValue() {
			return value;
		}
		public override void SetValue(LinkedList<Node> childrenToAdd) {
			if (childrenToAdd.Count != 2)
				throw new Exception("A two parameter operator must have two parameters");
			value = InfixOperators.GetOpInfo[this.op].Compute
				((double)childrenToAdd.Last().GetValue(), (double)childrenToAdd.First().GetValue());
			Children = childrenToAdd;
		}
	}

	class StringNode : Node {
		string value;
		public override string GetValueAsString() {
			return value;
		}
		public override object GetValue() {
			return value;
		}
		public override void SetValue(LinkedList<Node> childrenToAdd) {
			Children = childrenToAdd;
		}
	}

	class KeywordNode : Node {
		int value;
		public override string GetValueAsString() {
			return value.ToString();
		}
		public override object GetValue() {
			return value;
		}
		public override void SetValue(LinkedList<Node> childrenToAdd) {
			Children = childrenToAdd;
		}
	}

	class IdentifierNode : Node {
		string name;
		Node refernceValue;
		public override string GetValueAsString() {
			return name + ": " + refernceValue.GetValueAsString();
		}
		public override object GetValue() {
			return refernceValue;
		}
		public override void SetValue(LinkedList<Node> childrenToAdd) {
			Children = childrenToAdd;
		}
	}

	class FunctionNode : Node {
		public FunctionNode(string name) {
			this.functionName = name;
			this.NumberOfChildren = Functions.FunctionLookup[functionName.ToLower()].NumberOfParameters;
		}
		string functionName;
		Node value;
		public override string GetValueAsString() {
			return functionName + " [" + value.GetValueAsString() + "]";
		}
		public override object GetValue() {
			return value.GetValue();
		}
		public override void SetValue(LinkedList<Node> childrenToAdd) {
			value = Functions.FunctionLookup[functionName.ToLower()].Func(childrenToAdd);
			Children = childrenToAdd;
		}
	}
}


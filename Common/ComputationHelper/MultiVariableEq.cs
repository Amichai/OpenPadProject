using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace Common {

	//TODO: recognize different types of parameters and their implications:
	/// 1. Parameters that are dependent on themselves can have one side of the eq subtracted from the other and solved using the iterative solver
	/// 2. Throw an error when evaluation can lead to an infinite loop by defining a dependence DAG
	/// 
	public class MultiVariableEq  {
		private Dictionary<string, Func<double>> functions = new Dictionary<string, Func<double>>();

		public MultiVariableEq(){}
		public MultiVariableEq(double constant) {
			AddEqParameter("constant", constant);
		}

		public double this[string i] {
			get { return functions[i](); }
			set { functions[i] = () => value; }
		}

		LinkedList<string> parameterDependenceDAG = new LinkedList<string>();

		public double Get(string paramName) {
			return this[paramName];
		}
		public void SetParameter(string paramName, double val) {
			if (functions.ContainsKey(paramName)) {
				functions[paramName] = () => val;
			} else functions.Add(paramName, () => val);
		}
		/// <summary>
		/// Takes the partial derivative of one parameter with respect to another
		/// </summary>
		public double Partial(string dx, string dy, double at, double epsilon = .01) {
			if (dependentVariables.Contains(dy))
				throw new Exception("Cannot set a dependent variable");
			return (Evaluate(dy, at + epsilon / 2, dx) - Evaluate(dy, at - epsilon / 2, dx)) / epsilon;
		}

		HashSet<string> dependentVariables = new HashSet<string>();
		public void AddDependentVariable(string paramName, Func<double> del) {
			dependentVariables.Add(paramName);
			functions.Add(paramName, del);
		}

		public void AddSelfDependentVar(string paramName, Func<double,double> del) {
			new SingleVariableEq(del);
		}


		public void AddEqParameter(string param, double val) {
			if (dependentVariables.Contains(param))
				throw new Exception("Overriding a dependent variable");
			functions[param] = () => val;
		}

		public double Evaluate(string varToSet, double val, string varToObserve) {
			if (dependentVariables.Contains(varToSet))
				throw new Exception("Cannot set a dependent variable");
			functions[varToSet] = () => val;
			return functions[varToObserve]();
		}
		public double Evaluate(string varToObserve, params Tuple<string, double>[] set) {
			foreach (var s in set) {
				if(dependentVariables.Contains(s.Item1))
					throw new Exception("Cannot set a dependent variable");
				functions[s.Item1] = () => s.Item2;
			}
			return functions[varToObserve]();
		}

		public double Evaluate(string var1, double val1, string var2, double val2, string varToObserve) {
			if (dependentVariables.Contains(var1) || dependentVariables.Contains(var2))
				throw new Exception("Cannot set a dependent variable");
			functions[var1] = () => val1;
			functions[var2] = () => val2;
			return functions[varToObserve]();
		}

		public SingleVariableEq Relate(string var1, string var2) {
			return new SingleVariableEq(
				i => Evaluate(var1, i, var2)
			);
		}

		public Series TrialData(string xAxis, string yAxis, double xMin, double xMax, double dx, string lineName = null) {
			Series series;
			if (lineName == null)
				series = new Series(xAxis + "-" + yAxis);
			else
				series = new Series(lineName);
			var eq = Relate(xAxis, yAxis);
			for (double i = xMin; i <= xMax; i += dx) {
				series.Points.Add(new DataPoint(i, eq.Evaluate(i)));
			}
			series.ChartType = SeriesChartType.Line;
			series.XAxisType = AxisType.Primary;
			series.YAxisType = AxisType.Primary;
			
			series.ChartArea = "ChartArea1";
			series.Legend = "Legend1";
			return series;
		}

		public Series ParemetricTrial(string xAxis, string yAxis, string paremetricParm, double pMin, double pMax, double dp, string lineName = null) {
			Series series;
			if (lineName == null)
				series = new Series(xAxis + "-" + yAxis);
			else
				series = new Series(lineName);
			var eq1 = Relate(paremetricParm, xAxis);
			var eq2 = Relate(paremetricParm, yAxis);
			for (double i = pMin; i <= pMax; i += dp) {
				series.Points.Add(new DataPoint(eq1.Evaluate(i), eq2.Evaluate(i)));
			}
			series.ChartType = SeriesChartType.Line;
			series.XAxisType = AxisType.Primary;
			series.YAxisType = AxisType.Primary;

			series.ChartArea = "ChartArea1";
			series.Legend = "Legend1";
			return series;
		}

		internal bool Contains(params string[] p) {
			return p.All(i=>functions.ContainsKey(i) || i == this.variable);
		}

		string variable = string.Empty;
		public void AddVariable(string var) {
			this.variable = var;
		}
	}


	public class MultiVariable {
		Dictionary<string, Parameter> prmtrs = new Dictionary<string, Parameter>();
		Dictionary<Parameter, Evaluator> functions = new Dictionary<Parameter, Evaluator>();

		//LinkedList<LinkedListNode<Parameter>> systemParameters = new LinkedList<LinkedListNode<Parameter>>();
		public MultiVariable(params string[] parameters){
			foreach (string p in parameters) {
				prmtrs.Add(p, new Parameter(p));
				functions.Add(new Parameter(p), null);
			}
		}
		public void SetDependentVariable(string name, Evaluator function) {
			prmtrs[name].SetDependencies(function.Dependencies);
			functions[prmtrs[name]] = function;
			if (function.Dependencies.Where(i => i.ParameterName == name).Count() > 0) {
				throw new Exception("This parameter is self dependent. Should be set to zero.");
			}
		}
		public void SetIndependentVariable(string name, double val) {
			prmtrs[name].SetConstantVal(val);
			functions[prmtrs[name]] = new Evaluator(val);
		}
		public void SetSystemZero(string name, Evaluator function) {
			prmtrs[name].SetDependencies(function.Dependencies);
			functions[prmtrs[name]] = function;
			if (function.Dependencies.Where(i => i.ParameterName == name).Count() == 0) {
				throw new Exception("This parameter is not self dependent.");
			}
		}
	}

	public class Evaluator {
		public Evaluator(double constantVal) {
			this.constantVal = constantVal;
			this.function = null;
			this.type = EvaluatorType.constant;
		}
		double constantVal = double.MinValue;
		Func<double> function;
		SingleVariableEq singleVarToSetToZero;
		/// <summary>
		/// This is for resolving variables by setting the equation to zero.
		/// </summary>
		public Evaluator(SingleVariableEq function, params Parameter[] parameters) {
			Dependencies.AddRange(parameters);
			this.singleVarToSetToZero = function;
			this.constantVal = double.MinValue;
			this.type = EvaluatorType.setToZero;
		}
		public Evaluator(Func<double> function, params Parameter[] parameters) {
			Dependencies.AddRange(parameters);
			this.function = function;
			this.constantVal = double.MinValue;
			this.type = EvaluatorType.function;
		}
		
		public List<Parameter> Dependencies = new List<Parameter>();
		EvaluatorType type = EvaluatorType.none;
		public double Eval() {
			switch (type) {
				case EvaluatorType.constant:
					return constantVal;
				case EvaluatorType.function:
					return function();
			}
			throw new Exception("cannot be evaluated");
		}
		public double EvalAtZero(double approx,
			double xMin, double xMax, double eps, int maxiter) {
				if (type == EvaluatorType.setToZero)
					return singleVarToSetToZero.NewtonRaphson(.1, .1, .1, .01, 100);
				else throw new Exception();
		}
	}
	public enum EvaluatorType { constant, function, setToZero, none };
	public enum ParamaterType { independent, dependent, selfDependent, none };
	public class Parameter{
		public Parameter(string p) {
			this.ParameterName = p;
		}
		double constantValue = double.MinValue;
		ParamaterType type = ParamaterType.none;
		public void SetDependencies(List<Parameter> dependencies) {
			this.dependencies = dependencies;
			this.type = ParamaterType.dependent;
		}
		public void SetConstantVal(double value) {
			this.constantValue = value;
			type = ParamaterType.independent;
		}
		public void SetZero(List<Parameter> list) {
			this.type = ParamaterType.selfDependent;
			this.dependencies = list;
		}		
		public string ParameterName;
		public bool Resolvable(){
			switch (type) {
				case ParamaterType.none:
					return false;
				case ParamaterType.independent:
					return constantValue != double.MinValue;
				case ParamaterType.dependent:
					return allDependenciesAreResolvable();
				case ParamaterType.selfDependent:
					return allDependenciesAreResolvable();
			}
			throw new Exception("Unknown parameter type");
		}
		bool underinspection = false;
		private bool allDependenciesAreResolvable() {
			underinspection = true;
			foreach (var d in dependencies) {
				if (d.underinspection)
					throw new Exception("Circular reference: " + ParameterName);
				if (!d.Resolvable()) {
					underinspection = false;
					return false;
				}
			}
			underinspection = false;
			return true;
		}
		List<Parameter> dependencies = new List<Parameter>();
	}
}

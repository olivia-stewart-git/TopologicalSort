using System.Diagnostics;

namespace TopologicalSort
{
	[DebuggerDisplay("Node: {Value}")]
	public class Node<T>
	{
		public T Value { get; }

		List<Node<T>> _dependencies = new();

		public IReadOnlyList<Node<T>> Dependencies => _dependencies.AsReadOnly();
		public List<Node<T>> Dependents { get; } = new();

		public Node(T value)
		{
			Value = value;
		}

		public void AddDependency(Node<T> node)
		{
			_dependencies.Add(node);
			node.Dependents.Add(this);
		}
	}
}
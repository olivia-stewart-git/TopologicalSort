namespace TopologicalSort
{
	public class DepthFirstTopologicalSorter<T> : ITopologicalSorter<T>
	{
		public IEnumerable<Node<T>> Sort(IEnumerable<Node<T>> nodes)
		{
			var visited = new HashSet<Node<T>>();
			var result = new List<Node<T>>();
			void Visit(Node<T> node)
			{
				if (!visited.Contains(node))
				{
					visited.Add(node);
					foreach (var dependency in node.Dependencies)
					{
						Visit(dependency);
					}
					result.Add(node);
				}
				else if (!result.Contains(node))
				{
					throw new InvalidOperationException("Graph has at least one cycle.");
				}
			}
			foreach (var node in nodes)
			{
				Visit(node);
			}
			return result;
		}
	}
}
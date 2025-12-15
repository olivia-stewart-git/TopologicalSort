namespace TopologicalSort
{
	public class KahnTopologicalSorter<T> : ITopologicalSorter<T>
	{
		public IEnumerable<Node<T>> Sort(IEnumerable<Node<T>> nodes)
		{
			var inDegree = new Dictionary<Node<T>, int>();
			foreach (var node in nodes)
			{
				if (!inDegree.ContainsKey(node))
				{
					inDegree[node] = 0;
				}
				foreach (var dependent in node.Dependents)
				{
					if (!inDegree.ContainsKey(dependent))
					{
						inDegree[dependent] = 0;
					}
					inDegree[dependent]++;
				}
			}

			var queue = new Queue<Node<T>>(inDegree.Where(kv => kv.Value == 0).Select(kv => kv.Key));
			var result = new List<Node<T>>();
			while (queue.Count > 0)
			{
				var node = queue.Dequeue();
				result.Add(node);
				foreach (var dependent in node.Dependents)
				{
					inDegree[dependent]--;
					if (inDegree[dependent] == 0)
					{
						queue.Enqueue(dependent);
					}
				}
			}
			if (result.Count != inDegree.Count)
			{
				throw new InvalidOperationException("Graph has at least one cycle.");
			}
			return result;
		}
	}
}
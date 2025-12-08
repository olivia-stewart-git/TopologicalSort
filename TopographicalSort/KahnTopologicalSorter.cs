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
				foreach (var dependency in node.Dependencies)
				{
					if (!inDegree.ContainsKey(dependency))
					{
						inDegree[dependency] = 0;
					}
					inDegree[dependency]++;
				}
			}

			var queue = new Queue<Node<T>>(inDegree.Where(kv => kv.Value == 0).Select(kv => kv.Key));
			var result = new List<Node<T>>();
			while (queue.Count > 0)
			{
				var node = queue.Dequeue();
				result.Add(node);
				foreach (var dependency in node.Dependencies)
				{
					inDegree[dependency]--;
					if (inDegree[dependency] == 0)
					{
						queue.Enqueue(dependency);
					}
				}
			}
			if (result.Count != inDegree.Count)
			{
				throw new InvalidOperationException("Graph has at least one cycle.");
			}
			return result.Reverse<Node<T>>(); // Reverse to get correct order as dependencies first
		}
	}
}
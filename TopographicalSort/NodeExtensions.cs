namespace TopologicalSort
{
	public static class NodeExtensions
	{
		public static void AddDependencies<T>(this Node<T> node, params Node<T>[] dependencies)
		{
			foreach (var dependency in dependencies)
			{
				node.AddDependency(dependency);
			}
		}
	}
}
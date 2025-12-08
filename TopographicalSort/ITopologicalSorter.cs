namespace TopologicalSort
{
	public interface ITopologicalSorter<T>
	{
		IEnumerable<Node<T>> Sort(IEnumerable<Node<T>> nodes);
	}
}
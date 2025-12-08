namespace TopologicalSort
{
	public class TopologicalSorterFactory<T>
	{
		public ITopologicalSorter<T> Create(SortMode sortMode)
		{
			return sortMode switch
			{
				SortMode.DepthFirst => new DepthFirstTopologicalSorter<T>(),
				SortMode.Kahn => new KahnTopologicalSorter<T>(),
				_ => throw new ArgumentOutOfRangeException(nameof(sortMode), sortMode, null)
			};
		}
	}
}
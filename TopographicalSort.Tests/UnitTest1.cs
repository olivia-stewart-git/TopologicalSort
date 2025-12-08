namespace TopologicalSort.Tests
{
	[TestFixture(SortMode.DepthFirst)]
	[TestFixture(SortMode.Kahn)]
	public class SortingTests(SortMode sortMode)
	{
		ITopologicalSorter<string> _sorter;

		[SetUp]
		public void Setup()
		{
			_sorter = new TopologicalSorterFactory<string>().Create(sortMode);
		}

		[Test]
		public void Sort_AlreadyInOrder()
		{
			var nodes = new List<Node<string>>
			{
				new Node<string>("A"),
				new Node<string>("B"),
				new Node<string>("C")
			};

			nodes[1].AddDependency(nodes[0]);
			nodes[2].AddDependency(nodes[1]);

			var sorted = _sorter.Sort(nodes);

			AssertSorted(sorted);
		}

		void AssertSorted(IEnumerable<Node<string>> sortedNodes)
		{
			var nodePositions = new Dictionary<Node<string>, int>();
			var sortedNodesList = sortedNodes.ToList();
			for (int i = 0; i < sortedNodesList.Count; i++)
			{
				nodePositions[sortedNodesList[i]] = i;
			}

			foreach (var node in sortedNodes)
			{
				foreach (var dependency in node.Dependencies)
				{
					Assert.That(nodePositions[dependency], Is.LessThan(nodePositions[node]), $"Node {node.Value} appears before its dependency {dependency.Value}");
				}
			}
		}
	}
}

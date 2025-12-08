using System.Collections;

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
			var nodes = new StringNodesContainer("A", "B", "C");
			nodes["B"].AddDependency(nodes["A"]);
			nodes["C"].AddDependency(nodes["B"]);

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

			Assert.Multiple(() =>
			{
				Assert.That(sortedNodesList.Count, Is.EqualTo(nodePositions.Count), "Sorted list should contain all nodes");
				foreach (var node in sortedNodes)
				{
					foreach (var dependency in node.Dependencies)
					{
						Assert.That(nodePositions[dependency], Is.LessThan(nodePositions[node]), $"Node {node.Value} appears before its dependency {dependency.Value}");
					}
				}
			});
		}

		class StringNodesContainer : IEnumerable<Node<string>>
		{
			public Node<string> this[string value] => _nodes[value];

			Dictionary<string, Node<string>> _nodes = new();
			public StringNodesContainer(params string[] values)
			{
				foreach (var value in values)
				{
					_nodes[value] = new Node<string>(value);
				}
			}

			public IEnumerator<Node<string>> GetEnumerator()
			{
 				return _nodes.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}

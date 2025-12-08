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

		[Test]
		public void Sort_ReversedOrder()
		{
			var nodes = new StringNodesContainer("A", "B", "C");
			nodes["B"].AddDependency(nodes["A"]);
			nodes["C"].AddDependency(nodes["B"]);

			var sorted = _sorter.Sort(nodes);
			AssertSorted(sorted);
		}

		[Test]
		public void Sort_EmptyGraph()
		{
			var nodes = new StringNodesContainer();
			var sorted = _sorter.Sort(nodes);
			Assert.That(sorted.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Sort_SingleNode()
		{
			var nodes = new StringNodesContainer("A");
			var sorted = _sorter.Sort(nodes);
			Assert.That(sorted.Single().Value, Is.EqualTo("A"));
		}

		[Test]
		public void Sort_MultipleIndependentChains()
		{
			var nodes = new StringNodesContainer("A", "B", "C", "D");
			nodes["B"].AddDependency(nodes["A"]);
			nodes["D"].AddDependency(nodes["C"]);
			var sorted = _sorter.Sort(nodes);
			AssertSorted(sorted);
		}

		[Test]
		public void Sort_BranchingDependencies()
		{
			var nodes = new StringNodesContainer("A", "B", "C", "D");
			nodes["B"].AddDependency(nodes["A"]);
			nodes["C"].AddDependency(nodes["A"]);
			nodes["D"].AddDependency(nodes["B"]);
			nodes["D"].AddDependency(nodes["C"]);
			var sorted = _sorter.Sort(nodes);
			AssertSorted(sorted);
		}

		[Test]
		public void Sort_DiamondShape()
		{
			var nodes = new StringNodesContainer("A", "B", "C", "D");
			nodes["B"].AddDependency(nodes["A"]);
			nodes["C"].AddDependency(nodes["A"]);
			nodes["D"].AddDependency(nodes["B"]);
			nodes["D"].AddDependency(nodes["C"]);
			var sorted = _sorter.Sort(nodes);
			AssertSorted(sorted);
		}

		[Test]
		public void Sort_Cycle_Throws()
		{
			var nodes = new StringNodesContainer("A", "B", "C");
			nodes["A"].AddDependency(nodes["C"]);
			nodes["B"].AddDependency(nodes["A"]);
			nodes["C"].AddDependency(nodes["B"]);
			Assert.Throws<InvalidOperationException>(() => _sorter.Sort(nodes));
		}

		[Test]
		public void Sort_AllDependOnRoot()
		{
			var nodes = new StringNodesContainer("A", "B", "C", "D");
			nodes["B"].AddDependency(nodes["A"]);
			nodes["C"].AddDependency(nodes["A"]);
			nodes["D"].AddDependency(nodes["A"]);
			var sorted = _sorter.Sort(nodes);
			AssertSorted(sorted);
		}

		[Test]
		public void Sort_AllDependOnLeaf()
		{
			var nodes = new StringNodesContainer("A", "B", "C", "D");
			nodes["A"].AddDependency(nodes["D"]);
			nodes["B"].AddDependency(nodes["D"]);
			nodes["C"].AddDependency(nodes["D"]);
			var sorted = _sorter.Sort(nodes);
			AssertSorted(sorted);
		}

		[Test]
		public void Sort_ComplexGraph_MultipleValidOrders()
		{
			var nodes = new StringNodesContainer("A", "B", "C", "D", "E", "F");
			nodes["B"].AddDependency(nodes["A"]);
			nodes["C"].AddDependency(nodes["A"]);
			nodes["D"].AddDependency(nodes["B"]);
			nodes["E"].AddDependency(nodes["B"]);
			nodes["E"].AddDependency(nodes["C"]);
			nodes["F"].AddDependency(nodes["D"]);
			nodes["F"].AddDependency(nodes["E"]);
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

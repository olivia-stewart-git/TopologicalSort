# Topological Sort

## Descrption

## Use Case examples

## Algorithm
For the implementation I have actually chosen two algorithms to solve the given "problem". They are as follows.

### DFS (Depth First Search)

#### PseudoCode Implementation

#### Comlexity

### Kahns Algorithm

#### PseudoCode Implementation

#### Comlexity

#### Implementation Notes
Algorithm outputs nodes in an ordering of parent nodes first, to match to output of DFS for testing, I added a reverse to the end of the implmentation. This ands a constant of O(N), however it is specific to my implmentation and not Kahns algorithm.

### Comparison of algorithms

## Testing Approach

For testing, I used the NUnit library of unit tests. To support the usage of both algorithms, each implementation is tested against the same set of tests. The testing approach is to use a simple algorithm which that dependencies are not ordered after their owners. This allows for asserting true in cases of the sort differing in order while still being technically correct.
```cs
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
```

## How to run tests locally

- Download the files


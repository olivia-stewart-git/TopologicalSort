# Topological Sort

## Description

Topological sorting is an algorithm used to order the vertices of a directed acyclic graph (DAG) such that for every directed edge from vertex A to vertex B, vertex A appears before vertex B in the ordering. This is particularly useful when dealing with dependencies where certain tasks must be completed before others can begin.

The algorithm ensures that dependencies are resolved in the correct order, making it impossible for a task to be scheduled before its prerequisites are satisfied. If the graph contains a cycle, topological sorting is impossible, as circular dependencies cannot be resolved.

## Use Case Examples

1. **Build Systems**: Compiling source code files where some files depend on others (e.g., header files in C++)
2. **Task Scheduling**: Determining the order of tasks in a project where some tasks must be completed before others can start
3. **Course Prerequisites**: Determining the order in which university courses should be taken based on prerequisite requirements
4. **Package Dependency Resolution**: Installing software packages where some packages depend on others (e.g., npm, apt, pip)
5. **Spreadsheet Formula Calculation**: Evaluating cells in the correct order when formulas reference other cells
6. **Data Pipeline Orchestration**: Executing data transformation steps in the correct sequence

## Algorithm
For the implementation I have actually chosen two algorithms to solve the given "problem". They are as follows.

### DFS (Depth First Search)

The DFS-based topological sort works by performing a depth-first traversal of the graph, visiting each node's dependencies before adding the node itself to the result. This approach naturally ensures that all dependencies appear before their dependents in the final ordering.

The algorithm maintains a visited set to track processed nodes and detects cycles by checking if a node is visited but not yet added to the result list.

#### PseudoCode Implementation

```
function DFS_TopologicalSort(nodes):
    visited = empty set
    result = empty list
    
    function Visit(node):
        if node in visited:
            if node not in result:
                throw CycleDetected
            return
        
        add node to visited
        
        for each dependency in node.Dependencies:
            Visit(dependency)
        
        add node to result
    
    for each node in nodes:
        Visit(node)
    
    return result
```

#### Complexity

- **Time Complexity**: O(V + E), where V is the number of vertices (nodes) and E is the number of edges (dependencies). Each node is visited once, and each edge is traversed once.
- **Space Complexity**: O(V) for the visited set, result list, and recursive call stack in the worst case (linear chain of dependencies).

### Kahns Algorithm

Kahn's algorithm works by repeatedly removing nodes with no incoming edges (in-degree of 0) from the graph iteratively. It maintains a queue of nodes that are ready to be processed (those with no unprocessed dependencies) and processes them in order.

The algorithm calculates the in-degree for each node, then processes nodes with zero in-degree. As each node is processed, it decrements the in-degree of its dependents. If a cycle exists, some nodes will never reach zero in-degree, allowing for cycle detection.

#### PseudoCode Implementation

```
function Kahn_TopologicalSort(nodes):
    inDegree = map of node -> count of incoming edges
    
    for each node in nodes:
        if node not in inDegree:
            inDegree[node] = 0
        for each dependency in node.Dependencies:
            if dependency not in inDegree:
                inDegree[dependency] = 0
            inDegree[dependency]++
    
    queue = empty queue
    for each (node, degree) in inDegree:
        if degree == 0:
            enqueue node to queue
    
    result = empty list
    
    while queue is not empty:
        node = dequeue from queue
        add node to result
        
        for each dependency in node.Dependencies:
            inDegree[dependency]--
            if inDegree[dependency] == 0:
                enqueue dependency to queue
    
    if result.Count != inDegree.Count:
        throw CycleDetected
    
    return reverse(result)
```

#### Complexity

- **Time Complexity**: O(V + E), where V is the number of vertices and E is the number of edges. The algorithm iterates through all nodes and edges once to calculate in-degrees, then processes each node and edge once more.
- **Space Complexity**: O(V) for the in-degree map, queue, and result list.

#### Implementation Notes
Algorithm expects nodes with ordering of incoming dependencies to match to output of DFS for testing, I added a reverse to the end of the implmentation. This ands a constant of O(N), however it is specific to my implmentation and not Kahns algorithm.

### Comparison of algorithms

Both DFS and Kahn's algorithm have the same time and space complexity (O(V + E) time, O(V) space), but they differ in their approach and characteristics:

**DFS-based Topological Sort:**
- **Advantages:**
  - More intuitive recursive implementation
  - Naturally produces dependencies-first ordering
  - Generally simpler to understand and implement
  - Better suited for graphs represented as adjacency lists
- **Disadvantages:**
  - Uses recursion, which may cause stack overflow for very deep graphs
  - Cycle detection requires tracking visit state carefully

**Kahn's Algorithm:**
- **Advantages:**
  - Iterative approach avoids recursion/stack issues
  - Explicitly calculates in-degrees, which can be useful for other purposes
  - Cycle detection is straightforward (check if all nodes were processed)
  - Better for scenarios where you need to track progress incrementally
- **Disadvantages:**
  - Requires additional data structure (queue) and in-degree map
  - Slightly more complex to implement
  - In this implementation, requires reversing the result to match DFS ordering

**Which to choose?**
- For most cases, either algorithm works well
- Choose DFS if you prefer recursive solutions and have moderate graph depth
- Choose Kahn's if you need iterative processing or want explicit cycle detection
- For very large or deep graphs, Kahn's iterative approach may be safer

## Testing Approach

For testing, I used the NUnit library of unit tests. To support the usage of both algorithms, each implementation is tested against the same set of tests using NUnit's `TestFixture` parameterization. The testing approach uses a validation algorithm that checks dependencies are not ordered after their owners. This allows for asserting correctness in cases where the sort differs in order while still being technically correct, since topological sorts can have multiple valid orderings.

The test suite includes:
- **Basic cases**: Empty graphs, single nodes, already sorted sequences
- **Structural patterns**: Linear chains, branching dependencies, diamond shapes
- **Edge cases**: Multiple independent chains, nodes with shared dependencies
- **Complex scenarios**: Large graphs with multiple valid orderings
- **Error handling**: Cycle detection validation

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

1. **Prerequisites**:
   - Install .NET 10.0 SDK or later from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
   - Ensure you have a compatible IDE (Visual Studio 2022, VS Code, or JetBrains Rider) or use the command line

2. **Download the files**:
   ```powershell
   git clone https://github.com/olivia-stewart-git/TopologicalSort
   cd TopologicalSort
   ```

3. **Restore dependencies**:
   ```powershell
   dotnet restore
   ```

4. **Build the solution**:
   ```powershell
   dotnet build
   ```

5. **Run the tests**:
   ```powershell
   dotnet test
   ```

6. **Run tests with detailed output**:
   ```powershell
   dotnet test --verbosity normal
   ```

The test output will show all passed tests and any failures, with detailed information about which assertions failed and why.


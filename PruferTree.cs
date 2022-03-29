namespace TestJobj;

public static class PruferTree
{
    public static (int, int)[] GenerateRandomTree(this Random random, int nodesCount)
    {
        Span<int> prufer = stackalloc int[nodesCount - 2];
        for (int i = 0; i < prufer.Length; i++)
        {
            prufer[i] = random.Next(nodesCount - 1);
        }

        return GenerateTree(prufer);
    }

    public static (int, int)[] GenerateTree(params int[] prufer) => GenerateTree(prufer.AsSpan());

    // https://en.wikipedia.org/wiki/Pr%C3%BCfer_sequence
    public static (int, int)[] GenerateTree(Span<int> prufer)
    {
        var nodesCount = prufer.Length + 2;
        Span<int> vertexSet = stackalloc int[nodesCount];
        vertexSet.Clear();
        foreach (var p in prufer)
        {
            vertexSet[p]++;
        }

        var edges = new (int, int)[nodesCount - 1];
        var edgesIterator = 0;

        foreach (var p in prufer)
        {
            for (var vertex = 0; vertex < nodesCount; vertex++)
            {
                if (vertexSet[vertex] == 0)
                {
                    vertexSet[vertex] = -1;
                    vertexSet[p]--;

                    edges[edgesIterator] = (p, vertex);
                    edgesIterator++;

                    break;
                }
            }
        }

        for (int vertex = 0, previousVertex = -1; vertex < nodesCount; vertex++)
        {
            if (vertexSet[vertex] == 0 && previousVertex == -1)
            {
                previousVertex = vertex;
            }
            else if (vertexSet[vertex] == 0 && previousVertex > -1)
            {
                edges[edgesIterator] = (previousVertex, vertex);
                break;
            }
        }

        return edges;
    }
}
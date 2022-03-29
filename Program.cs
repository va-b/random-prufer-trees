// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json.Linq;
using TestJobj;


JObject GenerateTree(int nodesCount = 8)
{
    var treeNodes = new JObject[nodesCount];

    for (int i = 0; i < nodesCount; i++)
    {
        treeNodes[i] = new JObject
        {
            { "enabled", Random.Shared.Next() % 2 == 0 },
        };
    }

    var tree = Random.Shared.GenerateRandomTree(nodesCount);

    foreach (var (a, b) in tree)
    {
        if (treeNodes[a]["features"] == null)
        {
            treeNodes[a].Add("features", new JObject());
        }

        ((JObject)treeNodes[a]["features"])!.Add($"ft-{b}", treeNodes[b]);
    }

    var result = new JObject();
    for (int i = 0; i < nodesCount; i++)
    {
        if (treeNodes[i].Parent == null)
        {
            result.Add("features", treeNodes[i]["features"]);
            break;
        }
    }

    return result;
}

Console.WriteLine(JsonHelper.MergeMany(Enumerable.Repeat(0, 2).Select(x =>
{
    var tree = GenerateTree(5);
    Console.WriteLine(tree.ToString());
    return tree;
})).ToString());

// Console.WriteLine("obj1");
// var obj1 = new JObject { { "Enabled", true } };
// var obj11 = obj1.DeepClone() as JObject;
// var obj111 = obj1.DeepClone() as JObject;
// Console.WriteLine(obj1.ToString());
//
// Console.WriteLine("obj2");
// var obj2 = new JObject { { "Enabled", false } };
// var obj22 = obj2.DeepClone() as JObject;
// var obj222 = obj2.DeepClone() as JObject;
// Console.WriteLine(obj2.ToString());
//
// Console.WriteLine("Merge obj1 <- obj2");
// obj1.StraightMerge(obj2);
// Console.WriteLine(obj1.ToString());
//
// Console.WriteLine("Merge obj2 <- obj1");
// obj22!.StraightMerge(obj11!);
// Console.WriteLine(obj22!.ToString());
//
// Console.WriteLine("Merge obj1 <- obj2 with obj2 priority");
// obj111!.StraightMerge(obj222!, true);
// Console.WriteLine(obj111!.ToString());
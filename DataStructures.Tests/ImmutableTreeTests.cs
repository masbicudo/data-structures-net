using System.Collections.Generic;
using DataStructures.Immutable;
using DataStructures.Immutable.Tree;
using DataStructures.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DataStructures.Tests
{
    [TestClass]
    public class ImmutableTreeTests
    {
        [TestMethod]
        public void Test_BuildForestFromFlatData()
        {
            var forest = BuildForestFromFlatData();

            Assert.AreEqual(forest.Nodes[0].Value, "R1");
            Assert.AreEqual(forest.Nodes[0].Children[0].Value, "B1");
            Assert.AreEqual(forest.Nodes[0].Children[0].Children[0].Value, "B2");
            Assert.AreEqual(forest.Nodes[0].Children[0].Children[0].Children[0].Value, "L2");
            Assert.AreEqual(forest.Nodes[0].Children[0].Children[1].Value, "L1");
            Assert.AreEqual(forest.Nodes[0].Children[0].Children[2].Value, "L4");
            Assert.AreEqual(forest.Nodes[0].Children[1].Value, "B3");
            Assert.AreEqual(forest.Nodes[0].Children[1].Children[0].Value, "L3");
        }

        [TestMethod]
        public void Test_GetAllNodesInRecursiveDescentOrder()
        {
            var forest = BuildForestFromFlatData();
            var list = forest.Nodes[0].GetAllNodesEnum().ToList();

            Assert.AreEqual(list[0].Value, "R1");
            Assert.AreEqual(list[1].Value, "B1");
            Assert.AreEqual(list[2].Value, "B2");
            Assert.AreEqual(list[3].Value, "L2");
            Assert.AreEqual(list[4].Value, "L1");
            Assert.AreEqual(list[5].Value, "L4");
            Assert.AreEqual(list[6].Value, "B3");
            Assert.AreEqual(list[7].Value, "L3");
        }

        [TestMethod]
        public void Test_GetAllNodesInHightOrder()
        {
            var forest = BuildForestFromFlatData();
            var list = forest.Nodes[0].GetAllNodesByHeightEnum().ToList();

            Assert.AreEqual(list[0].Value, "R1");
            Assert.AreEqual(list[1].Value, "B1");
            Assert.AreEqual(list[2].Value, "B3");
            Assert.AreEqual(list[3].Value, "B2");
            Assert.AreEqual(list[4].Value, "L1");
            Assert.AreEqual(list[5].Value, "L4");
            Assert.AreEqual(list[6].Value, "L3");
            Assert.AreEqual(list[7].Value, "L2");
        }

        [TestMethod]
        public void Test_ChangeForestUsingVisitor()
        {
            var nodeList = new[]
            {
                new TreeItemData { Id = 1, ParentId = null, Value = "RB1" },
                new TreeItemData { Id = 2, ParentId = 1, Value = "RL1" },
                new TreeItemData { Id = 3, ParentId = 1, Value = "RL2" },
            };

            var forest = BuildForestFromFlatData();

            var builder = new ImmutableTreeBuilder();

            var dicChildren = nodeList
                .Where(n => n.ParentId != null)
                .GroupBy(n => n.ParentId)
                // ReSharper disable once PossibleInvalidOperationException
                .ToDictionary(g => g.Key.Value, n => n.ToList());

            forest.Visit(
                (n, c) =>
                {
                    // replaces L1 node with another sub-tree
                    if (n.Value == "L1")
                        return builder.BuildBranchesOrLeaves(
                            x => dicChildren.GetValueOrDefault(x.Id, new List<TreeItemData>()),
                            x => x.Value,
                            nodeList.Where(x => x.ParentId == null));

                    return ((Node<string>)n).RecreateNodeIfNeeded(c);
                });
        }

        private static ImmutableForest<string> BuildForestFromFlatData()
        {
            var nodeList = new[]
            {
                new TreeItemData { Id = 1, ParentId = null, Value = "R1" },
                new TreeItemData { Id = 2, ParentId = 1, Value = "B1" },
                new TreeItemData { Id = 3, ParentId = 2, Value = "B2" },
                new TreeItemData { Id = 4, ParentId = 1, Value = "B3" },
                new TreeItemData { Id = 5, ParentId = 2, Value = "L1" },
                new TreeItemData { Id = 6, ParentId = 3, Value = "L2" },
                new TreeItemData { Id = 7, ParentId = 4, Value = "L3" },
                new TreeItemData { Id = 8, ParentId = 2, Value = "L4" },
            };

            var dicChildren = nodeList
                .Where(n => n.ParentId != null)
                .GroupBy(n => n.ParentId)
                // ReSharper disable once PossibleInvalidOperationException
                .ToDictionary(g => g.Key.Value, n => n.ToList());

            var builder = new ImmutableTreeBuilder();
            var forest = builder.BuildForest(
                x => dicChildren.GetValueOrDefault(x.Id, new List<TreeItemData>()),
                x => x.Value,
                nodeList.Where(x => x.ParentId == null));
            return forest;
        }
    }
}
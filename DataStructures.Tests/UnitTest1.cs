﻿using System.Collections.Generic;
using DataStructures.Immutable;
using DataStructures.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DataStructures.Tests
{
    [TestClass]
    public class ImmutableTreeTests
    {
        [TestMethod]
        public void TestMethod1()
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
                .ToDictionary(g => g.Key.Value, n => n.ToList());

            var builder = new ImmutableTreeBuilder();
            var forest = builder.BuildForest(
                x => dicChildren.GetValueOrDefault(x.Id, new List<TreeItemData>()),
                x => x.Value,
                nodeList.Where(x => x.ParentId == null));

            Assert.AreEqual(forest.Nodes[0].Value, "R1");
            Assert.AreEqual(forest.Nodes[0].Children[0].Value, "B1");
            Assert.AreEqual(forest.Nodes[0].Children[0].Children[0].Value, "B2");
            Assert.AreEqual(forest.Nodes[0].Children[0].Children[0].Children[0].Value, "L2");
            Assert.AreEqual(forest.Nodes[0].Children[0].Children[1].Value, "L1");
            Assert.AreEqual(forest.Nodes[0].Children[0].Children[2].Value, "L4");
            Assert.AreEqual(forest.Nodes[0].Children[1].Value, "B3");
            Assert.AreEqual(forest.Nodes[0].Children[1].Children[0].Value, "L3");
        }
    }
}
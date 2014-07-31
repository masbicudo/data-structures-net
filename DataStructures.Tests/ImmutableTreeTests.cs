using System;
using DataStructures.Immutable;
using DataStructures.Immutable.Tree;
using DataStructures.Tests.TestModels;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataStructures.Tests
{
    [TestClass]
    public class ImmutableTreeTests
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            // call each method in this class using reflection, to JIT-compile them
            foreach (var methodInfo in typeof(ImmutableTreeTests)
                .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                .Where(methodInfo => methodInfo.GetParameters().Length == 0)
                .Where(methodInfo => methodInfo.GetCustomAttributes(typeof(TestMethodAttribute)).Any()))
            {
                methodInfo.Invoke(new ImmutableTreeTests(), null);
            }
        }

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

            Assert.IsInstanceOfType(forest.Nodes[0], typeof(IRoot));
            Assert.IsInstanceOfType(forest.Nodes[0].Children[0], typeof(IBranch));
            Assert.IsInstanceOfType(forest.Nodes[0].Children[0].Children[0], typeof(IBranch));
            Assert.IsInstanceOfType(forest.Nodes[0].Children[0].Children[0].Children[0], typeof(ILeaf));
            Assert.IsInstanceOfType(forest.Nodes[0].Children[0].Children[1], typeof(ILeaf));
            Assert.IsInstanceOfType(forest.Nodes[0].Children[0].Children[2], typeof(ILeaf));
            Assert.IsInstanceOfType(forest.Nodes[0].Children[1], typeof(IBranch));
            Assert.IsInstanceOfType(forest.Nodes[0].Children[1].Children[0], typeof(ILeaf));
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
        public void Test_GetAllNodesInHeightOrder()
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
                TreeItemData.Create(1, null, "RB1"),
                TreeItemData.Create(2, 1, "RL1"),
                TreeItemData.Create(3, 1, "RL2"),
            };

            var forest = BuildForestFromFlatData();

            var builder = new ImmutableTreeBuilder();

            var dicChildren = nodeList
                .Where(n => n.ParentId != null)
                .GroupBy(n => n.ParentId)
                // ReSharper disable once PossibleInvalidOperationException
                .ToDictionary(g => g.Key.Value, n => n.ToList());

            var newBranch = builder.BuildBranchesOrLeaves(
                x => dicChildren.GetValueOrDefault(x.Id, new List<TreeItemData<string>>()),
                x => x.Value,
                nodeList.Where(x => x.ParentId == null));

            var newForest = forest.Visit<string>(
                (n, c) =>
                {
                    // replaces L1 node with another sub-tree
                    if (n.Value == "L1")
                        return newBranch;

                    return n.RecreateNodeIfNeeded(c);
                });

            Assert.AreEqual(newForest.Nodes[0].Value, "R1");
            Assert.AreEqual(newForest.Nodes[0].Children[0].Value, "B1");
            Assert.AreEqual(newForest.Nodes[0].Children[0].Children[0].Value, "B2");
            Assert.AreEqual(newForest.Nodes[0].Children[0].Children[0].Children[0].Value, "L2");
            Assert.AreEqual(newForest.Nodes[0].Children[0].Children[1].Value, "RB1");
            Assert.AreEqual(newForest.Nodes[0].Children[0].Children[1].Children[0].Value, "RL1");
            Assert.AreEqual(newForest.Nodes[0].Children[0].Children[1].Children[1].Value, "RL2");
            Assert.AreEqual(newForest.Nodes[0].Children[0].Children[2].Value, "L4");
            Assert.AreEqual(newForest.Nodes[0].Children[1].Value, "B3");
            Assert.AreEqual(newForest.Nodes[0].Children[1].Children[0].Value, "L3");

            Assert.IsInstanceOfType(newForest.Nodes[0], typeof(IRoot));
            Assert.IsInstanceOfType(newForest.Nodes[0].Children[0], typeof(IBranch));
            Assert.IsInstanceOfType(newForest.Nodes[0].Children[0].Children[0], typeof(IBranch));
            Assert.IsInstanceOfType(newForest.Nodes[0].Children[0].Children[0].Children[0], typeof(ILeaf));
            Assert.IsInstanceOfType(newForest.Nodes[0].Children[0].Children[1], typeof(IBranch));
            Assert.IsInstanceOfType(newForest.Nodes[0].Children[0].Children[1].Children[0], typeof(ILeaf));
            Assert.IsInstanceOfType(newForest.Nodes[0].Children[0].Children[1].Children[1], typeof(ILeaf));
            Assert.IsInstanceOfType(newForest.Nodes[0].Children[0].Children[2], typeof(ILeaf));
            Assert.IsInstanceOfType(newForest.Nodes[0].Children[1], typeof(IBranch));
            Assert.IsInstanceOfType(newForest.Nodes[0].Children[1].Children[0], typeof(ILeaf));
        }

        [TestMethod]
        public void Test_CombineLionsAndGirafes()
        {
            var forestOfGiraffes = BuildForestWithGirafes();

            // We can visit a forest of Giraffes, as if it was a forest of Animals,
            // and insert Lions inside it... this MUST be possible with a forest of Animals,
            // because a forest of Animals can contain both Giraffes and Lions.
            // When doing so, a minimal set of nodes will be converted.
            var forestOfAnimals = forestOfGiraffes.Visit<Animal>(
                (n, c) =>
                {
                    if (n.Value.Id == 2)
                        return n.RecreateNodeIfNeeded(c, new Lion(2));

                    return n.RecreateNodeIfNeeded(c);
                });

            // Checking types of values.
            Assert.IsInstanceOfType(forestOfAnimals.Nodes[0].Value, typeof(Giraffe));
            Assert.IsInstanceOfType(forestOfAnimals.Nodes[0].Children[0].Value, typeof(Lion));
            Assert.IsInstanceOfType(forestOfAnimals.Nodes[0].Children[0].Children[0].Value, typeof(Giraffe));
            Assert.IsInstanceOfType(forestOfAnimals.Nodes[0].Children[0].Children[0].Children[0].Value, typeof(Giraffe));
            Assert.IsInstanceOfType(forestOfAnimals.Nodes[0].Children[1].Value, typeof(Giraffe));

            // Checking types of collections.
            // The innermost Children collection was not touched by the visitor,
            // so it must be a specific collection of Giraffes, and not a generic collection of Animals.
            // All the other collections, up to the root, must be collections of Animals.
            Assert.IsInstanceOfType(forestOfAnimals.Nodes, typeof(ImmutableCollection<INode<Animal>>));
            Assert.IsInstanceOfType(forestOfAnimals.Nodes[0].Children, typeof(ImmutableCollection<INode<Animal>>));
            Assert.IsInstanceOfType(forestOfAnimals.Nodes[0].Children[0].Children, typeof(ImmutableCollection<INode<Animal>>));
            Assert.IsInstanceOfType(forestOfAnimals.Nodes[0].Children[0].Children[0].Children, typeof(ImmutableCollection<INode<Giraffe>>));
        }

        [TestMethod]
        public void Test_BuildTreeFromFlatData_WithContext()
        {
            var nodeList = new[]
            {
                TreeItemData.Create(1, null, "R1"),
                TreeItemData.Create(2, 1, "B1"),
                TreeItemData.Create(3, 2, "B2"),
                TreeItemData.Create(4, 1, "B3"),
                TreeItemData.Create(5, 2, "L1"),
                TreeItemData.Create(6, 3, "L2"),
                TreeItemData.Create(7, 4, "L3"),
                TreeItemData.Create(8, 2, "L4"),
            };

            var builder = new ImmutableTreeBuilderWithContext();
            var forest1 = builder.BuildForest<TreeItemData<string>, DoublyLinkedTreeNode<string>>(
                x => nodeList.Where(y => y.ParentId == x.Id),
                ctx => new DoublyLinkedTreeNode<string>(ctx.Data.Value, ctx.GetParentOrDefault()),
                nodeList.Where(x => x.ParentId == null),
                ctx => { ctx.Value.Children = ctx.GetChildren().MatchNullWithEmpty().ToArray(); });

            var forest2 = builder.BuildForest<TreeItemData<string>, DoublyLinkedTreeNode<string>>(
                x => nodeList.Where(y => y.ParentId == x.Id),
                ctx => new DoublyLinkedTreeNode<string>(ctx.Data.Value, ctx.GetChildren().MatchNullWithEmpty().ToArray()),
                nodeList.Where(x => x.ParentId == null),
                ctx => ctx.Value.Parent = ctx.GetParentOrDefault());

            var all1 = forest1.RootsEnum.SelectMany(x => x.GetAllNodesEnum().Select(x2 => x2.Value.Value)).ToArray();
            var all2 = forest2.RootsEnum.SelectMany(x => x.GetAllNodesEnum().Select(x2 => x2.Value.Value)).ToArray();

            Assert.IsTrue(all1.SequenceEqual(all2), "Sequences must be equal");

            Assert.AreEqual(forest1.Nodes[0].Value.Value, "R1");
            Assert.AreEqual(forest1.Nodes[0].Value.Children[0].Value, "B1");
            Assert.AreEqual(forest1.Nodes[0].Value.Children[0].Children[0].Value, "B2");
            Assert.AreEqual(forest1.Nodes[0].Value.Children[0].Children[0].Children[0].Value, "L2");
            Assert.AreEqual(forest1.Nodes[0].Value.Children[0].Children[1].Value, "L1");
            Assert.AreEqual(forest1.Nodes[0].Value.Children[0].Children[2].Value, "L4");
            Assert.AreEqual(forest1.Nodes[0].Value.Children[1].Value, "B3");
            Assert.AreEqual(forest1.Nodes[0].Value.Children[1].Children[0].Value, "L3");
            Assert.AreEqual(forest1.Nodes[0].Children[0].Value.Value, "B1");
            Assert.AreEqual(forest1.Nodes[0].Children[0].Value.Children[0].Value, "B2");
            Assert.AreEqual(forest1.Nodes[0].Children[0].Value.Children[0].Children[0].Value, "L2");
            Assert.AreEqual(forest1.Nodes[0].Children[0].Value.Children[1].Value, "L1");
            Assert.AreEqual(forest1.Nodes[0].Children[0].Value.Children[2].Value, "L4");
            Assert.AreEqual(forest1.Nodes[0].Children[1].Value.Value, "B3");
            Assert.AreEqual(forest1.Nodes[0].Children[1].Value.Children[0].Value, "L3");
            Assert.AreEqual(forest1.Nodes[0].Children[0].Children[0].Value.Value, "B2");
            Assert.AreEqual(forest1.Nodes[0].Children[0].Children[0].Value.Children[0].Value, "L2");
            Assert.AreEqual(forest1.Nodes[0].Children[0].Children[1].Value.Value, "L1");
            Assert.AreEqual(forest1.Nodes[0].Children[0].Children[2].Value.Value, "L4");
            Assert.AreEqual(forest1.Nodes[0].Children[1].Children[0].Value.Value, "L3");
            Assert.AreEqual(forest1.Nodes[0].Children[0].Children[0].Children[0].Value.Value, "L2");

            Assert.AreEqual(forest1.Nodes[0].Value.Children.Length, 2);
            Assert.AreEqual(forest1.Nodes[0].Value.Children[0].Children.Length, 3);
            Assert.AreEqual(forest1.Nodes[0].Value.Children[0].Children[0].Children.Length, 1);
            Assert.AreEqual(forest1.Nodes[0].Value.Children[0].Children[0].Children[0].Children.Length, 0);
            Assert.AreEqual(forest1.Nodes[0].Value.Children[0].Children[1].Children.Length, 0);
            Assert.AreEqual(forest1.Nodes[0].Value.Children[0].Children[2].Children.Length, 0);
            Assert.AreEqual(forest1.Nodes[0].Value.Children[1].Children.Length, 1);
            Assert.AreEqual(forest1.Nodes[0].Value.Children[1].Children[0].Children.Length, 0);
            Assert.AreEqual(forest1.Nodes[0].Children[0].Value.Children.Length, 3);
            Assert.AreEqual(forest1.Nodes[0].Children[0].Value.Children[0].Children.Length, 1);
            Assert.AreEqual(forest1.Nodes[0].Children[0].Value.Children[0].Children[0].Children.Length, 0);
            Assert.AreEqual(forest1.Nodes[0].Children[0].Value.Children[1].Children.Length, 0);
            Assert.AreEqual(forest1.Nodes[0].Children[0].Value.Children[2].Children.Length, 0);
            Assert.AreEqual(forest1.Nodes[0].Children[1].Value.Children.Length, 1);
            Assert.AreEqual(forest1.Nodes[0].Children[1].Value.Children[0].Children.Length, 0);
            Assert.AreEqual(forest1.Nodes[0].Children[0].Children[0].Value.Children.Length, 1);
            Assert.AreEqual(forest1.Nodes[0].Children[0].Children[0].Value.Children[0].Children.Length, 0);
            Assert.AreEqual(forest1.Nodes[0].Children[0].Children[1].Value.Children.Length, 0);
            Assert.AreEqual(forest1.Nodes[0].Children[0].Children[2].Value.Children.Length, 0);
            Assert.AreEqual(forest1.Nodes[0].Children[1].Children[0].Value.Children.Length, 0);
            Assert.AreEqual(forest1.Nodes[0].Children[0].Children[0].Children[0].Value.Children.Length, 0);

            Assert.IsInstanceOfType(forest1.Nodes[0], typeof(IRoot));
            Assert.IsInstanceOfType(forest1.Nodes[0].Children[0], typeof(IBranch));
            Assert.IsInstanceOfType(forest1.Nodes[0].Children[0].Children[0], typeof(IBranch));
            Assert.IsInstanceOfType(forest1.Nodes[0].Children[0].Children[0].Children[0], typeof(ILeaf));
            Assert.IsInstanceOfType(forest1.Nodes[0].Children[0].Children[1], typeof(ILeaf));
            Assert.IsInstanceOfType(forest1.Nodes[0].Children[0].Children[2], typeof(ILeaf));
            Assert.IsInstanceOfType(forest1.Nodes[0].Children[1], typeof(IBranch));
            Assert.IsInstanceOfType(forest1.Nodes[0].Children[1].Children[0], typeof(ILeaf));

            Assert.AreEqual(forest2.Nodes[0].Value.Value, "R1");
            Assert.AreEqual(forest2.Nodes[0].Value.Children[0].Value, "B1");
            Assert.AreEqual(forest2.Nodes[0].Value.Children[0].Children[0].Value, "B2");
            Assert.AreEqual(forest2.Nodes[0].Value.Children[0].Children[0].Children[0].Value, "L2");
            Assert.AreEqual(forest2.Nodes[0].Value.Children[0].Children[1].Value, "L1");
            Assert.AreEqual(forest2.Nodes[0].Value.Children[0].Children[2].Value, "L4");
            Assert.AreEqual(forest2.Nodes[0].Value.Children[1].Value, "B3");
            Assert.AreEqual(forest2.Nodes[0].Value.Children[1].Children[0].Value, "L3");
            Assert.AreEqual(forest2.Nodes[0].Children[0].Value.Value, "B1");
            Assert.AreEqual(forest2.Nodes[0].Children[0].Value.Children[0].Value, "B2");
            Assert.AreEqual(forest2.Nodes[0].Children[0].Value.Children[0].Children[0].Value, "L2");
            Assert.AreEqual(forest2.Nodes[0].Children[0].Value.Children[1].Value, "L1");
            Assert.AreEqual(forest2.Nodes[0].Children[0].Value.Children[2].Value, "L4");
            Assert.AreEqual(forest2.Nodes[0].Children[1].Value.Value, "B3");
            Assert.AreEqual(forest2.Nodes[0].Children[1].Value.Children[0].Value, "L3");
            Assert.AreEqual(forest2.Nodes[0].Children[0].Children[0].Value.Value, "B2");
            Assert.AreEqual(forest2.Nodes[0].Children[0].Children[0].Value.Children[0].Value, "L2");
            Assert.AreEqual(forest2.Nodes[0].Children[0].Children[1].Value.Value, "L1");
            Assert.AreEqual(forest2.Nodes[0].Children[0].Children[2].Value.Value, "L4");
            Assert.AreEqual(forest2.Nodes[0].Children[1].Children[0].Value.Value, "L3");
            Assert.AreEqual(forest2.Nodes[0].Children[0].Children[0].Children[0].Value.Value, "L2");

            Assert.AreEqual(forest2.Nodes[0].Value.Children.Length, 2);
            Assert.AreEqual(forest2.Nodes[0].Value.Children[0].Children.Length, 3);
            Assert.AreEqual(forest2.Nodes[0].Value.Children[0].Children[0].Children.Length, 1);
            Assert.AreEqual(forest2.Nodes[0].Value.Children[0].Children[0].Children[0].Children.Length, 0);
            Assert.AreEqual(forest2.Nodes[0].Value.Children[0].Children[1].Children.Length, 0);
            Assert.AreEqual(forest2.Nodes[0].Value.Children[0].Children[2].Children.Length, 0);
            Assert.AreEqual(forest2.Nodes[0].Value.Children[1].Children.Length, 1);
            Assert.AreEqual(forest2.Nodes[0].Value.Children[1].Children[0].Children.Length, 0);
            Assert.AreEqual(forest2.Nodes[0].Children[0].Value.Children.Length, 3);
            Assert.AreEqual(forest2.Nodes[0].Children[0].Value.Children[0].Children.Length, 1);
            Assert.AreEqual(forest2.Nodes[0].Children[0].Value.Children[0].Children[0].Children.Length, 0);
            Assert.AreEqual(forest2.Nodes[0].Children[0].Value.Children[1].Children.Length, 0);
            Assert.AreEqual(forest2.Nodes[0].Children[0].Value.Children[2].Children.Length, 0);
            Assert.AreEqual(forest2.Nodes[0].Children[1].Value.Children.Length, 1);
            Assert.AreEqual(forest2.Nodes[0].Children[1].Value.Children[0].Children.Length, 0);
            Assert.AreEqual(forest2.Nodes[0].Children[0].Children[0].Value.Children.Length, 1);
            Assert.AreEqual(forest2.Nodes[0].Children[0].Children[0].Value.Children[0].Children.Length, 0);
            Assert.AreEqual(forest2.Nodes[0].Children[0].Children[1].Value.Children.Length, 0);
            Assert.AreEqual(forest2.Nodes[0].Children[0].Children[2].Value.Children.Length, 0);
            Assert.AreEqual(forest2.Nodes[0].Children[1].Children[0].Value.Children.Length, 0);
            Assert.AreEqual(forest2.Nodes[0].Children[0].Children[0].Children[0].Value.Children.Length, 0);

            Assert.IsInstanceOfType(forest2.Nodes[0], typeof(IRoot));
            Assert.IsInstanceOfType(forest2.Nodes[0].Children[0], typeof(IBranch));
            Assert.IsInstanceOfType(forest2.Nodes[0].Children[0].Children[0], typeof(IBranch));
            Assert.IsInstanceOfType(forest2.Nodes[0].Children[0].Children[0].Children[0], typeof(ILeaf));
            Assert.IsInstanceOfType(forest2.Nodes[0].Children[0].Children[1], typeof(ILeaf));
            Assert.IsInstanceOfType(forest2.Nodes[0].Children[0].Children[2], typeof(ILeaf));
            Assert.IsInstanceOfType(forest2.Nodes[0].Children[1], typeof(IBranch));
            Assert.IsInstanceOfType(forest2.Nodes[0].Children[1].Children[0], typeof(ILeaf));
        }

        private static ImmutableForest<string> BuildForestFromFlatData()
        {
            var nodeList = new[]
            {
                TreeItemData.Create(1, null, "R1"),
                TreeItemData.Create(2, 1, "B1"),
                TreeItemData.Create(3, 2, "B2"),
                TreeItemData.Create(4, 1, "B3"),
                TreeItemData.Create(5, 2, "L1"),
                TreeItemData.Create(6, 3, "L2"),
                TreeItemData.Create(7, 4, "L3"),
                TreeItemData.Create(8, 2, "L4"),
            };

            var dicChildren = nodeList
                .Where(n => n.ParentId != null)
                .GroupBy(n => n.ParentId)
                // ReSharper disable once PossibleInvalidOperationException
                .ToDictionary(g => g.Key.Value, n => n.ToList());

            var builder = new ImmutableTreeBuilder();
            var forest = builder.BuildForest(
                x => dicChildren.GetValueOrDefault(x.Id, new List<TreeItemData<string>>()),
                x => x.Value,
                nodeList.Where(x => x.ParentId == null));
            return forest;
        }

        private static ImmutableForest<Giraffe> BuildForestWithGirafes()
        {
            var nodeList = new[]
            {
                TreeItemData.Create(1, null, new Giraffe(1)),
                TreeItemData.Create(2, 1, new Giraffe(2)),
                TreeItemData.Create(3, 1, new Giraffe(3)),
                TreeItemData.Create(4, 2, new Giraffe(4)),
                TreeItemData.Create(5, 4, new Giraffe(5)),
            };

            var dicChildren = nodeList
                .Where(n => n.ParentId != null)
                .GroupBy(n => n.ParentId)
                // ReSharper disable once PossibleInvalidOperationException
                .ToDictionary(g => g.Key.Value, n => n.ToList());

            var builder = new ImmutableTreeBuilder();
            var forest = builder.BuildForest(
                x => dicChildren.GetValueOrDefault(x.Id, new List<TreeItemData<Giraffe>>()),
                x => x.Value,
                nodeList.Where(x => x.ParentId == null));

            return forest;
        }
    }

    public static class EnumerableExtensions
    {
        public static T MatchNull<T>(this T value, Func<T> whenNull)
        {
            if (whenNull == null)
                throw new ArgumentNullException("whenNull");

            if (value == null)
                return whenNull();

            return value;
        }

        public static IEnumerable<T> MatchNullWithEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return Enumerable.Empty<T>();

            return enumerable;
        }
    }
}
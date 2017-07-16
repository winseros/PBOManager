using System.Collections.Generic;
using Infrastructure;
using NUnit.Framework;
using PboManager.Components.PboTree;

namespace Test.PboManager.Components.PboTree
{
    public class PboNodeComparerTest
    {
        [Test]
        public void Test_Compare_Sorts_A_List()
        {
            var substitute = new AutoSubstitute();

            var node1 = substitute.Resolve<PboNodeModel>();
            var node2 = substitute.Resolve<PboNodeModel>();
            var node3 = substitute.Resolve<PboNodeModel>();

            node1.Name = "A"; //Directory
            node1.Children.Add(substitute.Resolve<PboNodeModel>());
            node2.Name = "B"; //Directory
            node2.Children.Add(substitute.Resolve<PboNodeModel>());
            node3.Name = "A"; //File

            var nodes = new List<PboNodeModel> {node3, node2, node1};
            nodes.Sort(new PboNodeComparer());

            Assert.AreSame(node1, nodes[0]);
            Assert.AreSame(node2, nodes[1]);
            Assert.AreSame(node3, nodes[2]);
        }

        [Test]
        public void Test_Compare_Does_Not_Throw_If_Collection_Contains_Nulls()
        {
            var nodes = new List<PboNodeModel> {null, null, null};
            Assert.DoesNotThrow(() => nodes.Sort(new PboNodeComparer()));
        }
    }
}
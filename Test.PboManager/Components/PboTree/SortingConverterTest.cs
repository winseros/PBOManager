using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Infrastructure;
using NUnit.Framework;
using PboManager.Components.PboTree;

namespace Test.PboManager.Components.PboTree
{
    public class SortingConverterTest
    {
        [Test]
        public void Test_Convert_Returns_Sorted_View()
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

            var sorted = (ListCollectionView)SortingConverter.INSTANCE.Convert(nodes, null, null, CultureInfo.CurrentCulture);
            Assert.AreEqual(0, sorted.IndexOf(node1));
            Assert.AreEqual(1, sorted.IndexOf(node2));
            Assert.AreEqual(2, sorted.IndexOf(node3));
        }
    }
}

using LightInject;
using NSubstitute;
using NUnit.Framework;

namespace Test.PboManager
{
    public class Class1
    {
        [Test]
        public void Test_Generic_NSubstitute()
        {
            var factoty = Substitute.For<IServiceFactory>();
            factoty.GetInstance<string>().Returns("qwerty");

            var str = factoty.GetInstance<string>();
            Assert.AreEqual("qwerty", str);
        }
    }
}

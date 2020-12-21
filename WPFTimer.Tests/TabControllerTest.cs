using NUnit.Framework;

namespace WPFTimer.Tests
{
    [TestFixture]
    public class TabControllerTest {
        [Test]
        public void Test_MaximumTabsCount() {
            TabController tabController = new TabController();
            for (int i = 0; i < 20; i++)
                tabController.AddTab.Execute(null);
            Assert.AreEqual(10, tabController.TabCollection.Count);
        }
    }
}
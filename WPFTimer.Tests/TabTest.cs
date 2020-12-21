using NUnit.Framework;

namespace WPFTimer.Tests
{
    [TestFixture]
    class TabTest {
        private TabController tabController;
        [SetUp]
        public void SetUp() {
            tabController = new TabController();
        }
        [Test]
        public void RemoveTab() {
            tabController.AddTab.Execute(null);
            tabController.AddTab.Execute(null);
            tabController.AddTab.Execute(null);
            Assert.AreEqual(4, tabController.TabCollection.Count);
            for (int i = 0; i < 4; i++) {
                tabController.TabCollection[0].RemoveCommand.Execute(null);
            }
            Assert.AreEqual(1, tabController.TabCollection.Count);
        }
        
        [Test]
        public void RemoveTabWorkingTimer() {
            tabController.AddTab.Execute(null);
            tabController.AddTab.Execute(null);
            tabController.AddTab.Execute(null);
            Assert.AreEqual(4, tabController.TabCollection.Count);

            tabController.TabCollection[0].StartCommand.Execute(null);
            tabController.TabCollection[1].StartCommand.Execute(null);

            bool wasRemoved = false; 
            tabController.TabCollection.CollectionChanged += (sender, e) => { wasRemoved = true; };

            for (int i = 0; i < tabController.TabCollection.Count; i++) {
                tabController.TabCollection[i].RemoveCommand.Execute(null);
                if (wasRemoved){
                    i--;
                    wasRemoved = false; 
                }
            }
            Assert.AreEqual(2, tabController.TabCollection.Count);
        }
        [TearDown]
        public void CleanUP() {
            tabController = null;
        }
    }
}

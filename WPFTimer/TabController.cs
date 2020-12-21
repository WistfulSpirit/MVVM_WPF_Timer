using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTimer
{
    public class TabController
    {
        private ObservableCollection<Tab> tabCollection;
        private int maxTabs;
        private int count;
        public TabController()
        {
            count = 1;
            maxTabs = 10;
            tabCollection = new ObservableCollection<Tab>();
            tabCollection.Add(new Tab(count, this));
            count++;
        }
        public ObservableCollection<Tab> TabCollection
        {
            get { return tabCollection; }
        }
        public int SelectedIndex { get; set; } = 0;
        private DelegateCommand addTab;
        public DelegateCommand AddTab
        {
            get
            {
                return addTab ?? (addTab = new DelegateCommand((x) =>
                {
                    tabCollection.Add(new Tab(count, this));
                    count++;
                },
                (x) =>
                {
                    return tabCollection.Count < maxTabs;
                }));
            }
        }
        public void RemoveItem(Tab item)
        {
            tabCollection.Remove(item);
        }
    }
}

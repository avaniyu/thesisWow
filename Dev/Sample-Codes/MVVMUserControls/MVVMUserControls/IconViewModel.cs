using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMUserControls
{
    internal class IconViewModel : BindableBase
    {
        private ObservableCollection<IconInfo> _IconInfos;

        public IconViewModel()
        {
            AscSortCommand = new RelayCommand(OnSortAsc);
            DescSortCommand = new RelayCommand(OnSortDesc);
        }

        public RelayCommand AscSortCommand { get; set; }
        public RelayCommand DescSortCommand { get; set; }

        public ObservableCollection<IconInfo> IconInfos
        {
            get { return _IconInfos; }
            set { SetProperty(ref _IconInfos, value); }
        }

        private void OnSortAsc()
        {
            if (IconInfos != null)
                IconInfos = new ObservableCollection<IconInfo>(
                    IconInfos.OrderBy(i => i.Label));
        }
        private void OnSortDesc()
        {
            if (IconInfos != null)
                IconInfos = new ObservableCollection<IconInfo>(
                    IconInfos.OrderByDescending(i=>i.Label));
        }
    }
}

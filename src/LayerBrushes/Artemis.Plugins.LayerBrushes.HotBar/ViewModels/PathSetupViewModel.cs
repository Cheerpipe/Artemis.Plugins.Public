using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Artemis.Plugins.LayerBrushes.Hotbar.Services;
using Artemis.UI.Shared.Services;
using GongSolutions.Wpf.DragDrop;

namespace Artemis.Plugins.LayerBrushes.Hotbar.ViewModels
{
    public class PathSetupViewModel : DialogViewModelBase, IDropTarget
    {
        private ObservableCollection<PersistentLed> _orderedLeds;

        public PathSetupViewModel(List<PersistentLed> sortedLeds)
        {
            _orderedLeds = new ObservableCollection<PersistentLed>(sortedLeds);
            OrderedLeds = CollectionViewSource.GetDefaultView(_orderedLeds);
        }

        public ICollectionView OrderedLeds { get; private set; }

        public void Save()
        {
            if (Session != null && !Session.IsEnded)
                Session.Close(_orderedLeds.ToList());
        }

        public void Close()
        {
            this.Cancel();
        }

        #region IDropTarget Interface Methods
        public void DragOver(IDropInfo dropInfo) { }

        public void Drop(IDropInfo dropInfo) { }

        #endregion
    }
}
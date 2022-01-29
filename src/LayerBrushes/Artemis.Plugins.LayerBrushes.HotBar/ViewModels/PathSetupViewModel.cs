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
        private ObservableCollection<PersistentLed> _orderedLedsObservableCollection;

        public PathSetupViewModel(List<PersistentLed> sortedLeds)
        {
            _orderedLedsObservableCollection = new ObservableCollection<PersistentLed>(sortedLeds);
            OrderedLedsCollectionView = CollectionViewSource.GetDefaultView(_orderedLedsObservableCollection);
        }

        public ICollectionView OrderedLedsCollectionView { get; private set; }

        public void Save()
        {
            if (Session != null && !Session.IsEnded)
                Session.Close(_orderedLedsObservableCollection.ToList());
        }

        public void Close()
        {
            Cancel();
        }

        #region IDropTarget Interface Methods
        public void DragOver(IDropInfo dropInfo) { }

        public void Drop(IDropInfo dropInfo) { }

        #endregion
    }
}
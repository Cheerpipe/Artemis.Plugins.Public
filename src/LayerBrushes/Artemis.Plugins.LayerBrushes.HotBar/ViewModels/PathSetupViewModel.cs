using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Artemis.Core;
using Artemis.Plugins.LayerBrushes.Hotbar.LayerBrush;
using Artemis.Plugins.LayerBrushes.Hotbar.LayerProperties;
using Artemis.Plugins.LayerBrushes.Hotbar.Services;
using Artemis.UI.Shared.LayerBrushes;
using GongSolutions.Wpf.DragDrop;

namespace Artemis.Plugins.LayerBrushes.Hotbar.ViewModels
{
    public class PathSetupViewModel : BrushConfigurationViewModel, IDropTarget
    {
        private ObservableCollection<PersistentLed> _orderedLeds;

        public PathSetupViewModel(HotbarLayerBrush layerBrush) : base(layerBrush)
        {
            Properties = layerBrush.Properties;
            if (Properties.LedSortMap?.CurrentValue == null)
            {
                _orderedLeds = new ObservableCollection<PersistentLed>(GetNewLayerLedCollection(layerBrush));
            }
            else
            {
                _orderedLeds = new ObservableCollection<PersistentLed>(Properties.LedSortMap.CurrentValue);
            }

            OrderedLeds = CollectionViewSource.GetDefaultView(_orderedLeds);
        }

        public List<PersistentLed> GetNewLayerLedCollection(HotbarLayerBrush layerBrush)
        {
            List<PersistentLed> ledsPath = new List<PersistentLed>();
            var leds = layerBrush.Layer.Leds;
            int pos = 0;

            foreach (ArtemisLed led in leds)
            {
                ledsPath.Add(new PersistentLed(led.RgbLed.Id, led.Device.Identifier,led.RgbLed.Device.DeviceInfo.DeviceName));
                pos++;
            }
            return ledsPath;
        }

        public ICollectionView OrderedLeds { get; private set; }

        protected override void OnClose()
        {
            Properties.LedSortMap.BaseValue = _orderedLeds.ToList();
        }

        public HotbarLayerBrushProperties Properties { get; }

        #region IDropTarget Interface Methods
        public void DragOver(IDropInfo dropInfo) { }

        public void Drop(IDropInfo dropInfo) { }
        
        #endregion
    }
}
using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Core.Services;
using Artemis.Plugins.LayerBrushes.Hotbar.LayerProperties;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Artemis.Plugins.LayerBrushes.Hotbar.Services;
using Artemis.Plugins.LayerBrushes.Hotbar.ViewModels;
using Artemis.UI.Shared.LayerBrushes;

namespace Artemis.Plugins.LayerBrushes.Hotbar.LayerBrush
{
    public class HotbarLayerBrush : PerLedLayerBrush<HotbarLayerBrushProperties>
    {
        #region  Variables

        private readonly IInputService _inputService;
        private readonly PersistentLedService _persistentLedService;
        private List<ArtemisLed> _sortedLeds;

        private ArtemisLed _activeLed;

        #endregion

        #region Constructor

        public HotbarLayerBrush(IInputService inputService, PersistentLedService persistentLedService)
        {
            _inputService = inputService;
            _persistentLedService = persistentLedService;
        }

        #endregion

        #region Pluging lifecicle methods

        public override void EnableLayerBrush()
        {
            // EnableLayerBrush is running twice O.O. Have to check with Spoinky. In the meanwhile...
            _inputService.KeyboardKeyDown -= InputServiceOnKeyboardKeyDown;
            _inputService.MouseScroll -= _inputService_MouseScroll;
            Layer.RenderPropertiesUpdated -= Layer_RenderPropertiesUpdated;
            Properties.LedSortMap.PropertyChanged -= LedSortMap_PropertyChanged;


            _inputService.KeyboardKeyDown += InputServiceOnKeyboardKeyDown;
            _inputService.MouseScroll += _inputService_MouseScroll;
            Layer.RenderPropertiesUpdated += Layer_RenderPropertiesUpdated;
            Properties.LedSortMap.PropertyChanged += LedSortMap_PropertyChanged;
        }

        private void LedSortMap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetCustomSortedLeds();
        }

        private void Layer_RenderPropertiesUpdated(object sender, EventArgs e)
        {
            _activeLed = null;

            if (_sortedLeds == null)
            {
                //Properties.LedSortMap.BaseValue GetNewLayerLedCollection(Layer.Leds);
                SetCustomSortedLeds();
            }
            else
            {
                Properties.LedSortMap.BaseValue = GetNewLayerLedCollection(Layer.Leds);
            }
        }

        private List<PersistentLed> GetNewLayerLedCollection(IReadOnlyCollection<ArtemisLed> layerLeds)
        {
            List<PersistentLed> ledsPath = new List<PersistentLed>();
            int pos = 0;

            foreach (ArtemisLed led in layerLeds)
            {
                ledsPath.Add(new PersistentLed(led.RgbLed.Id, led.Device.Identifier, led.RgbLed.Device.DeviceInfo.DeviceName));
                pos++;
            }
            return ledsPath;
        }

        public override void DisableLayerBrush()
        {
            _inputService.KeyboardKeyDown -= InputServiceOnKeyboardKeyDown;
            _inputService.MouseScroll -= _inputService_MouseScroll;
            Layer.RenderPropertiesUpdated -= Layer_RenderPropertiesUpdated;
            Properties.LedSortMap.PropertyChanged -= LedSortMap_PropertyChanged;
        }

        #endregion

        #region Pluging behavior methods

        public override void Update(double deltaTime)
        {
        }

        public override SKColor GetColor(ArtemisLed led, SKPoint renderPoint)
        {

            if (led == _activeLed)
            {
                if (Properties.ColorMode == KeyColorType.Solid)
                    return Properties.ActiveKeyColor.CurrentValue;
                else
                {
                    float colorPos = (float)GetOrderedLeds().IndexOf(led) / (Layer.Leds.Count - 1);
                    return Properties.ActiveKeyGradient.CurrentValue.GetColor(colorPos);
                }
            }
            else
            {
                if (Properties.PaintBackground.CurrentValue)
                    return SKColors.Black;
                else
                    return SKColors.Transparent;
            }
        }

        #endregion

        #region Event handlers

        private void InputServiceOnKeyboardKeyDown(object sender, ArtemisKeyboardKeyEventArgs e)
        {
            if (e.Led == null)
                return;

            if (!Layer.Leds.Contains(e.Led))
                return;

            _activeLed = e.Led;
        }

        private List<ArtemisLed> GetOrderedLeds()
        {
            // Order LEDs by their position to create a nice revealing effect from left top right, top to bottom
            // Copied from LayerRevealEffectLayer
            return Properties.LedOrder.CurrentValue switch
            {
                LedOrder.LedId => Layer.Leds.OrderBy(l => l.Device.Rectangle.Left).ThenBy(l => l.Device.Rectangle.Top).ThenBy(l => l.RgbLed.Id).ToList(),
                LedOrder.Vertical => Layer.Leds.OrderBy(l => l.AbsoluteRectangle.Left).ThenBy(l => l.AbsoluteRectangle.Top).ToList(),
                LedOrder.Horizontal => Layer.Leds.OrderBy(l => l.AbsoluteRectangle.Top).ThenBy(l => l.AbsoluteRectangle.Left).ToList(),
                LedOrder.VerticalReversed => Layer.Leds.OrderByDescending(l => l.AbsoluteRectangle.Left).ThenByDescending(l => l.AbsoluteRectangle.Top).ToList(),
                LedOrder.HorizontalReversed => Layer.Leds.OrderByDescending(l => l.AbsoluteRectangle.Top).ThenByDescending(l => l.AbsoluteRectangle.Left).ToList(),
                LedOrder.Custom => GetCustomSortedLeds(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private List<ArtemisLed> GetCustomSortedLeds()
        {
            return _sortedLeds;
        }

        private void SetCustomSortedLeds()
        {
            var sortedLeds = _persistentLedService.GetSortedLedsFromMap(Layer.Leds, Properties.LedSortMap?.BaseValue);
            _sortedLeds = sortedLeds ?? Layer.Leds.OrderBy(l => l.Device.Rectangle.Left).ThenBy(l => l.Device.Rectangle.Top).ThenBy(l => l.RgbLed.Id).ToList();
        }

        private void _inputService_MouseScroll(object sender, ArtemisMouseScrollEventArgs e)
        {
            if (!Properties.UseScroll.CurrentValue)
                return;

            List<ArtemisLed> leds = GetOrderedLeds();

            if ((_activeLed == null || !leds.Contains(_activeLed)) && Properties.ScrollActivation.CurrentValue)
            {
                _activeLed = leds.FirstOrDefault();
            }
            else if ((_activeLed == null || !leds.Contains(_activeLed)) && !Properties.ScrollActivation.CurrentValue)
                return;

            int currentLedPos = leds.IndexOf(_activeLed);
            int newLedPos;

            // Get the Key
            if (e.IsScrollingUp)
            {
                if (Properties.LoopOnScrollOverflow.CurrentValue)
                    newLedPos = (currentLedPos == leds.Count - 1) ? 0 : currentLedPos + 1;
                else
                    newLedPos = (currentLedPos == leds.Count - 1) ? currentLedPos : currentLedPos + 1;
            }
            else
            {
                if (Properties.LoopOnScrollOverflow.CurrentValue)
                    newLedPos = (currentLedPos == 0) ? leds.Count - 1 : currentLedPos - 1;
                else
                    newLedPos = (currentLedPos == 0) ? 0 : currentLedPos - 1;
            }
            _activeLed = leds.Count > 0 ? leds[newLedPos] : null;
        }

        #endregion
    }
}
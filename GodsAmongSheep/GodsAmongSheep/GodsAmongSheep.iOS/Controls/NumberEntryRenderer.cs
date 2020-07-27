using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(GodsAmongSheep.Controls.NumberEntry),
    typeof(GodsAmongSheep.iOS.Controls.NumberEntryRenderer))]
namespace GodsAmongSheep.iOS.Controls
{
    public class NumberEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.KeyboardType = UIKit.UIKeyboardType.NumberPad;
            }
        }
    }
}
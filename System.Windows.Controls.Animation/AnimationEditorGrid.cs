using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace System.Windows.Controls.Animation
{
    public class AnimationEditorGrid : Grid
    {
        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            RectangleGeometry geo = base.GetLayoutClip(layoutSlotSize) as RectangleGeometry;
            if (geo != null)
            {
                Rect newBounds = geo.Bounds;
                newBounds.Y -= 24;
                newBounds.Height += 24;
                return new RectangleGeometry(newBounds, geo.RadiusX, geo.RadiusY, geo.Transform);
            }
            else
            {
                return null;
            }
        }
    }
}

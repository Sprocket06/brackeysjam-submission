﻿﻿using Chroma.Graphics;

namespace Projection.UI
{
    public class SolidColorBrush : Brush
    {
        public Color Color { get; set; }

        public SolidColorBrush(Color color)
        {
            Color = color;
        }
    }
}
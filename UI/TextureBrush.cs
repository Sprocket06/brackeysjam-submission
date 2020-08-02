﻿using Chroma.Graphics;

namespace Projection.UI
{
    public class TextureBrush : Brush
    {
        public Texture Texture { get; set; }

        public TextureBrush(Texture texture)
        {
            Texture = texture;
        }
    }
}
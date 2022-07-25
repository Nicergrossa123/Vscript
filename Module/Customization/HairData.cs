using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Module.Customization
{
    public class HairData
    {
        public int Hair;
        public byte Color;
        public byte HighlightColor;

        public HairData(int hair, byte color, byte highlightcolor)
        {
            Hair = hair;
            Color = color;
            HighlightColor = highlightcolor;
        }
    }
}
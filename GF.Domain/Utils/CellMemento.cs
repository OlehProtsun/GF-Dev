using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Domain.Utils
{
    public sealed class CellMemento
    {
        public Point Key;                  // ColumnIndex, RowIndex
        public Color Back, Fore;
        public Color SelBack, SelFore;
        public bool HadBorder;
        public Color BorderColor;
        public bool HadCustomFill;        // у _customFilled
    }
}

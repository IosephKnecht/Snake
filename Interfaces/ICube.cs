using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsFormsApp18.Interfaces
{
    interface ICube
    {
        void UpdateIJ();
        float _X { get; set; }
        float _Y { get; set; }
        Brush color { get; set; }
        int colorNum { get; }
    }
}

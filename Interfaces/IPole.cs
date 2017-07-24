using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsFormsApp18.Interfaces
{
    interface IPole:IDisposable
    {
        Bitmap Paint();
        Bitmap btm { get; set; }

        event Action UpdateForm;
    }
}

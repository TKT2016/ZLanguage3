using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Z标准包.桌面控件
{
    public class ZForm:Form
    {
        public void Add(Control control)
        {
            this.Controls.Add(control);
        }
    }
}

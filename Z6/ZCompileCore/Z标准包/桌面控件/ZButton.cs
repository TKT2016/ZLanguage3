using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Z标准包.桌面控件
{
    public class ZButton : Button
    {
        public ZButton()
        {
            this.Click += new System.EventHandler(this.eventClick);
        }

        private void eventClick(object sender, EventArgs e)
        {
             if(ClickAction!=null)
             {
                 ClickAction();
             }
        }

        public Action ClickAction { get; set; }

    }
}

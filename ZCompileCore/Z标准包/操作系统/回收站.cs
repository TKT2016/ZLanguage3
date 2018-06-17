using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using ZLangRT.Attributes;
using System.Runtime.InteropServices;

namespace Z标准包.操作系统
{
    [ZStatic]
    public static class 回收站
    {
        const int SHERB_NOCONFIRMATION = 0x000001;   //不显示确认删除的对话框  
        const int SHERB_NOPROGRESSUI = 0x000002;     //不显示删除过程的进度条  
        const int SHERB_NOSOUND = 0x000004;          //当删除完成时,不播放声音  
  
        [DllImportAttribute("shell32.dll")]          //声明API函数  
        private static extern int SHEmptyRecycleBin(IntPtr handle, string root, int falgs);

        [ZCode("清空")]
        public static void 清空()
        {  
            //SHEmptyRecycleBin(this.Handle, "", SHERB_NOCONFIRMATION + SHERB_NOPROGRESSUI + SHERB_NOSOUND);  
            SHEmptyRecycleBin( IntPtr.Zero, "", SHERB_NOCONFIRMATION + SHERB_NOPROGRESSUI + SHERB_NOSOUND);  
        }
    }
}

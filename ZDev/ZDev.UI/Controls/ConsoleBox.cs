using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
namespace ZDev.UI.Controls
{
    class ConsoleBox  : TextBox
	{
        public ConsoleBox()
			: base()
		{
			this.Multiline = true;
		    this.BackColor = Color.Blue;
            this.ForeColor = Color.White;
			this.KeyPress += new KeyPressEventHandler(Console_KeyPress);
            /*
            Process CurrentProcess = Process.GetCurrentProcess();
            CurrentProcess.StartInfo.UseShellExecute = false;
            CurrentProcess.StartInfo.CreateNoWindow = true;
            CurrentProcess.StartInfo.RedirectStandardOutput = true;
            CurrentProcess.StartInfo.RedirectStandardInput = true;
            CurrentProcess.StartInfo.RedirectStandardError = true;

            this.OutputStream = CurrentProcess.StandardOutput;
            this.InputStream = CurrentProcess.StandardInput;*/
		}

		private void Console_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (InputStream != null)
			{
				if ((int)e.KeyChar == 13)
				{
					InputStream.WriteLine();
				}
				else
				{
					InputStream.Write(e.KeyChar);
				}
				InputStream.Flush();
			}
		}

		public void Start()
		{
			this.Text = string.Empty;
			if (OutputStream != null)
			{
				MethodInvoker readOutputStream = new MethodInvoker(ReadOutput);
				readOutputStream.BeginInvoke(null, null);
			}
		}

		public void ReadOutput()
		{
			int value;
			while ((value = OutputStream.Read()) != -1)
			{
				Invoke((MethodInvoker)delegate
				{
					this.Text += (char)value;
				});
			}
		}

		public StreamWriter InputStream
		{
			get;
			set;
		}

		public StreamReader OutputStream
		{
			get;
			set;
		}
	}
}

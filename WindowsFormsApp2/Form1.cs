using CefSharp;
using CefSharp.WinForms;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        ChromiumWebBrowser browser;

        System.Timers.Timer timer;
        int i = 1;

        public Form1()
        {
            InitializeComponent();

            timer = new System.Timers.Timer(400) { AutoReset = true };
            timer.Elapsed += Timer_Elapsed;

            browser = new ChromiumWebBrowser("www.baidu.com") { Parent = splitContainer1.Panel1, Dock = DockStyle.Fill };
            browser.LifeSpanHandler = new LifeSpanHandler(splitContainer1.Panel2);
            browser.FrameLoadEnd += Browser_FrameLoadEnd;
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain == false) return;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            i = i > 1 ? 1 : i + 1;
            browser.ExecuteScriptAsync("document.getElementsByClassName('mnav c-font-normal c-color-t')[" + (i) + "].click()");
        }
    }


    class LifeSpanHandler : ILifeSpanHandler
    {
        Control host;

        internal LifeSpanHandler(Control host)
        {
            this.host = host;
        }

        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return false;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;

            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;
            webBrowser.Invoke(new Action(() =>
            {
                if (host.HasChildren) host.Controls[0].Dispose();
                var control = new Control { Parent = host, Dock = DockStyle.Fill };
                control.CreateControl();
                windowInfo.SetAsChild(control.Handle);
            }));

            return false;
        }
    }
}

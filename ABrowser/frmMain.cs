using CefSharp;
using CefSharp.WinForms;
using System;
using System.Text.Json;
using System.Windows.Forms;

namespace ABrowser
{
    public partial class frmMain : Form
    {
        ChromiumWebBrowser browser;
        private string _localStorageData;
        private string _sessionStorageData;

        public frmMain()
        {
            InitializeComponent();
            splitContainer1.Panel1Collapsed = true;
            splitContainer1.SplitterDistance = 250;
            localStorage.Width = 500;
            sessionStorage.Width = 500;
        }

        private void searchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (IsUrl(searchBox.Text))
                {
                    browser.Load(searchBox.Text);
                }
                else
                {
                    string searchUrl = $"https://www.google.com/search?q={Uri.EscapeDataString(searchBox.Text)}";
                    browser.Load(searchUrl);
                }
            }
        }

        public bool IsUrl(string input)
        {
            return Uri.IsWellFormedUriString(input, UriKind.Absolute);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            browser = new ChromiumWebBrowser();
            browser.Dock = DockStyle.Fill;
            browser.AddressChanged += Browser_AddressChanged;
            browser.FrameLoadEnd += Browser_FrameLoadEnd;
            browser.Load("https://www.google.com");
            splitContainer1.Panel2.Controls.Add(browser);
        }

        private async void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                var localStorageValue = await browser.EvaluateScriptAsync("JSON.stringify(localStorage);");
                var sessionStorageValue = await browser.EvaluateScriptAsync("JSON.stringify(sessionStorage);");

                // Access the values returned by the JavaScript code
                _localStorageData = localStorageValue.Result?.ToString();
                _sessionStorageData = sessionStorageValue.Result?.ToString();

                // Deserialize the JSON string to an object for pretty printing
                var jsonObject = JsonSerializer.Deserialize<object>(_localStorageData);

                // Convert the object back to a formatted JSON string
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true // Enable indentation for a formatted view
                };
                _localStorageData = JsonSerializer.Serialize(jsonObject, options);
            }

            if (localStorage.InvokeRequired)
            {
                localStorage.Invoke((MethodInvoker)(() => localStorage.Text = _localStorageData ?? string.Empty));
            }
            else
            {
                localStorage.Text = _localStorageData ?? string.Empty;
            }

            if (sessionStorage.InvokeRequired)
            {
                sessionStorage.Invoke((MethodInvoker)(() => sessionStorage.Text = _sessionStorageData ?? string.Empty));
            }
            else
            {
                sessionStorage.Text = _sessionStorageData ?? string.Empty;
            }
        }

        private void UpdateTextBoxText(string text)
        {
            if (searchBox.InvokeRequired)
            {
                searchBox.Invoke((MethodInvoker)(() => searchBox.Text = text));
            }
            else
            {
                searchBox.Text = text;
            }
        }


        private async void Browser_AddressChanged(object sender, CefSharp.AddressChangedEventArgs e)
        {
            UpdateTextBoxText(e.Address.ToString());

        }

        private void button3_Click(object sender, EventArgs e)
        {
            browser.Load("https://www.google.com");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            browser.Back();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            browser.Forward();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            browser.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Net;
using HtmlAgilityPack;
using System.IO;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;

namespace Service
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            key.SetValue("Service.exe", "\"" + Application.ExecutablePath + "\"");


        }
        IWebDriver driver;
        public async void DriverBaslat()
        {

            ChromeOptions options = new ChromeOptions();
            options.AddArgument(@"user-data-dir=" + Application.StartupPath + "\\Profile");
            options.AddArgument("--headless");
            options.AddExcludedArgument("enable-automation");

            ChromeDriverService cds = ChromeDriverService.CreateDefaultService();
            cds.HideCommandPromptWindow = true;
            driver = new ChromeDriver(cds, options);

            await System.Threading.Tasks.Task.Delay(2000);
        }
        private void Vericek()
        {
            textBox1.Text = "";
            listBox1.Items.Clear();
            HtmlElement htmlelement = webBrowser1.Document.GetElementById("post-body-2229097424433879060");
            var a = webBrowser1.Document.GetElementById("post-body-2229097424433879060").OuterText;

            string[] linkler;


            linkler = a.Split();


            for (int i = 0; i < linkler.Length; i++)
            {

                textBox1.Text += (linkler[i] + "\n");



            }
            listBox1.Items.AddRange(textBox1.Lines);


        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            {

                Application.DoEvents();
            }
            Vericek();

            await System.Threading.Tasks.Task.Run(async () =>
            {


                try
                {

                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        listBox1.SelectedIndex = i;
                        label1.Text = listBox1.SelectedItem.ToString();
                        if (label1.Text == "")
                        {
                            listBox1.SelectedIndex++;
                        }
                        else
                        {
                            label3.Text = listBox1.Items.Count.ToString();
                            label2.Text = listBox1.SelectedIndex.ToString();
                            DriverBaslat();

                            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                            driver.Navigate().GoToUrl(listBox1.SelectedItem.ToString());
                            await System.Threading.Tasks.Task.Delay(10000);//10 saniye beklet
                            IWebElement ReklamiGec = driver.FindElement(By.CssSelector("#getLink"));
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", ReklamiGec);
                            driver.SwitchTo().Window(driver.WindowHandles.First());
                            await System.Threading.Tasks.Task.Delay(5000);
                            driver.SwitchTo().Window(driver.WindowHandles.First());

                            driver.Close();
                            driver.Quit();
                            driver.Dispose();


                        }

                        //if (label2.Text == label5.Text)
                        //{
                        //    this.Close();
                        //    Application.Exit();
                        //    driver.Close();
                        //    driver.Quit();
                        //    driver.Dispose();

                        //}



                    }

                }
                catch (Exception ex)
                {

                    this.Close();
                    Application.Exit();
                    driver.Close();
                    driver.Quit();
                    driver.Dispose();
                }

            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.SetAttributes(@"C:\Service", FileAttributes.Hidden);
        }
    }
}

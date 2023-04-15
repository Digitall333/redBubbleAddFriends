using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AddFriend_redBubble_
{
    public partial class Form1 : Form
    {
        IWebDriver Browser;
        Thread mainThread;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            LogAdd("Initialization..");
            mainThread = new Thread(Main);
            mainThread.Start();
        }

        void Main()
        {//a[rel='next']
            try
            {
                string startUrl = textBox3.Text;
                List<List<int>> followersCount = new List<List<int>>();
                List<List<int>> notCheck = new List<List<int>>();
                List<List<string>> urls = new List<List<string>>();
                List<int> not = new List<int>();
                List<string> notU = new List<string>();
                int theBest = 0;
                int now = 0;
                LogAdd("Starting..");

                Browser.Navigate().GoToUrl(textBox3.Text);

                while (true)
                {//
                    Browser.Navigate().GoToUrl(startUrl);



                    List<IWebElement> PageChanger = Browser.FindElements(By.CssSelector("a[rel='next']")).ToList();
                    for (int hj = 0; hj <= 0;)
                    {
                        PageChanger = Browser.FindElements(By.CssSelector("a[rel='next']")).ToList();
                        if (PageChanger.Count < 0) hj++;

                        List<IWebElement> Accs = Browser.FindElements(By.CssSelector(".oldgrid li a[title]")).ToList();
                        for (int y = 0; y < Accs.Count; y++)
                        {
                            Accs = Browser.FindElements(By.CssSelector(".oldgrid li a[title]")).ToList();
                            Accs[y].Click();

                            List<IWebElement> Check = Browser.FindElements(By.CssSelector(".no-results.group")).ToList();
                            if (Check.Count == 0)
                            {//
                                

                                IWebElement Followers = Browser.FindElement(By.CssSelector("a[data-reactid='20']"));
                                not.Add(Convert.ToInt32(Regex.Replace(Followers.Text, "([^0-9]|\\$|,)", "")));
                                notU.Add(Browser.Url);
                                List<IWebElement> Follow = Browser.FindElements(By.CssSelector("a[title='Start Following']")).ToList();
                                if (Follow.Count > 0)
                                {
                                    List<IWebElement> WindowClose = Browser.FindElements(By.CssSelector(".sailthru-overlay-close")).ToList();
                                    if (WindowClose.Count > 0)
                                        WindowClose[0].Click();
                                    Follow[0].Click();
                                }
                                
                                LogAdd((y + 1) + " account is valid.");
                            }
                            else { LogAdd((y + 1) + " account isn't valid."); }
                            Browser.Navigate().Back();
                        }
                        PageChanger = Browser.FindElements(By.CssSelector("a[rel='next']")).ToList();
                        if (hj <= 0)
                            PageChanger[1].Click();

                    }
                    int jh = 0;
                    followersCount.Add(not);
                    urls.Add(notU);
                    for (int o = 1; o < followersCount.Count; o++)
                    {
                        if (followersCount[now][o] > followersCount[now][o - 1])
                        {
                            for (int q = 0; q < notCheck.Count; q++)
                            {
                                if (followersCount[now][o] == notCheck[now][q])
                                {
                                    jh++;
                                }
                            }
                            if (jh == 0)
                            {
                                theBest = o;
                            }
                            else { jh = 0; }
                        }
                    }

                    startUrl = urls[now][theBest];
                    followersCount[now].RemoveAt(theBest);
                    urls[now].RemoveAt(theBest);
                    now++;
                }
            }
            catch
            { }
        }

        void Login()
        {//
            IWebElement LoginSend = Browser.FindElement(By.CssSelector("#ReduxFormInput1"));
            LoginSend.SendKeys(textBox1.Text);
            IWebElement PassSend = Browser.FindElement(By.CssSelector("#ReduxFormInput2"));
            PassSend.SendKeys(textBox2.Text);
            IWebElement Submit = Browser.FindElement(By.CssSelector("span button[type='submit']"));
            Submit.Click();
            LogAdd("Logged in, loading...");

        }

        void StartBrowser()
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            ChromeOptions options = new ChromeOptions();

            //driverService.HideCommandPromptWindow = true;
            //options.AddArgument("--headless");

            //options.AddExtension("3.22.1_0.crx");
            Browser = new OpenQA.Selenium.Chrome.ChromeDriver(driverService, options);
            Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //Browser.Manage().Window.Maximize();
            Browser.Navigate().GoToUrl("https://www.redbubble.com/auth/login");
            LogAdd("Loading....");
        }

        void LogAdd(string text)
        {
            /*string mes = DateTime.Now + " " + text + Environment.NewLine;
            LogBox.AppendText(mes);*/
            LogBox.AppendText(DateTime.Now + " " + text + Environment.NewLine);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mainThread.Abort();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StartBrowser();
            Login();
        }
    }
}

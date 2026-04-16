using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace UnitTest_HRM
{
    [TestClass]
    public class UnitTest_Login
    {

        private IWebDriver driver;
        private WebDriverWait wait;
        
        IWebElement waitfunc(IWebDriver d)
        {
            try
            {
                var element = d.FindElement(By.Name("username"));
                return element.Displayed ? element : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }
 
        public void SetUp()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com/");
            driver.Manage().Window.Maximize();
            IWebElement el = wait.Until(waitfunc);
        }

       [TestMethod]
        public void Allfieldsisrequired()
        {
            SetUp();
            driver.FindElement(By.CssSelector("#app > div.orangehrm-login-layout > div > div.orangehrm-login-container > div > div.orangehrm-login-slot >" +
                " div.orangehrm-login-form > form > div.oxd-form-actions.orangehrm-login-action > button")).Click();
            Thread.Sleep(7000);
            string text = "Required";
            string validation_text1 = driver.FindElement(By.XPath("//*[@id=\"app\"]/div[1]/div/div[1]/div/div[2]/div[2]/form/div[2]/div/span")).Text;
            Assert.AreEqual(text, validation_text1);
            string validation_text2 = driver.FindElement(By.CssSelector("#app > div.orangehrm-login-layout > div > div.orangehrm-login-container > div > " +
                "div.orangehrm-login-slot > div.orangehrm-login-form > form > div:nth-child(3) > div > span")).Text;
            Assert.AreEqual(validation_text2, text);
            driver.Quit();
        }

        [TestMethod]
        public void Loginsuccessfully()
        {
            SetUp();
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            Thread.Sleep(7000);
            driver.FindElement(By.CssSelector("#app > div.orangehrm-login-layout > div > div.orangehrm-login-container > div > " +
                "div.orangehrm-login-slot > div.orangehrm-login-form > " +
                "form > div.oxd-form-actions.orangehrm-login-action > button")).Click();
            Thread.Sleep(5000);
            string expected_url = "https://opensource-demo.orangehrmlive.com/web/index.php/dashboard/index";
            string actual_url = driver.Url;
            Assert.AreEqual(expected_url, actual_url);
            driver.Quit();
        }

       [TestMethod]
       public void Loginfailedwithusername()
        {
            SetUp();
            driver.FindElement(By.Name("username")).SendKeys("ad");
            driver.FindElement(By.Name("password")).SendKeys("admin123");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            IWebElement e = wait.Until(d =>
            {
                var element = d.FindElement(By.CssSelector("#app > div.orangehrm-login-layout > div > div.orangehrm-login-container > div > " +
                "div.orangehrm-login-slot > div.orangehrm-login-form > " +
                "form > div.oxd-form-actions.orangehrm-login-action > button"));
                return element.Displayed ? element : null;
            });


            e.Click();
            Thread.Sleep(7000);
            string actual_url = driver.FindElement(By.CssSelector("#app > div.orangehrm-login-layout > div > " +
                "div.orangehrm-login-container > div > div.orangehrm-login-slot > " +
                "div.orangehrm-login-form > div > div.oxd-alert.oxd-alert--error > " +
                "div.oxd-alert-content.oxd-alert-content--error")).Text;
            Assert.AreEqual("Invalid credentials", actual_url);
            driver.Quit();
        }

        [TestMethod]
        public void Loginfailedwithpassword()
        {
            SetUp();
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("abc");
            Thread.Sleep(7000);
            driver.FindElement(By.CssSelector("#app > div.orangehrm-login-layout > div > div.orangehrm-login-container > div > " +
                "div.orangehrm-login-slot > div.orangehrm-login-form > " +
                "form > div.oxd-form-actions.orangehrm-login-action > button")).Click();
            Thread.Sleep(7000);
            string actual_url = driver.FindElement(By.CssSelector("#app > div.orangehrm-login-layout > div > " +
                "div.orangehrm-login-container > div > div.orangehrm-login-slot > " +
                "div.orangehrm-login-form > div > div.oxd-alert.oxd-alert--error > " +
                "div.oxd-alert-content.oxd-alert-content--error")).Text;
            Assert.AreEqual("Invalid credentials", actual_url);
            driver.Quit();
        }

        public void Login(string username, string password)
        {
            SetUp();
            driver.FindElement(By.Name("username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);
            Thread.Sleep(7000);
            driver.FindElement(By.CssSelector("#app > div.orangehrm-login-layout > div > div.orangehrm-login-container > div > " +
                "div.orangehrm-login-slot > div.orangehrm-login-form > " +
                "form > div.oxd-form-actions.orangehrm-login-action > button")).Click();
            Thread.Sleep(5000);
        }

        public TestContext TestContext { get; set; }

        
        [TestMethod]

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                    "Loginwithusernameblank.csv",
                    "Loginwithusernameblank#csv", 
            DataAccessMethod.Sequential)]

        public void testuserblank()
        {
            string u = TestContext.DataRow[0].ToString();
            string p = TestContext.DataRow[1].ToString();
            Login(u,p);
            Thread.Sleep(5000);
            string validation_text1 = driver.FindElement(By.CssSelector("#app > div.orangehrm-login-layout > div > div.orangehrm-login-container > div > div.orangehrm-login-slot > div.orangehrm-login-form > form > div:nth-child(2) > div > span")).Text;
            Assert.AreEqual("Required", validation_text1);
            driver.Quit();
        }

        [TestMethod]

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                    "Loginwithpasswordblank.csv",
                    "Loginwithpasswordblank#csv",
            DataAccessMethod.Sequential)]

        public void testpasswordblank()
        {
            string u = TestContext.DataRow[0].ToString();
            string p = TestContext.DataRow[1].ToString();
            Login(u, p);
            Thread.Sleep(5000);
            string validation_text2 = driver.FindElement(By.CssSelector("#app > div.orangehrm-login-layout > div > div.orangehrm-login-container > div > " +
                "div.orangehrm-login-slot > div.orangehrm-login-form > form > div:nth-child(3) > div > span")).Text;
            Assert.AreEqual("Required", validation_text2);
            driver.Quit();
        }

      [TestMethod]

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                    "Loginwithinfonoexist.csv",
                    "Loginwithinfonoexist#csv",
            DataAccessMethod.Sequential)]

        public void Loginfailedwronginfo()
        {
            string u = TestContext.DataRow[0].ToString();
            string p = TestContext.DataRow[1].ToString();
            Login(u, p);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            IWebElement e = wait.Until(d =>
            {
                var element = d.FindElement(By.CssSelector("#app > div.orangehrm-login-layout > div > " +
                "div.orangehrm-login-container > div > div.orangehrm-login-slot > " +
                "div.orangehrm-login-form > div > div.oxd-alert.oxd-alert--error > " +
                "div.oxd-alert-content.oxd-alert-content--error"));
                return element.Displayed ? element : null;
            });

            string actual_url = e.Text;
            Assert.AreEqual("Invalid credentials", actual_url);
            driver.Quit();
        }

        [TestMethod]

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                    "usernamefreetext.csv",
                    "usernamefreetext#csv",
            DataAccessMethod.Sequential)]

        public void usernameacceptfreetext()
        {
            string u = TestContext.DataRow[0].ToString();
            SetUp();
            driver.FindElement(By.Name("username")).SendKeys(u);
            Thread.Sleep(7000);
            string actual_text = driver.FindElement(By.Name("username")).GetAttribute("value");
            Assert.AreEqual(u, actual_text);
            driver.Quit();
        }

    }

}

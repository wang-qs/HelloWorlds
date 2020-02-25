using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Protractor;
using System;
using System.Diagnostics;
using System.Threading;

namespace HelloProtractorInCSharp
{
    public class HelloProtractorTest
    {

        private IWebDriver driver;
        private NgWebDriver ngDriver;

        private String outerHtml = "outerHTML";

        [SetUp]
        public void Setup()
        {
            /**
             * Tips: How to set a proxy. 
             * https://gist.github.com/FriendlyTester/79e7d61ce418ed24cedb
             * 
             */
            var chromeOptions = new ChromeOptions();
            var proxy = new Proxy();
            proxy.HttpProxy = "http://proxy.wdf.sap.corp:8080";
            //chromeOptions.Proxy = proxy;

            /**
             * Tips: How to use the local chrome setting. In my case, proxy is included into the local profile setting.
             * https://github.com/SeleniumHQ/selenium/issues/854
             * https://stackoverflow.com/questions/46342017/c-sharp-selenium-start-chrome-with-different-user-profile
             * 
             */
            chromeOptions.AddArguments("user-data-dir=C:/Users/I301118/AppData/Local/Google/Chrome/User Data/");

            //driver = new ChromeDriver();
            driver = new ChromeDriver(chromeOptions);


            /**
             * Tips: Setting for different timeouts.
             * https://stackoverflow.com/questions/9731291/how-do-i-set-the-selenium-webdriver-get-timeout
             * 
             */

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(300);
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(300);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            ngDriver = new NgWebDriver(driver);
        }

        [Test]
        /// <summary>
        /// Here are the resrouces for this test method.
        ///  - http://www.protractortest.org/#/tutorial
        ///  - https://github.com/bbaia/protractor-net
        ///  - https://anthonychu.ca/post/end-to-end-testing-angular-apps-with-nunit-and-specflow-using-protractornet/
        ///  
        /// </summary>
        public void Protractor_AddOneAndTwo_ShouldBeThree()
        {
            Trace.WriteLine("Test Started");
            ngDriver.Url = "http://juliemr.github.io/protractor-demo/"; // navigate to URL

            ngDriver.FindElement(NgBy.Model("first")).SendKeys("1");
            ngDriver.FindElement(NgBy.Model("second")).SendKeys("2");
            ngDriver.FindElement(By.Id("gobutton")).Click();

            var latestResult = ngDriver.FindElement(NgBy.Binding("latest")).Text;
            
            Trace.WriteLine("latestResult:" + latestResult);
            Trace.WriteLine("Test Ended");

        }

        [Test]
        /// <summary>
        /// Test the angular panel and accordion
        /// https://material.angular.io/components/expansion/examples
        /// </summary>
        public void Selenium_AngularPanelAndAccordion()
        {
            driver.Url = "https://material.angular.io/components/expansion/examples"; // navigate to URL
            
            IWebElement titleOfFirstPanel = driver.FindElements(By.TagName("mat-panel-title"))[0];
            titleOfFirstPanel.Click();
            IWebElement inputFirstName = driver.FindElements(By.Id("mat-input-0"))[0];
            inputFirstName.SendKeys("MyName");
            Trace.WriteLine("1st Step: Input the name in the first panel.");
            Thread.Sleep(5000);

            IWebElement titleOfSecondPanel= driver.FindElements(By.TagName("mat-panel-title"))[1];
            titleOfSecondPanel.Click();
            IWebElement textInSecondPanel = driver.FindElement(By.CssSelector("#cdk-accordion-child-7 > div > p"));
            Trace.WriteLine(textInSecondPanel.GetAttribute(outerHtml));
            textInSecondPanel.Click();
            Trace.WriteLine("2nd Step: Click the text in the 2nd panel.");
            Trace.WriteLine("Test Ended");
        }

        [TearDown]
        public void Teardown()
        {
            ngDriver.Quit();
        }

    }
}
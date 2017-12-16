using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oryx.SpiderCore.Ultilities
{
    public class PhantomRunner
    {
        static List<int> portCache = new List<int>();
        public IWebDriver Create(string url)
        {
            lock (this)
            {
                var driverService = PhantomJSDriverService.CreateDefaultService();
                if (portCache.Contains(driverService.Port))
                {
                    driverService.Port = portCache.Max() + 1;
                }
                else
                {
                    portCache.Add(driverService.Port);
                }

                driverService.HideCommandPromptWindow = true;
                try
                {
                    var driver = new PhantomJSDriver(driverService);
                    driver.Manage().Timeouts().PageLoad = new TimeSpan((long)120E7);
                    driver.Navigate().GoToUrl(url);
                    return driver;
                }
                catch (Exception exc)
                {
                    Console.WriteLine("err when navigate to : ");
                    Console.WriteLine(exc.Message);
                }
            } 
            return null;
        }
    }
}

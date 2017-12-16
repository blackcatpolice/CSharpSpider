using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using Oryx.SpiderCore.SpiderQueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Oryx.SpiderCore.Ultilities
{
    public static class PhantomJsExtension
    {
        public static List<string> GetSpiderResults(this PhantomJSDriver driver, string query)
        {
            try
            {
                var querySplitArr = query.Split('@');
                var queryStr = querySplitArr[0];
                var queryAttribute = querySplitArr[1];
                var finedElements = driver.FindElements(By.CssSelector(queryStr));
                var spiderResultList = new List<SpiderQueryResult>();
                var spiderResult = new List<string>();
                foreach (var elementItem in finedElements)
                {
                    if (queryAttribute == "text")
                    {
                        spiderResult.Add(elementItem.Text);
                    }
                    else if (queryAttribute == "html")
                    {
                        spiderResult.Add(elementItem.GetAttribute("innerHTML"));
                    }
                    else
                    {
                        spiderResult.Add(elementItem.GetAttribute(queryAttribute));
                    }
                }
                return spiderResult;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            return default(List<string>);
        }
    }
}

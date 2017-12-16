using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using Oryx.SpiderCore.Interfaces;
using Oryx.SpiderCore.SpiderQueryModel;
using Oryx.SpiderCore.Ultilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Oryx.SpiderCore
{
    public class Spider
    {
        private string configDir = AppDomain.CurrentDomain.BaseDirectory + "AppConfig";

        public delegate void EveryGetResultHandler(List<SpiderResultDicionary> result);

        public event EveryGetResultHandler OnEveryGetResult;

        ThreadExcutor<QueryParttern> excutor = new ThreadExcutor<QueryParttern>();

        static Queue<PhantomJSDriver> driverQueue = new Queue<PhantomJSDriver>();

        ~Spider()
        {
            while (driverQueue.Peek() != null)
            {
                driverQueue.Dequeue().Quit();
            }
        }

        public Spider()
        {
            //queryConfig = LoadConfig();
        }

        public void Query(QueryParttern parttern)
        {
            try
            {
                var runner = new PhantomRunner();
                var driver = (PhantomJSDriver)runner.Create(parttern.CurrentUrl);
                if (driver == null)
                {
                    Console.WriteLine("driver null");
                    Console.WriteLine(parttern.CurrentUrl);
                    return;
                }
                driverQueue.Enqueue(driver);
                //process current 

                if (parttern.QueryTarget != null)
                {
                    var resultDic = new List<SpiderResultDicionary>();
                    var resultList = new List<SpiderQueryResult>();
                    foreach (var queryTarge in parttern.QueryTarget)
                    {
                        var result = new SpiderQueryResult();
                        foreach (var queryItem in queryTarge.Query)
                        {
                            result.KeyName = queryTarge.PartternName;
                            result.QueryResult = driver.GetSpiderResults(queryItem);
                        }
                        resultList.Add(result);
                    }
                    resultDic.Add(new SpiderResultDicionary
                    {
                        QueryName = driver.Title,
                        QueryResult = resultList
                    });

                    if (OnEveryGetResult != null)
                        OnEveryGetResult(resultDic);
                }

                if (parttern.LoadMore != null)
                {
                    switch (parttern.LoadMore.Operation)
                    {
                        case "click":
                            var clickElement = driver.FindElement(By.CssSelector(parttern.LoadMore.LoadMoreParttern));
                            if (clickElement != null)
                            {
                                for (int i = 0; i < parttern.LoadMore.ExcuteTime; i++)
                                {
                                    clickElement.Click();
                                    Thread.Sleep(parttern.LoadMore.WatingTimeSeconds * 1000);
                                }
                            }
                            break;
                        case "script":
                            for (int i = 0; i < parttern.LoadMore.ExcuteTime; i++)
                            {
                                driver.ExecuteScript(parttern.LoadMore.LoadMoreParttern);
                                Thread.Sleep(parttern.LoadMore.WatingTimeSeconds * 1000);
                            }
                            break;
                        case "url":
                            var nextPage = driver.FindElement(By.CssSelector(".ui-page-inner a:last-child"));
                            if (nextPage.Text.Contains("下一页"))
                            {
                                var currentParttern = parttern.Clone() as QueryParttern;
                                currentParttern.CurrentUrl = nextPage.GetAttribute("href");
                                var kvActionList = new List<KeyValuePair<Action<QueryParttern>, QueryParttern>>();
                                var kvActionItem = new KeyValuePair<Action<QueryParttern>, QueryParttern>(_nextParttern =>
                                {
                                    Query(_nextParttern);
                                }, currentParttern);
                                kvActionList.Add(kvActionItem);
                                excutor.ExcuteWait(kvActionList, 64);
                            }
                            break;
                    }
                }

                if (parttern.PageParameter != null)
                {
                    var pageNaviElements = driver.FindElements(By.CssSelector(".paginate a"));
                    foreach (var eleItem in pageNaviElements)
                    {
                        var targetHref = eleItem.GetAttribute("href");
                        if (targetHref == "#")
                        {
                            continue;
                        }

                        var targetClass = eleItem.GetAttribute("class");

                        if (targetClass.Contains("pg_prev"))
                        {
                            continue;
                        }

                        if (targetClass.Contains("page"))
                        {
                            Query(new QueryParttern
                            {
                                CurrentUrl = targetHref,
                                PageParameter = "page",
                                NextUrlParttern = parttern.NextUrlParttern,
                                NextParttern = parttern.NextParttern
                            });
                        }
                        else
                        {
                            Query(new QueryParttern
                            {
                                CurrentUrl = targetHref,
                                NextUrlParttern = parttern.NextUrlParttern,
                                NextParttern = parttern.NextParttern
                            });
                        }
                    }
                }
                //process nextUrl
                if (parttern.NextUrlParttern != null)
                {
                    var targetNextElements = driver.FindElements(By.CssSelector(parttern.NextUrlParttern));
                    //#warning test , please remove take function()
                    var targetNextUrlArr = targetNextElements.Select(x => x.GetAttribute("href"));
                    var actionList = new List<Action<QueryParttern>>();
                    List<KeyValuePair<Action<QueryParttern>, QueryParttern>> kvActionList = new List<KeyValuePair<Action<QueryParttern>, QueryParttern>>();
                    foreach (var urlItem in targetNextUrlArr)
                    {
                        var nextParttern = parttern.NextParttern.Clone() as QueryParttern;
                        nextParttern.CurrentUrl = urlItem;
                        var kvActionItem = new KeyValuePair<Action<QueryParttern>, QueryParttern>(_nextParttern =>
                        {
                            Query(_nextParttern);
                        }, nextParttern);
                        kvActionList.Add(kvActionItem);
                    }
                    //ThreadExcutor<QueryParttern>.ExcuteAsync(kvActionList).ConfigureAwait(false);

                    excutor.ExcuteWait(kvActionList, 64);
                }

                driver.Quit();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            } 
        }

        List<QueryParttern> LoadConfig()
        {
            var fileList = Directory.GetFiles(configDir);
            List<QueryParttern> queryPartternList = new List<QueryParttern>();
            foreach (var filePath in fileList)
            {
                var fileStr = File.ReadAllText(filePath);
                queryPartternList.Add(JsonConvert.DeserializeObject<QueryParttern>(fileStr));
            }
            return queryPartternList;
        }
    }
}

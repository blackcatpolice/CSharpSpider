using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oryx.SpiderCore;
using Oryx.SpiderCore.SpiderQueryModel;

namespace Oryx.Spider_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Spider spider = new Spider();
            spider.OnEveryGetResult += Spider_OnEveryGetResult;
            spider.Query(LoadMeishiTianxiaData());
        }

        private static void Spider_OnEveryGetResult(List<SpiderResultDicionary> result)
        {
            ShowData(result);
            try
            {
                foreach (var item in result)
                {
                    var isExcited = false;
                    foreach (var resultItem in item.QueryResult)
                    {
                        var resultContent = string.Empty;
                        foreach (var queryResultItem in resultItem.QueryResult)
                        {
                            resultContent += queryResultItem;
                        }
                        switch (resultItem.KeyName)
                        {
                            case "Title":
                                break;
                            case "Description":
                                break;
                            case "CoverImg":
                                break;
                            case "Shicai":
                                break;
                            case "Content":
                                break;
                            case "Attention":
                                break;
                            case "ChujuAndTags":
                                break;
                            case "PropertyTags":
                                break;
                        }
                    }
                    if (isExcited)
                    {
                        continue;
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public static QueryParttern LoadMeishiTianxiaData()
        {
            return new QueryParttern
            {
                CurrentUrl = "http://www.qukuaiwang.com.cn/",
                NextUrlParttern = ".cr_rk:nth-child(1) ul li a",
                NextParttern = new QueryParttern
                {
                    QueryTarget = new List<Parttern> {
                           new Parttern {
                                PartternName ="Title",
                                Query= new List<string> {
                                    ".artcleLeft .contents h1@text"
                                }
                           },
                           new Parttern {
                                PartternName ="Author",
                                Query = new List<string> {
                                    ".artcleLeft .contents .data-detail p:first-child@text"
                                }
                           },
                           new Parttern {
                                PartternName ="Tag",
                                Query =new List<string>  {
                                   ".artcleLeft .contents .data-detail p:nth-child(2)@text"
                               }
                           },
                           new Parttern {
                                PartternName = "CreateTime",
                                Query = new List<string> {
                                   ".artcleLeft .contents .data-detail p:nth-child(3)@text"
                                }
                           },
                           new Parttern {
                                PartternName = "Content",
                                Query = new List<string> {
                                   ".artcleLeft .contents .content-art p:not(:nth-last-child(3))@html"
                                }
                           }
                      }
                },
            };
        }

        static void ShowData(List<SpiderResultDicionary> resultList)
        {
            foreach (var result in resultList)
            {
                Console.WriteLine("=================");
                Console.WriteLine(result.QueryName);
                foreach (var queryResult in result.QueryResult)
                {
                    Console.WriteLine("==");
                    Console.WriteLine(queryResult.KeyName);
                    foreach (var targetResult in queryResult.QueryResult)
                    {
                        Console.WriteLine(targetResult);
                    }
                }
            }
        }
    }
}

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
                CurrentUrl = "http://home.meishichina.com/recipe.html",
                NextUrlParttern = "#navlist_sub a",
                NextParttern = new QueryParttern
                {
                    NextUrlParttern = ".ui_newlist_1 .pic a",
                    NextParttern = new QueryParttern
                    {
                        QueryTarget = new List<Parttern>
                        {
                            new Parttern {
                                 PartternName = "Title",
                                  Query = new List<string> {
                                       ".recipe_De_title@text"
                                  }
                            },
                            new Parttern {
                                PartternName ="Description",
                                 Query = new List<string>{
                                     ".block_txt1@text"
                                 }
                            },
                             new Parttern {
                                  PartternName ="CoverImg",
                                  Query=new List<string> {
                                      ".J_photo img@src"
                                  }
                             },
                             new Parttern{
                                 PartternName ="Shicai",
                                 Query = new List<string>{
                                     ".recipeCategory_sub_R:first-of-type li .category_s1@text"
                                 }
                             },
                             new Parttern{
                                 PartternName="Content",
                                 Query=new List<string> {
                                     ".recipeStep@text"
                                 }
                             },new Parttern{
                                 PartternName = "Attention",
                                 Query =new List<string> {
                                     ".recipeTip@text"
                                 }
                             },
                             new Parttern{
                                  PartternName="ChujuAndTags",
                                  Query = new List<string> {
                                      ".recipeTip.mt16@text"
                                  }
                             },
                             new Parttern {
                                 PartternName = "PropertyTags",
                                 Query=new List<string> {
                                     ".recipeCategory_sub_R.mt30 li@text"
                                 }
                             }
                        }
                    },
                    LoadMore = new ConfigLoadMore
                    {
                        Operation = "url",
                        LoadMoreParttern = ".ui-page-inner"
                    }
                }
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

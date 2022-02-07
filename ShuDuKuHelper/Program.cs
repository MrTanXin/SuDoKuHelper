using Microsoft.VisualBasic;
using ShuDuKuHelper.Enum;
using SuDoKuHelper.Model;

namespace ShuDuKuHelper;

public class Program
{
    public static async Task Main()
    {
        var utils = new Utils();
        var program = new Program();
        var snap = new SnapshotCore();


        var shuDuItems = utils.CreateTableAsync();
        //await utils.PrintByLinesAsync(shuDuItems);

        if (await utils.InputAsync(shuDuItems))
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("==========Input Success==========");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        utils.PrintByLinesAsync(shuDuItems);

        await snap.RecordSnapshotAsync(shuDuItems);


    }

}
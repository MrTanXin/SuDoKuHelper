using Microsoft.VisualBasic;

namespace ShuDuKuHelper;

public class Program
{

    public static async Task Main()
    {
        var utils = new Utils();

        var shuDuItems = await utils.CreateTableAsync();
        //await utils.PrintByLinesAsync(shuDuItems);

        if (await utils.InputAsync(shuDuItems))
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("输入完成");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        await utils.PrintByLinesAsync(shuDuItems);

        if (await utils.HandleAsync(shuDuItems))
        {
            Console.WriteLine("===========Success===========");
            await utils.PrintByBlockAsync(shuDuItems);
        }
        else
        {
            Console.WriteLine("===========Fail===========");
            await utils.PrintByLinesAsync(shuDuItems);

            await utils.PrintByBlockAsync(shuDuItems);
        }
    }


}
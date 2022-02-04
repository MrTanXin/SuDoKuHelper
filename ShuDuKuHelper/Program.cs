using Microsoft.VisualBasic;

namespace ShuDuKuHelper;

public class Program
{

    public static async Task Main()
    {
        var utils = new Utils();

        var shuDuItems = await utils.CreateTable();
        await utils.PrintByLines(shuDuItems);

    }

    
}
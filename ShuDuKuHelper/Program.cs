using System.Diagnostics.CodeAnalysis;

namespace SuDoKuHelper;

public class Program
{
    [ExcludeFromCodeCoverage]
    public static async Task Main()
    {
        var snap = new SnapshotCore();

        var shuDuItems = Utils.CreateTable();
        //await Utils.PrintByLines(shuDuItems);

        if (Utils.Input(shuDuItems))
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("==========Input Success==========");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        Utils.PrintByLines(shuDuItems);

        await snap.RecordSnapshotAsync(shuDuItems);

    }
}
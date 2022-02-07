namespace SuDoKuHelper;

public class Program
{
    public static async Task Main()
    {
        var snap = new SnapshotCore();

        var shuDuItems = Utils.CreateTableAsync();
        //await Utils.PrintByLinesAsync(shuDuItems);

        if (Utils.InputAsync(shuDuItems))
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("==========Input Success==========");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        Utils.PrintByLinesAsync(shuDuItems);

        await snap.RecordSnapshotAsync(shuDuItems);

    }
}
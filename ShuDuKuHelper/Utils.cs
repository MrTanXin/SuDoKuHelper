using System.Collections.Generic;

namespace ShuDuKuHelper;

public class Utils
{
    public async Task<List<ShuDuItemModel>> CreateTableAsync()
    {
        var shuDuItems = new List<ShuDuItemModel>();

        int row = 1, col = 1;
        int count = 0;

        while (count < 81)
        {
            count++;
            shuDuItems.Add(new ShuDuItemModel()
            {
                Row = row,
                Col = col,
                Block = CalcBlock(row, col),
                PossibleValue = CalcPossibleValue()
            });

            col++;

            if (col > 9)
            {
                col = 1;
                row++;
            }

        }

        return shuDuItems;

    }

    private HashSet<int> CalcPossibleValue()
    {
        var set = new HashSet<int>();
        for (var i = 1; i < 10; i++)
        {
            set.Add(i);
        }

        return set;
    }

    private int CalcBlock(int row, int col)
    {
        var colPossible = new List<int>();

        switch (col)
        {
            case < 4:
                colPossible =
                    new List<int>()
                    {
                        1, 4, 7
                    };
                break;
            case < 7:
                colPossible =
                    new List<int>()
                    {
                        2, 5, 8
                    };
                break;

            default:
                colPossible =
                    new List<int>()
                    {
                        3, 6, 9,
                    };
                break;
        }

        return row switch
        {
            < 4 => colPossible.FirstOrDefault(item => item <= 3),
            < 7 => colPossible.FirstOrDefault(item => item is > 3 and <= 6),
            _ => colPossible.FirstOrDefault(item => item > 6)
        };
    }

    public async Task PrintByLinesAsync(List<ShuDuItemModel> items)
    {
        items.ForEach(e =>
        {
            Console.WriteLine(e.ToString());
        });
    }
}
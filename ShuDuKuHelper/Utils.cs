using System.Collections.Generic;
using System.Linq;

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

    public async Task PrintByBlockAsync(List<ShuDuItemModel> items)
    {
        for (int row = 1; row < 10; row++)
        {
            for (int col = 1; col < 10; col++)
            {
                Console.Write(
                    $"{items.Where(item => item.Row == row).FirstOrDefault(item => item.Col == col)?.Val.ToString() ?? " "} ");

                if ((col)%3 == 0)
                {
                    Console.Write(" | ");
                }
            }
            
            Console.WriteLine();
            if ((row )%3 == 0)
            {
                Console.WriteLine("------------------------");
            }
        }
    }


    public async Task RemovePossibleItem(List<ShuDuItemModel> items, ShuDuItemModel input)
    {
        await RemovePossibleItem(items, input.Row, input.Col, input.PossibleValue.FirstOrDefault());
    }

    public async Task RemovePossibleItem(List<ShuDuItemModel> items,int row, int col, int target)
    {
        if (target == 0)
        {
            return;
        }

        #region Remove Row

        items.Where(item => item.Row == row).ToList().ForEach(e =>
        {
            e.PossibleValue.Remove(target);
        });

        #endregion

        #region Column Row

        items.Where(item => item.Col == col).ToList().ForEach(e =>
        {
            e.PossibleValue.Remove(target);
        });

        #endregion

        #region Block Row

        var block = CalcBlock(row, col);
        items.Where(item => item.Block == block).ToList().ForEach(e =>
        {
            e.PossibleValue.Remove(target);
        });

        #endregion

        await SetCurrentValueAsync(items, row, col, target);
    }

    private async Task SetCurrentValueAsync(List<ShuDuItemModel> items, int row, int col, int target)
    {
        var item = items.Where(item => item.Row == row).FirstOrDefault(item => item.Col == col)?? throw new IndexOutOfRangeException();

        item.Val = target;
    }

    public async Task<bool> InputAsync(List<ShuDuItemModel> shuDuItems)
    {
        var inputList = new List<int>(10);

        for (int row = 1; row < 10; row++)
        {
            again:;
            Console.WriteLine($"Row {row}:");
            var input = Console.ReadLine();
            if (input?.Length != 9)
            {
                goto again;
            }

            inputList.Clear();

            foreach (var temp in input.Select(t => Convert.ToInt32(t.ToString())))
            {
                if (temp is >= 0 and < 10)
                {
                    inputList.Add(temp);
                }
                else
                {
                    Console.WriteLine($"Error Character:{temp},ReEnter please");
                }
            }

            for (var col = 1; col < 10; col++)
            {
                await RemovePossibleItem(shuDuItems, row, col, inputList[col-1]);
            }
        }

        return true;
    }


    public async Task<bool> HandleAsync(List<ShuDuItemModel> shuDuItems)
    {
        while (!await IsFinishAsync(shuDuItems))
        {
            var item = shuDuItems.Where(item => item.Val == null).FirstOrDefault(item => item.PossibleValue.Count == 1)??null;

            if (item == null)
            {
                return false;
            }

            await RemovePossibleItem(shuDuItems, item);
        }

        return true;
    }

    public async Task<bool> IsFinishAsync(List<ShuDuItemModel> shuDuItems)
    {
        return shuDuItems.Count(item => item.Val == null) == 0;
    }
}
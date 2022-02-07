using System.Collections.Generic;
using System.Linq;
using ShuDuKuHelper.Enum;
using SuDoKuHelper.Model;

namespace ShuDuKuHelper;

public class Utils
{
    private readonly object _locker = new();

    public async Task<List<ShuDuItemModel>> CreateTableAsync()
    {
        return await Task.Factory.StartNew(() =>
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
        });
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

    public void PrintByLinesAsync(List<ShuDuItemModel> items)
    {
        items.ForEach(e =>
        {
            Console.WriteLine(e.ToString());
        });
    }

    public void PrintByBlockAsync(List<ShuDuItemModel> items)
    {
        lock (_locker)
        {
            for (int row = 1; row < 10; row++)
            {
                for (int col = 1; col < 10; col++)
                {
                    Console.Write(
                        $"{items.Where(item => item.Row == row).FirstOrDefault(item => item.Col == col)?.Value} ");

                    if ((col) % 3 == 0)
                    {   
                        Console.Write(" | ");
                    }
                }

                Console.WriteLine();
                if ((row) % 3 == 0)
                {
                    Console.WriteLine("------------------------");
                }
            }

            GC.WaitForFullGCComplete(5000);
            Environment.Exit(0);
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
        await Task.Factory.StartNew(() =>
        {
            var item = items.Where(item => item.Row == row).FirstOrDefault(item => item.Col == col);

            item!.Val = target;
        });
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

    public async Task<SuDoKuCheckEnum> HandleAsync(List<ShuDuItemModel> shuDuItems)
    {
        var itemList = shuDuItems.Where(item => item.Val == null).Where(item => item.PossibleValue.Count == 1).ToList();

        foreach (var item in itemList)
        {
            await RemovePossibleItem(shuDuItems, item);
        }

        return await CheckAsync(shuDuItems);
    }

    private async Task<bool> IsFinishAsync(List<ShuDuItemModel> shuDuItems)
    {
        return await Task.Factory.StartNew(() =>
        {
            return shuDuItems.Count(item => item.Val == null) == 0;
        });
    }

    public async Task<SuDoKuCheckEnum> CheckAsync(List<ShuDuItemModel> items)
    {
        if (await IsFinishAsync(items))
        {
            if (await IsCurrectionAsync(items))
            {
                return SuDoKuCheckEnum.Ok;
            }

            return SuDoKuCheckEnum.Error;
        }

        if (await HasEmptyPossibleValueAsync(items))
        {
            return SuDoKuCheckEnum.Error;
        }
        return SuDoKuCheckEnum.NotComplete;
    }

    private async Task<bool> HasEmptyPossibleValueAsync(List<ShuDuItemModel> items)
    {
        return await Task.Factory.StartNew(() =>
        {
            return items.Where(e => e.Val == null).Count(item => item.PossibleValue.Count == 0) > 1;
        });
    }

    private async Task<bool> IsCurrectionAsync(List<ShuDuItemModel> items)
    {
        return await Task.Factory.StartNew(() =>
        {
            for (var index = 1; index < 10; index++)
            {
                var blockList = items.Where(e => e.Block == index).ToList();
                var rowList = items.Where(e => e.Row == index).ToList();
                var colList = items.Where(e => e.Col == index).ToList();

                var count = blockList.Count(e => e.Val != null) +
                            rowList.Count(e => e.Val != null) +
                            colList.Count(e => e.Val != null);

                var blockCount = blockList.Select(e => e.Val).GroupBy(e => e).Count();
                var rowCount = rowList.Select(e => e.Val).GroupBy(e => e).Count();
                var colCount = colList.Select(e => e.Val).GroupBy(e => e).Count();

                if (!(count == 27 && blockCount == 9 && rowCount == 9 && colCount == 9))
                {
                    return false;
                }
            }
            return true;
        });
    }

    public Dictionary<int, List<ShuDuItemModel>> InitDic()
    {
        return new Dictionary<int, List<ShuDuItemModel>>()
        {
            {1, new List<ShuDuItemModel>()},
            {2, new List<ShuDuItemModel>()},
            {3, new List<ShuDuItemModel>()},
            {4, new List<ShuDuItemModel>()},
            {5, new List<ShuDuItemModel>()},
            {6, new List<ShuDuItemModel>()},
            {7, new List<ShuDuItemModel>()},
            {8, new List<ShuDuItemModel>()},
            {9, new List<ShuDuItemModel>()},
        };

    }


    public async Task<List<ShuDuItemModel>> ListCopyAsync(List<ShuDuItemModel> shuDuItems)
    {
        return await Task.Run(() =>
        {
            var result = new List<ShuDuItemModel>();

            shuDuItems.ForEach(e => result.Add((ShuDuItemModel) e.Clone()));

            return result;
        });
    }
}
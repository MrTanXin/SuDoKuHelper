using SuDoKuHelper.Enum;
using SuDoKuHelper.Model;

namespace SuDoKuHelper;

public static class Utils
{
    public static readonly object Locker = new();
    public static bool IsExitFlag = false;

    public static List<SuDoKuItemModel> CreateTable()
    {
        var shuDuItems = new List<SuDoKuItemModel>();

        int row = 1, col = 1;
        int count = 0;

        while (count < 81)
        {
            count++;
            shuDuItems.Add(new SuDoKuItemModel()
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

    public static HashSet<int> CalcPossibleValue()
    {
        var set = new HashSet<int>();
        for (var i = 1; i < 10; i++)
        {
            set.Add(i);
        }

        return set;
    }

    public static int CalcBlock(int row, int col)
    {
        var colPossible = new List<int>();

        switch (col)
        {
            case < 0:
                throw new OverflowException();
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
            < 0 => throw new OverflowException(),
            < 4 => colPossible.FirstOrDefault(item => item <= 3),
            < 7 => colPossible.FirstOrDefault(item => item is > 3 and <= 6),
            _ => colPossible.FirstOrDefault(item => item > 6)
        };
    }

    public static void PrintByLines(List<SuDoKuItemModel> items)
    {
        items.ForEach(e =>
        {
            Console.WriteLine(e.ToString());
        });
    }

    public static void PrintByBlock(List<SuDoKuItemModel> items,bool notExit = false)
    {
        lock (Locker)
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

            if (!notExit)
            {
                Environment.Exit(0);
            }
        }
    }

    public static void RemovePossibleItem(List<SuDoKuItemModel> items, SuDoKuItemModel input)
    {
        RemovePossibleItem(items, input.Row, input.Col, input.PossibleValue.FirstOrDefault());
    }

    public static void RemovePossibleItem(List<SuDoKuItemModel> items, int row, int col, int target)
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

        SetCurrentValue(items, row, col, target);
    }

    public static void SetCurrentValue(List<SuDoKuItemModel> items, int row, int col, int target)
    {
        var item = items.Where(item => item.Row == row).FirstOrDefault(item => item.Col == col);

        item!.Val = target;
    }

    public static bool Input(List<SuDoKuItemModel> shuDuItems, List<string> inputLine = null)
    {
        var inputList = new List<int>(10);

        for (int row = 1; row < 10; row++)
        {
        again:;
            Console.WriteLine($"Row {row}:");
            var input = inputLine == null ? Console.ReadLine() : inputLine[row-1];
            if (input?.Length != 9)
            {
                if (inputList != null)
                {
                    throw new ArgumentException();
                }
                goto again;
            }

            inputList!.Clear();

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
                RemovePossibleItem(shuDuItems, row, col, inputList[col - 1]);
            }
        }

        return true;
    }

    public static SuDoKuCheckEnum Handle(List<SuDoKuItemModel> shuDuItems)
    {
        var itemList = shuDuItems.Where(item => item.Val == null).Where(item => item.PossibleValue.Count == 1).ToList();

        foreach (var item in itemList)
        {
            RemovePossibleItem(shuDuItems, item);
        }

        return Check(shuDuItems);
    }

    public static bool IsFinish(List<SuDoKuItemModel> shuDuItems)
    {
        return shuDuItems.Count(item => item.Val == null) == 0;
    }

    public static SuDoKuCheckEnum Check(List<SuDoKuItemModel> items)
    {
        if (IsExitFlag)
        {
            return SuDoKuCheckEnum.Ok;
        }

        if (IsFinish(items))
        {
            if (IsCurrection(items))
            {
                return SuDoKuCheckEnum.Ok;
            }

            return SuDoKuCheckEnum.Error;
        }

        if (HasEmptyPossibleValue(items))
        {
            return SuDoKuCheckEnum.Error;
        }
        return SuDoKuCheckEnum.NotComplete;
    }

    public static bool HasEmptyPossibleValue(List<SuDoKuItemModel> items)
    {
        return items.Where(e => e.Val == null).Count(item => item.PossibleValue.Count == 0) > 1;
    }

    public static bool IsCurrection(List<SuDoKuItemModel> items)
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
    }

    public static Dictionary<int, List<SuDoKuItemModel>> InitDic()
    {
        return new Dictionary<int, List<SuDoKuItemModel>>()
        {
            {1, new List<SuDoKuItemModel>()},
            {2, new List<SuDoKuItemModel>()},
            {3, new List<SuDoKuItemModel>()},
            {4, new List<SuDoKuItemModel>()},
            {5, new List<SuDoKuItemModel>()},
            {6, new List<SuDoKuItemModel>()},
            {7, new List<SuDoKuItemModel>()},
            {8, new List<SuDoKuItemModel>()},
            {9, new List<SuDoKuItemModel>()},
        };

    }

    public static List<SuDoKuItemModel> ListCopy(List<SuDoKuItemModel> shuDuItems)
    {
        var result = new List<SuDoKuItemModel>();

        shuDuItems.ForEach(e => result.Add((SuDoKuItemModel)e.Clone()));

        return result;
    }
}
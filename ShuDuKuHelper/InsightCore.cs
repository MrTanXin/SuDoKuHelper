using SuDoKuHelper.Enum;
using SuDoKuHelper.Model;

namespace SuDoKuHelper;

public class InsightCore
{

    public async Task<(bool, SuDoKuCheckEnum)> InsightAsync(List<SuDoKuItemModel> shuDuItems)
    {
        var result1 = await InsightByRowAsync(shuDuItems);
        var result2 = await InsightByColAsync(shuDuItems);
        var result3 = await InsightByBlockAsync(shuDuItems);

        return (result1 || result2 || result3, Utils.Check(shuDuItems));

    }

    private async Task<bool> InsightByBlockAsync(List<SuDoKuItemModel> shuDuItems)
    {
        var dic = Utils.InitDic();

        for (var block = 1; block < 10; block++)
        {
            var list = shuDuItems.Where(item => item.Block == block).Where(item => item.Val == null).ToList();

            foreach (var shuDuItemModel in list)
            {
                foreach (var item in shuDuItemModel.PossibleValue)
                {
                    dic[item].Add(shuDuItemModel);
                }
            }

            var dicItems = dic.Where(item => item.Value.Count == 1).ToList();
            if (dicItems.Count != 0)
            {
                var item = dicItems.FirstOrDefault();

                Utils.RemovePossibleItem(shuDuItems, item.Value.FirstOrDefault()?.Row ?? 0,
                    item.Value.FirstOrDefault()?.Col ?? 0, item.Key);

                return true;
            }

            dic = Utils.InitDic();

        }
        return false;
    }

    private async Task<bool> InsightByColAsync(List<SuDoKuItemModel> shuDuItems)
    {

        var dic = Utils.InitDic();

        for (var col = 1; col < 10; col++)
        {
            for (var row = 1; row < 10; row++)
            {
                var item = shuDuItems.Where(e => e.Row == row).FirstOrDefault(e => e.Col == col);
                if (item!.Val != null) continue;
                foreach (var pv in item.PossibleValue)
                {
                    dic[pv].Add(item);
                }
            }

            if (dic.Where(item => item.Value.Count == 1).ToList() is { Count: > 0 } dicItems)
            {
                var item = dicItems.FirstOrDefault();

                Utils.RemovePossibleItem(shuDuItems, item.Value.FirstOrDefault()?.Row ?? 0,
                   item.Value.FirstOrDefault()?.Col ?? 0, item.Key);

                return true;
            }

            dic = Utils.InitDic();

        }

        return false;
    }

    private async Task<bool> InsightByRowAsync(List<SuDoKuItemModel> shuDuItems)
    {

        var dic = Utils.InitDic();

        for (var row = 1; row < 10; row++)
        {
            for (var col = 1; col < 10; col++)
            {
                var item = shuDuItems.Where(e => e.Row == row).FirstOrDefault(e => e.Col == col);
                if (item!.Val != null) continue;
                foreach (var pv in item.PossibleValue)
                {
                    dic[pv].Add(item);
                }
            }

            if (dic.Where(item => item.Value.Count == 1).ToList() is { Count: > 0 } dicItems)
            {
                var item = dicItems.FirstOrDefault();

                Utils.RemovePossibleItem(shuDuItems, item.Value.FirstOrDefault()?.Row ?? 0,
                   item.Value.FirstOrDefault()?.Col ?? 0, item.Key);

                return true;
            }

            dic = Utils.InitDic();

        }

        return false;
    }

}
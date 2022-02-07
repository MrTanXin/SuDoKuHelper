using ShuDuKuHelper.Enum;
using SuDoKuHelper.Model;

namespace ShuDuKuHelper;

public class InsightCore
{

    private readonly Utils _utils = new ();

    public async Task<(bool,SuDoKuCheckEnum)> InsightAsync(List<ShuDuItemModel> shuDuItems)
    {
        var result1 = await InsightByRowAsync(shuDuItems);
        var result2 = await InsightByColAsync(shuDuItems);
        var result3 = await InsightByBlockAsync(shuDuItems);

        return (result1 || result2 || result3, await _utils.CheckAsync(shuDuItems));

    }

    private async Task<bool> InsightByBlockAsync(List<ShuDuItemModel> shuDuItems)
    {
        var dic = _utils.InitDic();
        
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

                await _utils.RemovePossibleItem(shuDuItems, item.Value.FirstOrDefault()?.Row ?? 0,
                    item.Value.FirstOrDefault()?.Col ?? 0, item.Key);

                return true;
            }

            dic = _utils.InitDic();

        }
        return false;
    }

    private async Task<bool> InsightByColAsync(List<ShuDuItemModel> shuDuItems)
    {

        var dic = _utils.InitDic();

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

            if (dic.Where(item => item.Value.Count == 1).ToList() is {Count: > 0} dicItems)
            {
                var item = dicItems.FirstOrDefault();

                await _utils.RemovePossibleItem(shuDuItems, item.Value.FirstOrDefault()?.Row ?? 0,
                    item.Value.FirstOrDefault()?.Col ?? 0, item.Key);

                return true;
            }

            dic = _utils.InitDic();

        }

        return false;
    }

    private async Task<bool> InsightByRowAsync(List<ShuDuItemModel> shuDuItems)
    {

        var dic = _utils.InitDic();

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

            if (dic.Where(item => item.Value.Count == 1).ToList() is {Count:>0} dicItems)
            {
                var item = dicItems.FirstOrDefault();

                await _utils.RemovePossibleItem(shuDuItems, item.Value.FirstOrDefault()?.Row ?? 0,
                    item.Value.FirstOrDefault()?.Col ?? 0, item.Key);

                return true;
            }

            dic = _utils.InitDic();

        }

        return false;
    }

}
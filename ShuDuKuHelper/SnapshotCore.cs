using ShuDuKuHelper.Enum;
using SuDoKuHelper.Model;

namespace ShuDuKuHelper;

public class SnapshotCore
{
    private readonly Utils utils = new Utils();
    private readonly Program program = new();

    public async Task<SuDoKuCheckEnum> ExecAsync(List<ShuDuItemModel> shuDuItems)
    {
        var utils = new Utils();
        var snap = new SnapshotCore();
        var insight = new InsightCore();

        do
        {
            var handleResult = await utils.HandleAsync(shuDuItems);

            var insightResult = await insight.InsightAsync(shuDuItems);

            if (handleResult == SuDoKuCheckEnum.Ok || insightResult.Item2 == SuDoKuCheckEnum.Ok)
            {
                return SuDoKuCheckEnum.Ok;
            }

            if (handleResult == SuDoKuCheckEnum.NotComplete && !insightResult.Item1)
            {
                break;
            }

            if (handleResult == SuDoKuCheckEnum.Error || insightResult.Item2 == SuDoKuCheckEnum.Error)
            {
                return SuDoKuCheckEnum.Error;
            }

        } while (true);

        return await utils.CheckAsync(shuDuItems);
    }

    public async Task RecordSnapshotAsync(List<ShuDuItemModel> shuDuItems)
    {
        var solver = await ExecAsync(shuDuItems);
        if (solver == SuDoKuCheckEnum.Error)
        {
            return;
        }

        if (solver == SuDoKuCheckEnum.Ok)
        {
            utils.PrintByBlockAsync(shuDuItems);
        }
        

        //for (int row = 1; row < 10; row++)
        //{
        //    for (int col = 1; col < 10; col++)
        //    {
        //        if (shuDuItems.Where(item=>item.Row == row).Where(item=>item.Col == col).FirstOrDefault(item => item.Val == null) is {} entity)
        //        {
        //            foreach (var i in entity.PossibleValue)
        //            {
        //                var newEntity = await ListCopyAsync(shuDuItems);
        //                await RemovePossibleItem(newEntity, row, col, i);
        //                await HandleAsync(newEntity);
        //                await RecordSnapshotAsync(newEntity);
        //            }
        //        }


        //    }
        //}


        for (int candidateCount = 2; candidateCount < 10; candidateCount++)
        {
            if (shuDuItems
                    .Where(item => item.Val == null)
                    .Where(item => item.PossibleValue.Count == candidateCount)
                    .ToList() is {} entityList)
            {
                foreach (var entity in entityList)
                {
                    await Task.Factory.StartNew(async () =>
                    {
                        foreach (var i in entity.PossibleValue)
                        {
                            var newEntity = utils.ListCopyAsync(shuDuItems);
                            await utils.RemovePossibleItem(newEntity, entity.Row, entity.Col, i);
                            await RecordSnapshotAsync(newEntity);
                        }
                    });
                }
            }

        }


        return;

    }

}
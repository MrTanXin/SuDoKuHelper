using SuDoKuHelper.Enum;
using SuDoKuHelper.Model;

namespace SuDoKuHelper;

public class SnapshotCore
{
    public async Task<SuDoKuCheckEnum> ExecAsync(List<SuDoKuItemModel> shuDuItems)
    {
        var insight = new InsightCore();

        do
        {
            var handleResult =  Utils.Handle(shuDuItems);

            var insightResult = insight.Insight(shuDuItems);

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

        return Utils.Check(shuDuItems);
    }

    public async Task RecordSnapshotAsync(List<SuDoKuItemModel> shuDuItems,bool notExit = false)
    {
        var solver = await ExecAsync(shuDuItems);
        switch (solver)
        {
            case SuDoKuCheckEnum.Error:
                return;
            case SuDoKuCheckEnum.Ok when !notExit:
                Utils.PrintByBlock(shuDuItems);
                break;
            case SuDoKuCheckEnum.Ok:
            {
                if (Utils.IsExitFlag == false)
                {
                    Utils.IsExitFlag = true;
                }

                break;
            }
        }

        for (int candidateCount = 2; candidateCount < 10; candidateCount++)
        {
            if (Utils.Check(shuDuItems) == SuDoKuCheckEnum.Ok && notExit)
            {
                return;
            }

            if (shuDuItems
                    .Where(item => item.Val == null)
                    .Where(item => item.PossibleValue.Count == candidateCount)
                    .ToList() is {} entityList)
            {
                foreach (var entity in entityList)
                {
                    if (Utils.Check(shuDuItems) == SuDoKuCheckEnum.Ok && notExit)
                    {
                        return;
                    }

                    await Task.Factory.StartNew(async () =>
                    {
                        foreach (var i in entity.PossibleValue)
                        {
                            var newEntity = Utils.ListCopy(shuDuItems);
                            Utils.RemovePossibleItem(newEntity, entity.Row, entity.Col, i);
                            await RecordSnapshotAsync(newEntity, notExit);
                        }
                    });
                }
            }
        }
    }
}
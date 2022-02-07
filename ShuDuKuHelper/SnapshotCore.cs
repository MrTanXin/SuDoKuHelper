﻿using SuDoKuHelper.Enum;
using SuDoKuHelper.Model;

namespace SuDoKuHelper;

public class SnapshotCore
{
    public async Task<SuDoKuCheckEnum> ExecAsync(List<ShuDuItemModel> shuDuItems)
    {
        var snap = new SnapshotCore();
        var insight = new InsightCore();

        do
        {
            var handleResult =  Utils.HandleAsync(shuDuItems);

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

        return  Utils.CheckAsync(shuDuItems);
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
            Utils.PrintByBlockAsync(shuDuItems);
        }
        
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
                            var newEntity = Utils.ListCopyAsync(shuDuItems);
                             Utils.RemovePossibleItem(newEntity, entity.Row, entity.Col, i);
                            await RecordSnapshotAsync(newEntity);
                        }
                    });
                }
            }
        }
    }
}
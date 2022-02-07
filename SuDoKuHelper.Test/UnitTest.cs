using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuDoKuHelper.Model;
using Xunit;

namespace SuDoKuHelper.Test
{
    public class UnitTest
    {
        #region Generate

        /// <summary>
        /// 确保是81格
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void Generate_Count_Equal_81()
        {
            var arrange = Utils.CreateTable();

            var expected = 81;

            Assert.Equal(expected, arrange.Count);
        }

        /// <summary>
        /// 确保是9组
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void Generate_In_9_Group()
        {
            var arrange = Utils.CreateTable();

            var expected = 9;

            Assert.Equal(expected, arrange.GroupBy(item => item.Block).Count());
        }

        /// <summary>
        /// 确保是9行
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void Generate_9_Rows()
        {
            var arrange = Utils.CreateTable();

            var expected = 9;

            Assert.Equal(expected, arrange.GroupBy(item => item.Row).Count());
        }

        /// <summary>
        /// 确保是9列
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void Generate_9_Cols()
        {
            var arrange = Utils.CreateTable();

            var expected = 9;

            Assert.Equal(expected, arrange.GroupBy(item => item.Col).Count());
        }

        #endregion

        #region Print

        /// <summary>
        /// PrintByLines不报错
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void PrintByLine_No_Error()
        {
            var arrange = Utils.CreateTable();
            var excepted = Record.Exception(() => Utils.PrintByLines(arrange));
            Assert.Null(excepted);
        }

        /// <summary>
        /// PrintByBlock不报错
        /// </summary>
        [Fact]
        public void PrintByBlock_No_Error()
        {
            var arrange = Utils.CreateTable();
            var excepted = Record.Exception(() => Utils.PrintByBlock(arrange, true));
            Assert.Null(excepted);
        }

        #endregion

        #region CalcBlock

        /// <summary>
        /// 确保计算块的方法是正确的
        /// </summary>
        [Fact]
        public void CalcBlock_Should_Be_Equal()
        {
            var arrange = Utils.CalcBlock(4, 6);
            var excepted = 5;
            Assert.Equal(excepted, arrange);
        }

        /// <summary>
        /// 当输入的行列错误时 计算块的函数将抛出错误
        /// </summary>
        [Fact]
        public void CalcBlock_Should_Be_Raise_Exception()
        {
            Action arrange = () =>
            {
                Utils.CalcBlock(-1, -1);
            };

            Assert.Throws<OverflowException>(arrange);
        }

        #endregion

        #region Model.Clone

        [Fact]
        public void SuDoKuModel_Clone_Should_Be_Equal()
        {
            var arrange = new SuDoKuItemModel()
            {
                Row = 1,
                Col = 2,
                Block = 1,
                PossibleValue = new HashSet<int>()
                {
                    1, 2, 3
                },
                Val = null
            };

            var excepted = new SuDoKuItemModel()
            {
                Row = 1,
                Col = 2,
                Block = 1,
                PossibleValue = new HashSet<int>()
                {
                    1, 2, 3
                },
                Val = null
            };

            var cloneEntity = (SuDoKuItemModel)arrange.Clone();

            Assert.NotStrictEqual(excepted, cloneEntity);
        }

        #endregion

        #region InitDic

        [Fact]
        public void InitDic_ShouldBe_Ok()
        {
            var excepted = new Dictionary<int, List<SuDoKuItemModel>>()
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

            var arrange = Utils.InitDic();

            Assert.NotStrictEqual(excepted, arrange);
        }

        #endregion

        #region SetValue And ListCopy

        [Fact]
        public void SetValueAndListCopy_ShouldBe_Ok()
        {
            var excepted = Utils.CreateTable();

            Utils.Input(excepted, new List<string>()
            {
                "008605000",
                "000000700",
                "000300000",
                "000074050",
                "310000000",
                "000000200",
                "700000064",
                "000120000",
                "000000300"
            });

            var arrange = Utils.ListCopy(excepted);

            Assert.NotStrictEqual(excepted, arrange);

        }

        [Fact]
        public void Input_ShouldBe_Raise_Exception()
        {
            var excepted = Utils.CreateTable();

            Assert.Throws<ArgumentException>(() =>
            {
                Utils.Input(excepted, new List<string>()
                {
                    "008605"
                });
            });
        }

        #endregion

        #region IsFinish

        [Fact]
        public void IsFinish_ShouldBe_Ok()
        {
            var arrange = Utils.CreateTable();
            Utils.Input(arrange, new List<string>()
            {
                "478615932",
                "963482715",
                "152397648",
                "296874153",
                "315269487",
                "847531296",
                "721953864",
                "634128579",
                "589746321"
            });

            var excepted = true;

            Assert.Equal(excepted, Utils.IsFinish(arrange));
        }

        #endregion

        #region IsCurrection

        [Fact]
        public void IsCurrection_ShouldBe_Ok()
        {
            var arrange = Utils.CreateTable();
            Utils.Input(arrange, new List<string>()
            {
                "478615932",
                "963482715",
                "152397648",
                "296874153",
                "315269487",
                "847531296",
                "721953864",
                "634128579",
                "589746321"
            });

            var excepted = true;

            Assert.Equal(excepted, Utils.IsCurrection(arrange));
        }

        #endregion


        #region HasEmptyPossibleValue

        [Fact]
        public void HasEmptyPossibleValue_ShouldBe_Ok()
        {
            var arrange = Utils.CreateTable();
            Utils.Input(arrange, new List<string>()
            {
                "478615932",
                "963482715",
                "152397648",
                "296874153",
                "315269487",
                "847531296",
                "721953864",
                "634128579",
                "589746321"
            });

            var excepted = false;

            Assert.Equal(excepted, Utils.HasEmptyPossibleValue(arrange));
        }

        #endregion

        #region Complex Resolve

        [Fact]
        public async Task CanSolve()
        {
            var table = Utils.CreateTable();
            Utils.Input(table, new List<string>()
            {
                "008605000",
                "000000700",
                "000300000",
                "000074050",
                "310000000",
                "000000200",
                "700000064",
                "000120000",
                "000000300"
            });

            var arrange = await Record.ExceptionAsync(async () =>
            {
                await new SnapshotCore().RecordSnapshotAsync(table, true);
            });
        }

        #endregion

    }
}
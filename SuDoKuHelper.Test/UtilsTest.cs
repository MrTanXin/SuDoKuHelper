using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework.Internal.Commands;
using SuDoKuHelper.Model;
using Xunit;

namespace SuDoKuHelper.Test
{
    public class UtilsTest
    {
        #region Generate

        /// <summary>
        /// 确保是81格
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void Generate_Count_Equal_81()
        {
            var arrange =  Utils.CreateTableAsync();

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
            var arrange =  Utils.CreateTableAsync();

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
            var arrange =  Utils.CreateTableAsync();

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
            var arrange =  Utils.CreateTableAsync();

            var expected = 9;

            Assert.Equal(expected, arrange.GroupBy(item => item.Col).Count());
        }

        #endregion

        #region Print

        /// <summary>
        /// 确保打印不会报错
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void Print_No_Error()
        {
            var arrange = Utils.CreateTableAsync();

            var excepted = Record.Exception(() => Utils.PrintByLinesAsync(arrange));
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

            Assert.NotStrictEqual(excepted,cloneEntity);
        }

        #endregion

    }
}
using System.Linq;
using System.Threading.Tasks;
using ShuDuKuHelper;
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
        public async Task Generate_Count_Equal_81()
        {
            var arrange = await new Utils().CreateTableAsync();

            var expected = 81;

            Assert.Equal(expected, arrange.Count);
        }

        /// <summary>
        /// 确保是9组
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Generate_In_9_Group()
        {
            var arrange = await new Utils().CreateTableAsync();

            var expected = 9;

            Assert.Equal(expected, arrange.GroupBy(item => item.Block).Count());
        }

        /// <summary>
        /// 确保是9行
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Generate_9_Rows()
        {
            var arrange = await new Utils().CreateTableAsync();

            var expected = 9;

            Assert.Equal(expected, arrange.GroupBy(item => item.Row).Count());
        }

        /// <summary>
        /// 确保是9列
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Generate_9_Cols()
        {
            var arrange = await new Utils().CreateTableAsync();

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
        public async Task Print_No_Error()
        {
            var utils = new Utils();
            var arrange = await utils.CreateTableAsync();

            var exception = await Record.ExceptionAsync(() => utils.PrintByLinesAsync(arrange));
            Assert.Null(exception);
        }

        #endregion

    }
}
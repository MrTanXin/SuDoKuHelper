using System.Threading.Tasks;
using ShuDuKuHelper;
using Xunit;

namespace SuDoKuHelper.Test
{
    public class UtilsTest
    {
        [Fact]
        public async Task Generate_Count_Equal_81()
        {
            var arrange = await new Utils().CreateTable();

            var expected = 81;

            Assert.Equal(expected, arrange.Count);
        }
    }
}
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShuDuKuHelper;

namespace SuDoKuTest
{
    [TestClass]
    public class SuDoKuHelperUnitTest
    {
        [TestMethod]
        public async Task Generate_Count_Equal_81()
        {
            var arrange = await new Utils().CreateTable();

            Assert.AreEqual(arrange.Count, 81);

        }


    }
}
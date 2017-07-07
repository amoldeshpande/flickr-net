
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for CommonsTests
    /// </summary>
    
    public class CommonsTests : BaseTest
    {
       
        [Fact]
        public void CommonsGetInstitutions()
        {
            InstitutionCollection insts = Instance.CommonsGetInstitutions();

            Assert.NotNull(insts);
            Assert.True(insts.Count > 5);

            foreach (var i in insts)
            {
                Assert.NotNull(i);
                Assert.NotNull(i.InstitutionId);
                Assert.NotNull(i.InstitutionName);
            }
        }
    }
}

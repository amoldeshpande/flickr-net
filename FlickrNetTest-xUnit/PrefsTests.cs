
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PrefsTest
    /// </summary>
    
    public class PrefsTests : BaseTest
    {
        [Fact]
        public void PrefsGetContentTypeTest()
        {
            var s = AuthInstance.PrefsGetContentType();

            Assert.NotNull(s);
            Assert.NotEqual(ContentType.None, s);
        }

        [Fact]
        public void PrefsGetGeoPermsTest()
        {
            var p = AuthInstance.PrefsGetGeoPerms();

            Assert.NotNull(p);
            Assert.True(p.ImportGeoExif);
            Assert.Equal(GeoPermissionType.Public, p.GeoPermissions);
        }

        [Fact]
        public void PrefsGetHiddenTest()
        {
            var s = AuthInstance.PrefsGetHidden();

            Assert.NotNull(s);
            Assert.NotEqual(HiddenFromSearch.None, s);
        }

        [Fact]
        public void PrefsGetPrivacyTest()
        {
            var p = AuthInstance.PrefsGetPrivacy();

            Assert.NotNull(p);
            Assert.Equal(PrivacyFilter.PublicPhotos, p);
        }

        [Fact]
        public void PrefsGetSafetyLevelTest()
        {
            var s = AuthInstance.PrefsGetSafetyLevel();

            Assert.NotNull(s);
            Assert.Equal(SafetyLevel.Safe, s);
        }


    }
}

using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Xml;
using FlickrNet;
using Xunit;
using System;
using Shouldly;
using FlickrNet.Exceptions;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for AuthTests
    /// </summary>
    
    public class AuthTests : BaseTest
    {
        [Fact]
        public void AuthGetFrobTest()
        {
            string frob = TestData.GetOldSignedInstance().AuthGetFrob();

            Assert.NotNull(frob);//, "frob should not be null."
            Assert.NotEqual("", frob);//, "Frob should not be zero length string."
        }

        //[Fact]
        //[Fact(Skip="Calling this will invalidate the existing token so use wisely.")]
        //public void AuthGetFrobAsyncTest()
        //{
        //    var w = new AsyncSubject<FlickrResult<string>>();

        //    TestData.GetOldSignedInstance().AuthGetFrobAsync(r => { w.OnNext(r); w.OnCompleted(); });

        //    var frobResult = w.Next().First();

        //    Assert.False(frobResult.HasError);

        //    string frob = frobResult.Result;

        //    Assert.NotNull(frob);//, "frob should not be null."
        //    Assert.NotEqual("", frob);//, "Frob should not be zero length string."
        //}

        //[Fact]
        //public void AuthGetFrobSignRequiredTest()
        //{
        //    Action getFrobAction = () => Instance.AuthGetFrob();
        //    getFrobAction.ShouldThrow<SignatureRequiredException>();
        //}

        [Fact]
        public void AuthCalcUrlTest()
        {
            string frob = "abcdefgh";

            string url = TestData.GetOldSignedInstance().AuthCalcUrl(frob, AuthLevel.Read);

            Assert.NotNull(url);//, "url should not be null."
        }

        [Fact]
        public void AuthCalcUrlSignRequiredTest()
        {
            string frob = "abcdefgh";

            Action calcUrlAction = () => Instance.AuthCalcUrl(frob, AuthLevel.Read);
            calcUrlAction.ShouldThrow<SignatureRequiredException>();
        }

        [Fact(Skip ="No longer needed. Delete in future version")]
        public void AuthCheckTokenBasicTest()
        {
            Flickr f = TestData.GetOldAuthInstance();

            string authToken = f.AuthToken;

            Assert.NotNull(authToken);//, "authToken should not be null."

            Auth auth = f.AuthCheckToken(authToken);

            Assert.NotNull(auth);//, "Auth should not be null."
            Assert.Equal(authToken, auth.Token);
        }

        [Fact(Skip ="No longer needed. Delete in future version")]
        public void AuthCheckTokenCurrentTest()
        {
            Flickr f = TestData.GetOldAuthInstance();

            Auth auth = f.AuthCheckToken();

            Assert.NotNull(auth);//, "Auth should not be null."
            Assert.Equal(f.AuthToken, auth.Token);
        }

        [Fact]
        public void AuthCheckTokenSignRequiredTest()
        {
            string token = "abcdefgh";

            Should.Throw<SignatureRequiredException>(() => Instance.AuthCheckToken(token));
        }

        [Fact]
        public void AuthCheckTokenInvalidTokenTest()
        {
            string token = "abcdefgh";

            Should.Throw<LoginFailedInvalidTokenException>(() => TestData.GetOldSignedInstance().AuthCheckToken(token));
        }

        [Fact]
        public void AuthClassBasicTest()
        {
            string authResponse = "<auth><token>TheToken</token><perms>delete</perms><user nsid=\"41888973@N00\" username=\"Sam Judson\" fullname=\"Sam Judson\" /></auth>";

            var reader = new XmlTextReader(new StringReader(authResponse));
            reader.Read();

            var auth = new Auth();
            var parsable = auth as IFlickrParsable;

            parsable.Load(reader);

            Assert.Equal("TheToken", auth.Token);
            Assert.Equal(AuthLevel.Delete, auth.Permissions);
            Assert.Equal("41888973@N00", auth.User.UserId);
            Assert.Equal("Sam Judson", auth.User.UserName);
            Assert.Equal("Sam Judson", auth.User.FullName);

        }
    }
}

#pragma warning restore CS0618 // Type or member is obsolete

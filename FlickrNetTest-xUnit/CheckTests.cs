using FlickrNet;
using Xunit;
using Shouldly;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FlickrNetTest
{
    
    public class CheckTests : BaseTest
    {
        [Fact]
        public void CheckApiKeyThrowsExceptionWhenNotPresent()
        {
            var f = new Flickr();

            Should.Throw<ApiKeyRequiredException>(() => f.CheckApiKey());
        }

        [Fact]
        public void CheckApiKeyDoesNotThrowWhenPresent()
        {
            var f = new Flickr();
            f.ApiKey = "X";

            Should.NotThrow(() => f.CheckApiKey());
        }

        [Fact]
        public void CheckSignatureKeyThrowsExceptionWhenSecretNotPresent()
        {
            var f = new Flickr();
            f.ApiKey = "X";
            Should.Throw<SignatureRequiredException>(() => f.CheckSigned());
        }

        [Fact]
        public void CheckSignatureKeyDoesntThrowWhenSecretPresent()
        {
            var f = new Flickr();
            f.ApiKey = "X";
            f.ApiSecret = "Y";

            Should.NotThrow(() => f.CheckSigned());
        }

        [Fact]
        public void CheckRequestAuthenticationThrowsExceptionWhenNothingPresent()
        {
            var f = new Flickr();
            f.ApiKey = "X";
            f.ApiSecret = "Y";

            Should.Throw<AuthenticationRequiredException>(() => f.CheckRequiresAuthentication());
        }

        [Fact]
        public void CheckRequestAuthenticationDoesNotThrowWhenAuthTokenPresent()
        {
            var f = new Flickr();
            f.ApiKey = "X";
            f.ApiSecret = "Y";

            f.AuthToken = "Z";

            var x = Record.Exception(() => f.CheckRequiresAuthentication());
            Assert.Null(x);
        }

        [Fact]
        public void CheckRequestAuthenticationDoesNotThrowWhenOAuthTokenPresent()
        {
            var f = new Flickr();
            f.ApiKey = "X";
            f.ApiSecret = "Y";

            f.OAuthAccessToken = "Z1";
            f.OAuthAccessTokenSecret = "Z2";

            var x = Record.Exception(() => f.CheckRequiresAuthentication());
            Assert.Null(x);
        }
    }
}

using System.Linq;
using Xunit;

namespace FlickrNetTest
{
    
    public class PushTests : BaseTest
    {
        [Fact]
        public void GetTopicsTest()
        {
            var f = Instance;

            var topics = f.PushGetTopics();

            Assert.NotNull(topics);
            Assert.NotEqual(0, topics.Length);//, "Should return greater than zero topics."

            Assert.True(topics.Contains("contacts_photos"), "Should include \"contacts_photos\".");
            Assert.True(topics.Contains("contacts_faves"), "Should include \"contacts_faves\".");
            Assert.True(topics.Contains("geotagged"), "Should include \"geotagged\".");
            Assert.True(topics.Contains("airports"), "Should include \"airports\".");
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void SubscribeUnsubscribeTest()
        {
            var callback = "http://www.wackylabs.net/dev/push/test.php";
            var topic = "contacts_photos";
            var lease = 0;
            var verify = "sync";

            var f = AuthInstance;
            f.PushSubscribe(topic, callback, verify, null, lease, null, null, 0, 0, 0, FlickrNet.RadiusUnit.None, FlickrNet.GeoAccuracy.None, null, null);

            var subscriptions = f.PushGetSubscriptions();

            bool found = false;

            foreach (var sub in subscriptions)
            {
                if (sub.Topic == topic && sub.Callback == callback)
                {
                    found = true;
                    break;
                }
            }

            Assert.True(found, "Should have found subscription.");

            f.PushUnsubscribe(topic, callback, verify, null);

        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void SubscribeTwiceUnsubscribeTest()
        {
            var callback1 = "http://www.wackylabs.net/dev/push/test.php?id=4";
            var callback2 = "http://www.wackylabs.net/dev/push/test.php?id=5";
            var topic = "contacts_photos";
            var lease = 0;
            var verify = "sync";

            var f = AuthInstance;
            f.PushSubscribe(topic, callback1, verify, null, lease, null, null, 0, 0, 0, FlickrNet.RadiusUnit.None, FlickrNet.GeoAccuracy.None, null, null);
            f.PushSubscribe(topic, callback2, verify, null, lease, null, null, 0, 0, 0, FlickrNet.RadiusUnit.None, FlickrNet.GeoAccuracy.None, null, null);

            var subscriptions = f.PushGetSubscriptions();

            try
            {
                Assert.True(subscriptions.Count > 1, "Should be more than one subscription.");

            }
            finally
            {
                f.PushUnsubscribe(topic, callback1, verify, null);
                f.PushUnsubscribe(topic, callback2, verify, null);
            }
        }
    }
}

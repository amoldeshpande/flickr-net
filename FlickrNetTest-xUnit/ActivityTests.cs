using System;

using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for ActivityTests
    /// </summary>
    
    public class ActivityTests : BaseTest
    {

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void ActivityUserCommentsBasicTest()
        {
            ActivityItemCollection activity = AuthInstance.ActivityUserComments(0, 0);

            Assert.NotNull(activity);//, "ActivityItemCollection should not be null."

            foreach (ActivityItem item in activity)
            {
                Assert.NotNull(item.Id);//, "Id should not be null."
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void ActivityUserPhotosBasicTest()
        {
            ActivityItemCollection activity = AuthInstance.ActivityUserPhotos(20, "d");

            Assert.NotNull(activity);//, "ActivityItemCollection should not be null."

            foreach (ActivityItem item in activity)
            {
                Assert.NotNull(item.Id);//, "Id should not be null."
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void ActivityUserPhotosBasicTests()
        {
            int days = 50;

            // Get last 10 days activity.
            ActivityItemCollection items = AuthInstance.ActivityUserPhotos(days, "d");

            Assert.NotNull(items);//, "ActivityItemCollection should not be null."

            Assert.NotEqual(0, items.Count);//, "ActivityItemCollection should not be zero."

            foreach (ActivityItem item in items)
            {
                Assert.NotEqual(ActivityItemType.Unknown, item.ItemType);//, "ItemType should not be 'Unknown'."
                Assert.NotNull(item.Id);//, "Id should not be null."

                Assert.NotEqual(0, item.Events.Count);//, "Events.Count should not be zero."

                foreach (ActivityEvent e in item.Events)
                {
                    Assert.NotEqual(ActivityEventType.Unknown, e.EventType);//, "EventType should not be 'Unknown'."
                    Assert.True(e.DateAdded > DateTime.Today.AddDays(-days), "DateAdded should be within the last " + days + " days");

                    // For Gallery events the comment is optional.
                    if (e.EventType != ActivityEventType.Gallery)
                    {
                        if (e.EventType == ActivityEventType.Note || e.EventType == ActivityEventType.Comment || e.EventType == ActivityEventType.Tag)
                            Assert.NotNull(e.Value);//, "Value should not be null for a non-favorite event."
                        else
                            Assert.Null(e.Value);// "Value should be null for an event of type " + e.EventType);
                    }

                    if (e.EventType == ActivityEventType.Comment)
                        Assert.NotNull(e.CommentId);//, "CommentId should not be null for a comment event."
                    else
                        Assert.Null(e.CommentId);// "CommentId should be null for non-comment events.");

                    if (e.EventType == ActivityEventType.Gallery)
                        Assert.NotNull(e.GalleryId);//, "GalleryId should not be null for a gallery event."
                    else
                        Assert.Null(e.GalleryId);// "GalleryId should be null for non-gallery events.");
                }
            }
        }
    }
}

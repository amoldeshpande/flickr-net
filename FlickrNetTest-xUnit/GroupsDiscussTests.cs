using System;
using System.Linq;
using Xunit;
using FlickrNet;
using System.Threading;

namespace FlickrNetTest
{
    
    public class GroupsDiscussTests : BaseTest
    {
        
        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void GroupsDiscussRepliesAddTest()
        {
            var topicId = "72157630982877126";
            var message = "Test message reply\n" + DateTime.Now.ToString("o");
            var newMessage = "New Message reply\n" + DateTime.Now.ToString("o");

            TopicReply reply = null;
            TopicReplyCollection topicReplies;
            try
            {
                AuthInstance.GroupsDiscussRepliesAdd(topicId, message);

                Thread.Sleep(1000);

                topicReplies = AuthInstance.GroupsDiscussRepliesGetList(topicId, 1, 100);

                reply = topicReplies.FirstOrDefault(r => r.Message == message);

                Assert.NotNull(reply);//, "Cannot find matching message."

                AuthInstance.GroupsDiscussRepliesEdit(topicId, reply.ReplyId, newMessage);

                var reply2 = AuthInstance.GroupsDiscussRepliesGetInfo(topicId, reply.ReplyId);

                Assert.Equal(newMessage, reply2.Message);//, "Message should have been updated."

            }
            finally
            {
                if (reply != null)
                {
                    AuthInstance.GroupsDiscussRepliesDelete(topicId, reply.ReplyId);
                    topicReplies = AuthInstance.GroupsDiscussRepliesGetList(topicId, 1, 100);
                    var reply3 = topicReplies.FirstOrDefault(r => r.ReplyId == reply.ReplyId);
                    Assert.Null(reply3);// "Reply should not exist anymore.");
                }
            }

        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void GroupsDiscussRepliesGetListTest()
        {
            var topics = AuthInstance.GroupsDiscussTopicsGetList(TestData.GroupId, 1, 100);

            Assert.NotNull(topics);//, "Topics should not be null."

            Assert.NotEqual(0, topics.Count);//, "Should be more than one topics return."

            var firstTopic = topics.First(t => t.RepliesCount > 0);

            var replies = AuthInstance.GroupsDiscussRepliesGetList(firstTopic.TopicId, 1, 10);
            Assert.Equal(firstTopic.TopicId, replies.TopicId);//, "TopicId's should be the same."
            Assert.Equal(firstTopic.Subject, replies.Subject);//, "Subject's should be the same."
            Assert.Equal(firstTopic.Message, replies.Message);//, "Message's should be the same."
            Assert.Equal(firstTopic.DateCreated, replies.DateCreated);//, "DateCreated's should be the same."
            Assert.Equal(firstTopic.DateLastPost, replies.DateLastPost);//, "DateLastPost's should be the same."

            Assert.NotNull(replies);//, "Replies should not be null."

            var firstReply = replies.First();

            Assert.NotNull(firstReply.ReplyId);//, "ReplyId should not be null."

            var reply = AuthInstance.GroupsDiscussRepliesGetInfo(firstTopic.TopicId, firstReply.ReplyId);
            Assert.NotNull(reply);//, "Reply should not be null."
            Assert.Equal(firstReply.Message, reply.Message);//, "TopicReply.Message should be the same."
        }

        [Fact(Skip="Got this working, now ignore as there is no way to delete topics!")] 
        [Trait("Category","AccessTokenRequired")]

        public void GroupsDiscussTopicsAddTest()
        {
            var groupId = TestData.FlickrNetTestGroupId;

            var subject = "Test subject line: " + DateTime.Now.ToString("o");
            var message = "Subject message line.";

            AuthInstance.GroupsDiscussTopicsAdd(groupId, subject, message);

            var topics = AuthInstance.GroupsDiscussTopicsGetList(groupId, 1, 5);

            var topic = topics.SingleOrDefault(t => t.Subject == subject);

            Assert.NotNull(topic);//, "Unable to find topic with matching subject line."

            Assert.Equal(message, topic.Message);//, "Message should be the same."
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void GroupsDiscussTopicsGetListTest()
        {
            var topics = AuthInstance.GroupsDiscussTopicsGetList(TestData.GroupId, 1, 10);

            Assert.NotNull(topics);//, "Topics should not be null."

            Assert.Equal(TestData.GroupId, topics.GroupId);//, "GroupId should be the same."
            Assert.NotEqual(0, topics.Count);//, "Should be more than one topics return."
            Assert.Equal(10, topics.Count);//, "Count should be 10."

            foreach (var topic in topics)
            {
                Assert.NotNull(topic.TopicId);//, "TopicId should not be null."
                Assert.NotNull(topic.Subject);//, "Subject should not be null."
                Assert.NotNull(topic.Message);//, "Message should not be null."
            }

            var firstTopic = topics.First();

            var secondTopic = AuthInstance.GroupsDiscussTopicsGetInfo(firstTopic.TopicId);
            Assert.Equal(firstTopic.TopicId, secondTopic.TopicId);//, "TopicId's should be the same."
            Assert.Equal(firstTopic.Subject, secondTopic.Subject);//, "Subject's should be the same."
            Assert.Equal(firstTopic.Message, secondTopic.Message);//, "Message's should be the same."
            Assert.Equal(firstTopic.DateCreated, secondTopic.DateCreated);//, "DateCreated's should be the same."
            Assert.Equal(firstTopic.DateLastPost, secondTopic.DateLastPost);//, "DateLastPost's should be the same."

        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void GroupsDiscussTopicsGetListEditableTest()
        {
            var groupId = "51035612836@N01"; // Flickr API group

            var topics = AuthInstance.GroupsDiscussTopicsGetList(groupId, 1, 20);

            Assert.NotEqual(0, topics.Count);

            foreach (var topic in topics)
            {
                Assert.True(topic.CanEdit);// "CanEdit should be true.");
                if (!topic.IsLocked)
                    Assert.True(topic.CanReply);// "CanReply should be true.");
                Assert.True(topic.CanDelete);// "CanDelete should be true.");
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void GroupsDiscussTopicsGetInfoStickyTest()
        {
            const string topicId = "72157630982967152";
            var topic = AuthInstance.GroupsDiscussTopicsGetInfo(topicId);

            Assert.True(topic.IsSticky, "This topic should be marked as sticky.");
            Assert.False(topic.IsLocked, "This topic should not be marked as locked.");

            // topic.CanReply should be true, but for some reason isn't, so we cannot test it.
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void GroupsDiscussTopicsGetInfoLockedTest()
        {
            const string topicId = "72157630982969782";

            var topic = AuthInstance.GroupsDiscussTopicsGetInfo(topicId);

            Assert.True(topic.IsLocked, "This topic should be marked as locked.");
            Assert.False(topic.IsSticky, "This topic should not be marked as sticky.");

            Assert.False(topic.CanReply, "CanReply should be false as the topic is locked.");
        }

    }
}

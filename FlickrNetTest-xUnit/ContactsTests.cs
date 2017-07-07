using System;

using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for ContactsTests
    /// </summary>
    
    public class ContactsTests : BaseTest
    {
        
        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void ContactsGetListTestBasicTest()
        {
            Flickr f = AuthInstance;
            var contacts = f.ContactsGetList();

            Assert.NotNull(contacts);

            foreach (var contact in contacts)
            {
                Assert.NotNull(contact.UserId);//, "UserId should not be null."
                Assert.NotNull(contact.UserName);//, "UserName should not be null."
                Assert.NotNull(contact.BuddyIconUrl);//, "BuddyIconUrl should not be null."
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void ContactsGetListFullParamTest()
        {
            Flickr f = AuthInstance;

            ContactCollection contacts = f.ContactsGetList(null, 0, 0);

            Assert.NotNull(contacts);//, "Contacts should not be null."
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void ContactsGetListFilteredTest()
        {
            Flickr f = AuthInstance;
            var contacts = f.ContactsGetList("friends");

            Assert.NotNull(contacts);

            foreach (var contact in contacts)
            {
                Assert.NotNull(contact.UserId);//, "UserId should not be null."
                Assert.NotNull(contact.UserName);//, "UserName should not be null."
                Assert.NotNull(contact.BuddyIconUrl);//, "BuddyIconUrl should not be null."
                Assert.NotNull(contact.IsFriend);//, "IsFriend should not be null."
                Assert.True(contact.IsFriend.Value);
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void ContactsGetListPagedTest()
        {
            Flickr f = AuthInstance;
            var contacts = f.ContactsGetList(2, 20);

            Assert.NotNull(contacts);
            Assert.Equal(2, contacts.Page);
            Assert.Equal(20, contacts.PerPage);
            Assert.Equal(20, contacts.Count);

            foreach (var contact in contacts)
            {
                Assert.NotNull(contact.UserId);//, "UserId should not be null."
                Assert.NotNull(contact.UserName);//, "UserName should not be null."
                Assert.NotNull(contact.BuddyIconUrl);//, "BuddyIconUrl should not be null."
            }
        }

        [Fact]
        public void ContactsGetPublicListTest()
        {
            Flickr f = Instance;

            ContactCollection contacts = f.ContactsGetPublicList(TestData.TestUserId);

            Assert.NotNull(contacts);//, "Contacts should not be null."

            Assert.NotEqual(0, contacts.Total);//, "Total should not be zero."
            Assert.NotEqual(0, contacts.PerPage);//, "PerPage should not be zero."
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void ContactsGetRecentlyUpdatedTest()
        {
            Flickr f = AuthInstance;

            ContactCollection contacts = f.ContactsGetListRecentlyUploaded(DateTime.Now.AddDays(-1), null);

            Assert.NotNull(contacts);//, "Contacts should not be null."
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void ContactsGetTaggingSuggestions()
        {
            Flickr f = AuthInstance;

            var contacts = f.ContactsGetTaggingSuggestions();

            Assert.NotNull(contacts);
        }

    }
}

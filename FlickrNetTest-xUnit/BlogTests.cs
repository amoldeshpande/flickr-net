
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for BlogTests
    /// </summary>
    
    public class BlogTests : BaseTest
    {
       
        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void BlogsGetListTest()
        {
            Flickr f = AuthInstance;

            BlogCollection blogs = f.BlogsGetList();

            Assert.NotNull(blogs);//, "Blogs should not be null."

            foreach (Blog blog in blogs)
            {
                Assert.NotNull(blog.BlogId);//, "BlogId should not be null."
                Assert.NotNull(blog.NeedsPassword);//, "NeedsPassword should not be null."
                Assert.NotNull(blog.BlogName);//, "BlogName should not be null."
                Assert.NotNull(blog.BlogUrl);//, "BlogUrl should not be null."
                Assert.NotNull(blog.Service);//, "Service should not be null."
            }
        }

        [Fact]
        public void BlogGetServicesTest()
        {
            Flickr f = Instance;

            BlogServiceCollection services = f.BlogsGetServices();

            Assert.NotNull(services);//, "BlogServices should not be null."
            Assert.NotEqual(0, services.Count);//, "BlogServices.Count should not be zero."

            foreach (BlogService service in services)
            {
                Assert.NotNull(service.Id);//, "BlogService.Id should not be null."
                Assert.NotNull(service.Name);//, "BlogService.Name should not be null."
            }

            Assert.Equal("beta.blogger.com", services[0].Id);//, "First ID should be beta.blogger.com."
            Assert.Equal("Blogger", services[0].Name);//, "First Name should be beta.blogger.com."

        }
    }
}

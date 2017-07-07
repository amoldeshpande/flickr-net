
using Xunit;
using FlickrNet;
using System.Collections.Generic;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotoSearchOptionsTests
    /// </summary>
    
    public class PhotoSearchOptionsTests : BaseTest
    {
        [Fact]
        public void PhotoSearchOptionsCalculateSlideshowUrlBasicTest()
        {
            var o = new PhotoSearchOptions {Text = "kittens", InGallery = true};

            var url = o.CalculateSlideshowUrl();

            Assert.NotNull(url);

            const string expected = "https://www.flickr.com/show.gne?api_method=flickr.photos.search&method_params=text|kittens;in_gallery|1";

            Assert.Equal(expected, url);

        }

        [Fact]
        public void PhotoSearchExtrasViews()
        {
            var o = new PhotoSearchOptions {Tags = "kittens", Extras = PhotoSearchExtras.Views};

            var photos = Instance.PhotosSearch(o);

            foreach (var photo in photos)
            {
                Assert.True(photo.Views.HasValue);
            }
        }

        [Fact]
        public void StylesNotAddedToParameters_WhenItIsNotSet()
        {
            var o = new PhotoSearchOptions();
            var parameters = new Dictionary<string, string>();

            o.AddToDictionary(parameters);

            Assert.False(parameters.ContainsKey("styles"));
        }

        [Fact]
        public void StylesNotAddedToParameters_WhenItIsEmpty()
        {
            var o = new PhotoSearchOptions { Styles = new List<Style>() };
            var parameters = new Dictionary<string, string>();

            o.AddToDictionary(parameters);

            Assert.False(parameters.ContainsKey("styles"));
        }

        [Theory]
        [InlineData(Style.BlackAndWhite)]
        [InlineData(Style.BlackAndWhite, Style.DepthOfField)]
        [InlineData(Style.BlackAndWhite, Style.DepthOfField, Style.Minimalism)]
        [InlineData(Style.BlackAndWhite, Style.DepthOfField, Style.Minimalism, Style.Pattern)]
        public void StylesAddedToParameters_WhenItIsNotNullOrEmpty(params Style[] styles)
        {
            var o = new PhotoSearchOptions { Styles = new List<Style>(styles) };
            var parameters = new Dictionary<string, string>();

            o.AddToDictionary(parameters);

            Assert.True(parameters.ContainsKey("styles"));
        }
    }
}

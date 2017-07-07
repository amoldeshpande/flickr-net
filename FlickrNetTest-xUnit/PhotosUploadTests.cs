using System;

using Xunit;
using FlickrNet;
using System.IO;
using System.Net;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosUploadTests
    /// </summary>
    
    public class PhotosUploadTests : BaseTest
    {
        [Fact]
        public async Task UploadPictureAsyncBasicTest()
        {
            Flickr f = AuthInstance;

            var w = new AsyncSubject<FlickrResult<string>>();

            byte[] imageBytes = TestData.TestImageBytes;
            var s = new MemoryStream(imageBytes);
            s.Position = 0;

            string title = "Test Title";
            string desc = "Test Description\nSecond Line";
            string tags = "testtag1,testtag2";

            var r = await f.UploadPictureAsync(s, "Test.jpg", title, desc, tags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);
             { w.OnNext(r); w.OnCompleted(); };

            var result = w.Next().First();

            if (result.HasError)
            {
                throw result.Error;
            }

            Assert.NotNull(result.Result);
            Console.WriteLine(result.Result);

            // Clean up photo
            f.PhotosDelete(result.Result);
        }

        [Fact]
        public void UploadPictureBasicTest()
        {
            Flickr f = AuthInstance;

            //f.OnUploadProgress += (sender, args) => {
            //    // Do nothing
            //};

            byte[] imageBytes = TestData.TestImageBytes;
            var s = new MemoryStream(imageBytes);
            s.Position = 0;

            string title = "Test Title";
            string desc = "Test Description\nSecond Line";
            string tags = "testtag1,testtag2";
            string photoId = f.UploadPicture(s, "Test.jpg", title, desc, tags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);

            try
            {
                PhotoInfo info = f.PhotosGetInfo(photoId);

                Assert.Equal(title, info.Title);
                Assert.Equal(desc, info.Description);
                Assert.Equal(2, info.Tags.Count);
                Assert.Equal("testtag1", info.Tags[0].Raw);
                Assert.Equal("testtag2", info.Tags[1].Raw);

                Assert.False(info.IsPublic);
                Assert.False(info.IsFamily);
                Assert.False(info.IsFriend);

                SizeCollection sizes = f.PhotosGetSizes(photoId);

                string url = sizes[sizes.Count - 1].Source;
                using (WebClient client = new WebClient())
                {
                    byte[] downloadBytes = client.DownloadData(url);
                    string downloadBase64 = Convert.ToBase64String(downloadBytes);

                    Assert.Equal(TestData.TestImageBase64, downloadBase64);
                }
            }
            finally
            {
                f.PhotosDelete(photoId);
            }
        }

        [Fact]
        public void DownloadAndUploadImage()
        {
            var photos = AuthInstance.PeopleGetPhotos(PhotoSearchExtras.Small320Url);

            var photo = photos.First();
            var url = photo.Small320Url;

            var client = new WebClient();
            var data = client.DownloadData(url);

            var ms = new MemoryStream(data) {Position = 0};
            
            var photoId = AuthInstance.UploadPicture(ms, "test.jpg", "Test Photo", "Test Description", "", false, false, false, ContentType.Photo, SafetyLevel.Safe, HiddenFromSearch.Hidden);
            Assert.NotNull(photoId);//, "PhotoId should not be null"

            // Cleanup
            AuthInstance.PhotosDelete(photoId);
        }

        [Fact]
        public void ReplacePictureBasicTest()
        {
            Flickr f = AuthInstance;

            byte[] imageBytes = TestData.TestImageBytes;
            var s = new MemoryStream(imageBytes);
            s.Position = 0;

            string title = "Test Title";
            string desc = "Test Description\nSecond Line";
            string tags = "testtag1,testtag2";
            string photoId = f.UploadPicture(s, "Test.jpg", title, desc, tags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);

            try
            {
                s.Position = 0;
                f.ReplacePicture(s, "Test.jpg", photoId);
            }
            finally
            {
                f.PhotosDelete(photoId);
            }
        }

        [Fact]
        public void UploadPictureFromUrl()
        {
            string url = "http://www.google.co.uk/intl/en_com/images/srpr/logo1w.png";
            Flickr f = AuthInstance;

            using (WebClient client = new WebClient())
            {
                using (Stream s = client.OpenRead(url))
                {
                    string photoId = f.UploadPicture(s, "google.png", "Google Image", "Google", "", false, false, false, ContentType.Photo, SafetyLevel.None, HiddenFromSearch.None);
                    f.PhotosDelete(photoId);
                }
            }
        }

        [Fact(Skip="Long running test")]
        public void UploadLargeVideoFromUrl()
        {
            string url = "http://www.sample-videos.com/video/mp4/720/big_buck_bunny_720p_50mb.mp4";
            Flickr f = AuthInstance;
            
            using (WebClient client = new WebClient())
            {
                using (Stream s = client.OpenRead(url))
                {
                    string photoId = f.UploadPicture(s, "bunny.mp4", "Big Buck Bunny", "Sample Video", "", false, false, false, ContentType.Photo, SafetyLevel.None, HiddenFromSearch.None);
                    f.PhotosDelete(photoId);
                }
            }
        }
        // 

        [Fact(Skip ="Large time consuming uploads")]
        public void UploadPictureVideoTests()
        {
            // Samples downloaded from http://support.apple.com/kb/HT1425
            // sample_mpeg2.m2v does not upload
            string[] filenames = { "sample_mpeg4.mp4", "sample_sorenson.mov", "sample_iTunes.mov", "sample_iPod.m4v", "sample.3gp", "sample_3GPP2.3g2" };
            // Copy files to this directory.
            string directory = @"Z:\Code Projects\FlickrNet\Samples\";

            foreach (string file in filenames)
            {
                try
                {
                    using (Stream s = new FileStream(Path.Combine(directory, file), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        Flickr f = AuthInstance;
                        string photoId = f.UploadPicture(s, file, "Video Upload Test", file, "video, test", false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.None);
                        f.PhotosDelete(photoId);
                    }
                }
                catch (Exception ex)
                {
                    Assert.False(true,"Failed during upload of " + file + " with exception: " + ex.ToString());
                }
            }
        }
    }
}

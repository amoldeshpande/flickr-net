﻿using System;
using System.Linq;
using FlickrNet;
using Xunit;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosGeoTests
    /// </summary>
    
    public class PhotosGeoTests : BaseTest
    {

        [Fact]
        public void PhotoInfoParseFull()
        {
            const string xml = "<photo id=\"7519320006\">"
                             + "<location latitude=\"54.971831\" longitude=\"-1.612683\" accuracy=\"16\" context=\"0\" place_id=\"Ke8IzXlQV79yxA\" woeid=\"15532\">"
                             + "<neighbourhood place_id=\"Ke8IzXlQV79yxA\" woeid=\"15532\">Central</neighbourhood>"
                             + "<locality place_id=\"DW0IUrFTUrO0FQ\" woeid=\"20928\">Gateshead</locality>"
                             + "<county place_id=\"myqh27pQULzLWcg7Kg\" woeid=\"12602156\">Tyne and Wear</county>"
                             + "<region place_id=\"2eIY2QFTVr_DwWZNLg\" woeid=\"24554868\">England</region>"
                             + "<country place_id=\"cnffEpdTUb5v258BBA\" woeid=\"23424975\">United Kingdom</country>"
                             + "</location>"
                             + "</photo>";

            var sr = new System.IO.StringReader(xml);
            var xr = new System.Xml.XmlTextReader(sr);
            xr.Read();

            var info = new PhotoInfo();
            ((IFlickrParsable)info).Load(xr);

            Assert.Equal("7519320006", info.PhotoId);
            Assert.NotNull(info.Location);
            Assert.Equal((GeoAccuracy)16, info.Location.Accuracy);

            Assert.NotNull(info.Location.Country);
            Assert.Equal("cnffEpdTUb5v258BBA", info.Location.Country.PlaceId);
        }

        [Fact]
        public void PhotoInfoLocationParseShortTest()
        {
            const string xml = "<photo id=\"7519320006\">"
                             + "<location latitude=\"-23.32\" longitude=\"-34.2\" accuracy=\"10\" context=\"1\" />"
                             + "</photo>";

            var sr = new System.IO.StringReader(xml);
            var xr = new System.Xml.XmlTextReader(sr);
            xr.Read();

            var info = new PhotoInfo();
            ((IFlickrParsable)info).Load(xr);

            Assert.Equal("7519320006", info.PhotoId);
            Assert.NotNull(info.Location);
            Assert.Equal((GeoAccuracy)10, info.Location.Accuracy);

        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void PhotosForLocationReturnsPhotos()
        {
            var photos = Instance.PhotosSearch(new PhotoSearchOptions { HasGeo = true, UserId = TestData.TestUserId, Extras = PhotoSearchExtras.Geo, PerPage = 10 });

            var geoPhoto = photos.First();

            var geoPhotos = AuthInstance.PhotosGeoPhotosForLocation(geoPhoto.Latitude, geoPhoto.Longitude,
                                                                    GeoAccuracy.Street, PhotoSearchExtras.None, 100, 1);

            Assert.True(geoPhotos.Select(p => p.PhotoId).Contains(geoPhoto.PhotoId));
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void PhotosGetGetLocationTest()
        {
            var photos = AuthInstance.PhotosSearch(new PhotoSearchOptions { HasGeo = true, UserId = TestData.TestUserId, Extras = PhotoSearchExtras.Geo });

            var photo = photos.First();

            Console.WriteLine(photo.PhotoId);

            var location = AuthInstance.PhotosGeoGetLocation(photo.PhotoId);

            Assert.Equal(photo.Longitude, location.Longitude);//, "Longitudes should match exactly."
            Assert.Equal(photo.Latitude, location.Latitude);//, "Latitudes should match exactly."
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PhotosGetGetLocationNullTest()
        {
            var photos = AuthInstance.PhotosSearch(new PhotoSearchOptions { HasGeo = false, UserId = TestData.TestUserId, Extras = PhotoSearchExtras.Geo });

            var photo = photos.First();

            var location = AuthInstance.PhotosGeoGetLocation(photo.PhotoId);

            Assert.Null(location);// "Location should be null.");
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PhotosGetCorrectLocationTest()
        {
            var photo = AuthInstance.PhotosSearch(new PhotoSearchOptions { HasGeo = true, UserId = TestData.TestUserId, Extras = PhotoSearchExtras.Geo }).First();

            AuthInstance.PhotosGeoCorrectLocation(photo.PhotoId, photo.PlaceId, null);
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PhotosGeoSetContextTest()
        {
            var photo = AuthInstance.PhotosSearch(new PhotoSearchOptions { HasGeo = true, UserId = TestData.TestUserId, Extras = PhotoSearchExtras.Geo }).First();

            Assert.True(photo.GeoContext.HasValue, "GeoContext should be set.");

            var origContext = photo.GeoContext.Value;

            var newContext = origContext == GeoContext.Indoors ? GeoContext.Outdoors : GeoContext.Indoors;

            try
            {
                AuthInstance.PhotosGeoSetContext(photo.PhotoId, newContext);
            }
            finally
            {
                AuthInstance.PhotosGeoSetContext(photo.PhotoId, origContext);
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PhotosGeoSetLocationTest()
        {
            var photo = AuthInstance.PhotosSearch(new PhotoSearchOptions { HasGeo = true, UserId = TestData.TestUserId, Extras = PhotoSearchExtras.Geo }).First();

            if (photo.GeoContext == null)
            {
                Assert.False(true,"GeoContext should not be null");
            }

            var origGeo = new {photo.Latitude, photo.Longitude, photo.Accuracy, Context = photo.GeoContext.Value};
            var newGeo = new {Latitude = -23.32, Longitude = -34.2, Accuracy = GeoAccuracy.Level10, Context = GeoContext.Indoors};

            try
            {
                AuthInstance.PhotosGeoSetLocation(photo.PhotoId, newGeo.Latitude, newGeo.Longitude, newGeo.Accuracy, newGeo.Context);

                var location = AuthInstance.PhotosGeoGetLocation(photo.PhotoId);
                Assert.Equal(newGeo.Latitude, location.Latitude);//, "New Latitude should be set."
                Assert.Equal(newGeo.Longitude, location.Longitude);//, "New Longitude should be set."
                Assert.Equal(newGeo.Context, location.Context);//, "New Context should be set."
                Assert.Equal(newGeo.Accuracy, location.Accuracy);//, "New Accuracy should be set."
            }
            finally
            {
                AuthInstance.PhotosGeoSetLocation(photo.PhotoId, origGeo.Latitude, origGeo.Longitude, origGeo.Accuracy, origGeo.Context);
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PhotosGeoPhotosForLocationBasicTest()
        {
            var o = new PhotoSearchOptions
                        {
                            UserId = TestData.TestUserId,
                            HasGeo = true,
                            PerPage = 1,
                            Extras = PhotoSearchExtras.Geo
                        };

            var photos = AuthInstance.PhotosSearch(o);
            var photo = photos[0];

            var photos2 = AuthInstance.PhotosGeoPhotosForLocation(photo.Latitude, photo.Longitude, photo.Accuracy, PhotoSearchExtras.All, 0, 0);

            Assert.NotNull(photos2);//, "PhotosGeoPhotosForLocation should not return null."
            Assert.True(photos2.Count > 0, "Should return one or more photos.");

            foreach (var p in photos2)
            {
                Assert.NotNull(p.PhotoId);
                Assert.NotEqual(0, p.Longitude);
                Assert.NotEqual(0, p.Latitude);
            }

        }
    }
}

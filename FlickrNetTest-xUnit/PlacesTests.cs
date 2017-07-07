using System;

using Xunit;
using FlickrNet;
using Shouldly;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PlacesForUserTests
    /// </summary>
    
    public class PlacesTests : BaseTest
    {
        [Fact]
        public void PlacesFindBasicTest()
        {
            var places = Instance.PlacesFind("Newcastle");

            Assert.NotNull(places);
            Assert.NotEqual(0, places.Count);
        }

        [Fact]
        public void PlacesFindNewcastleTest()
        {
            var places = Instance.PlacesFind("Newcastle upon Tyne");

            Assert.NotNull(places);
            Assert.Equal(1, places.Count);
        }

        [Fact]
        public void PlacesFindByLatLongNewcastleTest()
        {
            double lat = 54.977;
            double lon = -1.612;

            var place = Instance.PlacesFindByLatLon(lat, lon);

            Assert.NotNull(place);
            Assert.Equal("Haymarket, Newcastle upon Tyne, England, GB, United Kingdom", place.Description);
        }

        [Fact]
        public void PlacesPlacesForUserAuthenticationRequiredTest()
        {
            Flickr f = Instance;
            Should.Throw<SignatureRequiredException>(() => f.PlacesPlacesForUser());
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PlacesPlacesForUserHasContinentsTest()
        {
            Flickr f = AuthInstance;
            PlaceCollection places = f.PlacesPlacesForUser();

            foreach (Place place in places)
            {
                Assert.NotNull(place.PlaceId);//, "PlaceId should not be null."
                Assert.NotNull(place.WoeId);//, "WoeId should not be null."
                Assert.NotNull(place.Description);//, "Description should not be null."
                Assert.Equal(PlaceType.Continent, place.PlaceType);//, "PlaceType should be continent."
            }

            Assert.Equal("6dCBhRRTVrJiB5xOrg", places[0].PlaceId);
            Assert.Equal("Europe", places[0].Description);
            Assert.Equal("l5geY0lTVrLoNkLgeQ", places[1].PlaceId);
            Assert.Equal("North America", places[1].Description);
        }

        [Fact(Skip="Not currently returning any records for some reason.")]
        public void PlacesGetChildrenWithPhotosPublicPlaceIdTest()
        {
            string placeId = "6dCBhRRTVrJiB5xOrg"; // Europe
            Flickr f = Instance;

            var places = f.PlacesGetChildrenWithPhotosPublic(placeId, null);
            Console.WriteLine(f.LastRequest);
            Console.WriteLine(f.LastResponse);

            Assert.NotNull(places);
            Assert.NotEqual(0, places.Count);

            foreach (var place in places)
            {
                Assert.Equal(PlaceType.Country, place.PlaceType);
            }
        }

        [Fact(Skip ="Not currently returning any records for some reason.")]
        public void PlacesGetChildrenWithPhotosPublicWoeIdTest()
        {
            string woeId = "24865675"; // Europe

            var places = Instance.PlacesGetChildrenWithPhotosPublic(null, woeId);
            Assert.NotNull(places);
            Assert.NotEqual(0, places.Count);

            foreach (var place in places)
            {
                Assert.Equal(PlaceType.Country, place.PlaceType);
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PlacesPlacesForUserContinentHasRegionsTest()
        {
            Flickr f = AuthInstance;

            // Test place ID of '6dCBhRRTVrJiB5xOrg' is Europe
            PlaceCollection p = f.PlacesPlacesForUser(PlaceType.Region, null, "6dCBhRRTVrJiB5xOrg");

            foreach (Place place in p)
            {
                Assert.NotNull(place.PlaceId);//, "PlaceId should not be null."
                Assert.NotNull(place.WoeId);//, "WoeId should not be null."
                Assert.NotNull(place.Description);//, "Description should not be null."
                Assert.NotNull(place.PlaceUrl);//, "PlaceUrl should not be null"
                Assert.Equal(PlaceType.Region, place.PlaceType);//, "PlaceType should be Region."
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PlacesPlacesForContactsBasicTest()
        {
            var f = AuthInstance;
            var places = f.PlacesPlacesForContacts(PlaceType.Country, null, null, 0, ContactSearch.AllContacts, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);

            Assert.NotNull(places);

            Assert.NotEqual(0, places.Count);

            foreach (var place in places)
            {
                Assert.Equal(PlaceType.Country, place.PlaceType);
                Assert.NotNull(place.PlaceId);
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PlacesPlacesForContactsFullParamTest()
        {
            DateTime lastYear = DateTime.Today.AddYears(-1);
            DateTime today = DateTime.Today;

            var f = AuthInstance;
            var places = f.PlacesPlacesForContacts(PlaceType.Country, null, null, 1, ContactSearch.AllContacts, lastYear, today, lastYear, today);

            Console.WriteLine(f.LastRequest);

            Assert.NotNull(places);

            Assert.NotEqual(0, places.Count);

            foreach (var place in places)
            {
                Assert.Equal(PlaceType.Country, place.PlaceType);
                Assert.NotNull(place.PlaceId);
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PlacesPlacesForTagsBasicTest()
        {
            var f = AuthInstance;
            var places = f.PlacesPlacesForTags(PlaceType.Country, null, null, 0, new string[] {"newcastle"},
                                               TagMode.AllTags, null, MachineTagMode.None, DateTime.MinValue,
                                               DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);

            Assert.NotNull(places);

            Assert.NotEqual(0, places.Count);

            foreach (var place in places)
            {
                Assert.Equal(PlaceType.Country, place.PlaceType);
                Assert.NotNull(place.PlaceId);
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PlacesPlacesForTagsFullParamTest()
        {
            var f = AuthInstance;
            var places = f.PlacesPlacesForTags(PlaceType.Country, null, null, 0, new string[] {"newcastle"},
                                               TagMode.AllTags, new string[] {"dc:author=*"}, MachineTagMode.AllTags,
                                               DateTime.Today.AddYears(-10), DateTime.Today,
                                               DateTime.Today.AddYears(-10), DateTime.Today);

            Assert.NotNull(places);
        }

        [Fact]
        public void PlacesGetInfoBasicTest()
        {
            var f = Instance;
            var placeId = "X9sTR3BSUrqorQ";
            PlaceInfo p = f.PlacesGetInfo(placeId, null);

            Console.WriteLine(f.LastResponse);

            Assert.NotNull(p);
            Assert.Equal(placeId, p.PlaceId);
            Assert.Equal("30079", p.WoeId);
            Assert.Equal(PlaceType.Locality, p.PlaceType);
            Assert.Equal("Newcastle upon Tyne, England, United Kingdom", p.Description);

            Assert.Equal("X9sTR3BSUrqorQ", p.Locality.PlaceId);
            Assert.Equal("myqh27pQULzLWcg7Kg", p.County.PlaceId);
            Assert.Equal("2eIY2QFTVr_DwWZNLg", p.Region.PlaceId);
            Assert.Equal("cnffEpdTUb5v258BBA", p.Country.PlaceId);

            Assert.True(p.HasShapeData);
            Assert.NotNull(p.ShapeData);
            Assert.Equal(0.00015, p.ShapeData.Alpha);
            Assert.Equal(1, p.ShapeData.PolyLines.Count);
            Assert.Equal(89, p.ShapeData.PolyLines[0].Count);
            Assert.Equal(55.030498504639, p.ShapeData.PolyLines[0][88].X);
            Assert.Equal(-1.6404060125351, p.ShapeData.PolyLines[0][88].Y);
        }

        [Fact]
        public void PlacesGetInfoByUrlBasicTest()
        {
            var f = Instance;
            var placeId = "X9sTR3BSUrqorQ";
            PlaceInfo p1 = f.PlacesGetInfo(placeId, null);
            PlaceInfo p2 = f.PlacesGetInfoByUrl(p1.PlaceUrl);

            Assert.NotNull(p2);
            Assert.Equal(p1.PlaceId, p2.PlaceId);
            Assert.Equal(p1.WoeId, p2.WoeId);
            Assert.Equal(p1.PlaceType, p2.PlaceType);
            Assert.Equal(p1.Description, p2.Description);

            Assert.NotNull(p2.PlaceFlickrUrl);
        }

        [Fact]
        public void PlacesGetTopPlacesListTest()
        {
            var f = Instance;
            var places = f.PlacesGetTopPlacesList(PlaceType.Continent);

            Assert.NotNull(places);
            Assert.NotEqual(0, places.Count);

            foreach (var p in places)
            {
                Assert.Equal(PlaceType.Continent, p.PlaceType);
                Assert.NotNull(p.PlaceId);
                Assert.NotNull(p.WoeId);
            }
        }

        [Fact]
        public void PlacesGetShapeHistoryTest()
        {
            var placeId = "X9sTR3BSUrqorQ";
            var f = Instance;
            var col = f.PlacesGetShapeHistory(placeId, null);

            Assert.NotNull(col);//, "ShapeDataCollection should not be null."
            Assert.Equal(7, col.Count);//, "Count should be six."
            Assert.Equal(placeId, col.PlaceId);

            Assert.Equal(1, col[1].PolyLines.Count);//, "The second shape should have one polylines."
        }

        [Fact]
        public void PlacesGetTagsForPlace()
        {
            var placeId = "X9sTR3BSUrqorQ";
            var f = Instance;
            var col = f.PlacesTagsForPlace(placeId, null);

            Assert.NotNull(col);//, "TagCollection should not be null."
            Assert.Equal(100, col.Count);//, "Count should be one hundred."

            foreach (var t in col)
            {
                Assert.NotEqual(0, t.Count);//, "Count should be greater than zero."
                Assert.NotNull(t.TagName);//, "TagName should not be null."
            }

        }

        [Fact]
        public void PlacesGetPlaceTypes()
        {
            var pts = Instance.PlacesGetPlaceTypes();
            Assert.NotNull(pts);
            Assert.True(pts.Count > 1, "Count should be greater than one. Count = " + pts.Count + ".");

            foreach (var kp in pts)
            {
                Assert.NotEqual(0, kp.Id);//, "Key should not be zero."
                Assert.NotNull(kp.Name);//, "Value should not be null."

                Assert.True(Enum.IsDefined(typeof(PlaceType), kp.Id), "PlaceType with ID " + kp.Id + " and Value '" + kp.Name + "' not defined in PlaceType enum.");
                var type = (PlaceType)kp.Id;
                Assert.Equal(type.ToString("G").ToLower(), kp.Name);//, "Name of enum should match."
            }
        }

        [Fact]
        public void PlacesPlacesForBoundingBoxUsaTest()
        {
            Flickr f = Instance;

            var places = f.PlacesPlacesForBoundingBox(PlaceType.County, null, null, BoundaryBox.UKNewcastle);

            Assert.NotNull(places);
            Assert.NotEqual(0, places.Count);

            foreach (var place in places)
            {
                Assert.NotNull(place.PlaceId);
                Assert.Equal(PlaceType.County, place.PlaceType);
            }
        }

    }
}

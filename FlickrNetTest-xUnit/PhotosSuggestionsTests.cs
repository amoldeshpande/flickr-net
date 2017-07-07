using System;
using System.Linq;
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    
    public class PhotosSuggestionsTests : BaseTest
    {
        string photoId = "6282363572";

        //[SetUp]
        public PhotosSuggestionsTests()
        {
            Flickr.CacheDisabled = true;
        }
        
        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void GetListTest()
        {
            var f = AuthInstance;

            // Remove any pending suggestions
            var suggestions = f.PhotosSuggestionsGetList(photoId, SuggestionStatus.Pending);
            Assert.NotNull(suggestions);//, "SuggestionCollection should not be null."

            foreach (var s in suggestions)
            {
                if (s.SuggestionId == null)
                {
                    Console.WriteLine(f.LastRequest);
                    Console.WriteLine(f.LastResponse);
                }
                Assert.NotNull(s.SuggestionId);//, "Suggestion ID should not be null."
                f.PhotosSuggestionsRemoveSuggestion(s.SuggestionId);
            }

            // Add test suggestion
            AddSuggestion();

            // Get list of suggestions and check
            suggestions = f.PhotosSuggestionsGetList(photoId, SuggestionStatus.Pending);

            Assert.NotNull(suggestions);//, "SuggestionCollection should not be null."
            Assert.NotEqual(0, suggestions.Count);//, "Count should not be zero."

            var suggestion = suggestions.First();

            Assert.Equal("41888973@N00", suggestion.SuggestedByUserId);
            Assert.Equal("Sam Judson", suggestion.SuggestedByUserName);
            Assert.Equal("I really think this is a good suggestion.", suggestion.Note);
            Assert.Equal(54.977, suggestion.Latitude);//, "Latitude should be the same."

            f.PhotosSuggestionsRemoveSuggestion(suggestion.SuggestionId);

            // Add test suggestion
            AddSuggestion();
            suggestion = f.PhotosSuggestionsGetList(photoId, SuggestionStatus.Pending).First();
            f.PhotosSuggestionsApproveSuggestion(suggestion.SuggestionId);
            f.PhotosSuggestionsRemoveSuggestion(suggestion.SuggestionId);

        }

        [Fact]
        public void AddSuggestion()
        {
            var f = AuthInstance;

            var lat = 54.977;
            var lon = -1.612;
            var accuracy = GeoAccuracy.Street;
            var woeId = "30079";
            var placeId = "X9sTR3BSUrqorQ";
            var note = "I really think this is a good suggestion.";

            f.PhotosSuggestionsSuggestLocation(photoId, lat, lon, accuracy, woeId, placeId, note);
        }
    }
}

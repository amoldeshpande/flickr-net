using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Gets a list of contacts for the logged in user.
        /// Requires authentication.
        /// </summary>
       
        public async Task<FlickrResult<ContactCollection>> ContactsGetListAsync()
        {
            return await ContactsGetListAsync(null, 0, 0);
        }

        /// <summary>
        /// Gets a list of contacts for the logged in user.
        /// Requires authentication.
        /// </summary>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of contacts to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
       
        public async Task<FlickrResult<ContactCollection>> ContactsGetListAsync(int page, int perPage)
        {
            return await ContactsGetListAsync(null, page, perPage);
        }

        /// <summary>
        /// Gets a list of contacts for the logged in user.
        /// Requires authentication.
        /// </summary>
        /// <param name="filter">An optional filter of the results. The following values are valid: "friends", "family", "both", "neither".</param>
       
        public async Task<FlickrResult<ContactCollection>> ContactsGetListAsync(string filter)
        {
            return await ContactsGetListAsync(filter, 0, 0);
        }

        /// <summary>
        /// Gets a list of contacts for the logged in user.
        /// Requires authentication.
        /// </summary>
        /// <param name="filter">An optional filter of the results. The following values are valid: "friends", "family", "both", "neither".</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of contacts to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
       
        public async Task<FlickrResult<ContactCollection>> ContactsGetListAsync(string filter, int page, int perPage)
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.contacts.getList");
            if (!string.IsNullOrEmpty(filter)) parameters.Add("filter", filter);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<ContactCollection>(parameters);
        }

        /// <summary>
        /// Return a list of contacts for a user who have recently uploaded photos along with the total count of photos uploaded.
        /// </summary>
        /// <remarks>
        /// This method is still considered experimental. We don't plan for it to change or to go away but so long as this notice is present you should write your code accordingly.
        /// </remarks>
       
        public async Task<FlickrResult<ContactCollection>> ContactsGetListRecentlyUploadedAsync()
        {
            return await ContactsGetListRecentlyUploadedAsync(DateTime.MinValue, null);
        }

        /// <summary>
        /// Return a list of contacts for a user who have recently uploaded photos along with the total count of photos uploaded.
        /// </summary>
        /// <remarks>
        /// This method is still considered experimental. We don't plan for it to change or to go away but so long as this notice is present you should write your code accordingly.
        /// </remarks>
        /// <param name="filter">Limit the result set to all contacts or only those who are friends or family. Valid options are:
        /// "ff" friends and family, and "all" all your contacts.
        /// Default value is "all".</param>
       
        public async Task<FlickrResult<ContactCollection>> ContactsGetListRecentlyUploadedAsync(string filter)
        {
            return await ContactsGetListRecentlyUploadedAsync(DateTime.MinValue, filter);
        }

        /// <summary>
        /// Return a list of contacts for a user who have recently uploaded photos along with the total count of photos uploaded.
        /// </summary>
        /// <remarks>
        /// This method is still considered experimental. We don't plan for it to change or to go away but so long as this notice is present you should write your code accordingly.
        /// </remarks>
        /// <param name="dateLastUpdated">Limits the resultset to contacts that have uploaded photos since this date. The default offset is (1) hour and the maximum (24) hours.</param>
       
        public async Task<FlickrResult<ContactCollection>> ContactsGetListRecentlyUploadedAsync(DateTime dateLastUpdated)
        {
            return await ContactsGetListRecentlyUploadedAsync(dateLastUpdated, null);
        }

        /// <summary>
        /// Return a list of contacts for a user who have recently uploaded photos along with the total count of photos uploaded.
        /// </summary>
        /// <remarks>
        /// This method is still considered experimental. We don't plan for it to change or to go away but so long as this notice is present you should write your code accordingly.
        /// </remarks>
        /// <param name="dateLastUpdated">Limits the resultset to contacts that have uploaded photos since this date. The default offset is (1) hour and the maximum (24) hours.</param>
        /// <param name="filter">Limit the result set to all contacts or only those who are friends or family. Valid options are:
        /// "ff" friends and family, and "all" all your contacts.
        /// Default value is "all".</param>
       
        public async Task<FlickrResult<ContactCollection>> ContactsGetListRecentlyUploadedAsync(DateTime dateLastUpdated, string filter)
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();

            parameters.Add("method", "flickr.contacts.getListRecentlyUploaded");
            if (dateLastUpdated != DateTime.MinValue) parameters.Add("date_lastupload", UtilityMethods.DateToUnixTimestamp(dateLastUpdated));
            if (!string.IsNullOrEmpty(filter)) parameters.Add("filter", filter);

            return await GetResponseAsync<ContactCollection>(parameters);

        }

        /// <summary>
        /// Gets a list of the given users contact, or those that are publically avaiable.
        /// </summary>
        /// <param name="userId">The Id of the user who's contacts you want to return.</param>
       
        public async Task<FlickrResult<ContactCollection>> ContactsGetPublicListAsync(string userId)
        {
            return await ContactsGetPublicListAsync(userId, 0, 0);
        }

        /// <summary>
        /// Gets a list of the given users contact, or those that are publically avaiable.
        /// </summary>
        /// <param name="userId">The Id of the user who's contacts you want to return.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of contacts to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
       
        public async Task<FlickrResult<ContactCollection>> ContactsGetPublicListAsync(string userId, int page, int perPage)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.contacts.getPublicList");
            parameters.Add("user_id", userId);
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

            return await GetResponseAsync<ContactCollection>(parameters);
        }

        /// <summary>
        /// Returns a list of contacts who Flickr suggests you might want to tag.
        /// </summary>
        /// <remarks>
        /// Not sure exactly what the purpose of this function is as it appears to just return a list of all my contacts.
        /// </remarks>
       
        /// <returns></returns>
        public async Task<FlickrResult<ContactCollection>>  ContactsGetTaggingSuggestionsAsync()
        {
            return await ContactsGetTaggingSuggestionsAsync(0, 0);
        }

        /// <summary>
        /// Returns a list of contacts who Flickr suggests you might want to tag.
        /// </summary>
        /// <remarks>
        /// Not sure exactly what the purpose of this function is as it appears to just return a list of all my contacts.
        /// </remarks>
        /// <param name="page">The page of results to return. Default is 1.</param>
        /// <param name="perPage">The number of contacts to return per page.</param>
       
        /// <returns></returns>
        public async Task<FlickrResult<ContactCollection>> ContactsGetTaggingSuggestionsAsync(int page, int perPage)
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.contacts.getTaggingSuggestions");
            if (page > 0) parameters.Add("page", page.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (perPage > 0) parameters.Add("per_page", perPage.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            return await GetResponseAsync<ContactCollection>(parameters);
        }
    }
}

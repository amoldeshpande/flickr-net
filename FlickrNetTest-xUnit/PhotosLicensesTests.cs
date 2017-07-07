using System;
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    
    public class PhotosLicensesTests : BaseTest
    {
        [Fact]
        public void PhotosLicensesGetInfoBasicTest()
        {
            LicenseCollection col = Instance.PhotosLicensesGetInfo();

            foreach (License lic in col)
            {
                if (!Enum.IsDefined(typeof(LicenseType), lic.LicenseId))
                {
                    Assert.False(true,"License with ID " + (int)lic.LicenseId + ", " + lic.LicenseName + " dooes not exist.");
                }
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void PhotosLicensesSetLicenseTest()
        {
            Flickr f = AuthInstance;
            string photoId = "7176125763";

            var photoInfo = f.PhotosGetInfo(photoId); // Rainbow Rose
            var origLicense = photoInfo.License;

            var newLicense = origLicense == LicenseType.AttributionCC ? LicenseType.AttributionNoDerivativesCC : LicenseType.AttributionCC;
            f.PhotosLicensesSetLicense(photoId, newLicense);

            var newPhotoInfo = f.PhotosGetInfo(photoId);

            Assert.Equal(newLicense, newPhotoInfo.License);//, "License has not changed"

            // Reset license 
            f.PhotosLicensesSetLicense(photoId, origLicense);
        }

    }
}

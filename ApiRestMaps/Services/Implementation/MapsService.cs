using ApiRestMaps.Configuration;
using ApiRestMaps.Data.GoogleObj;
using ApiRestMaps.Data.VO;
using Microsoft.Extensions.Options;

namespace ApiRestMaps.Services.Implementation
{
    public class MapsService : IMapsService
    {
        private GoogleCloud _googleCloud { get; }

        public MapsService(IOptions<GoogleCloud> googleCloud)
        {
            _googleCloud = googleCloud.Value ?? throw new ArgumentNullException(nameof(googleCloud)); ;
        }

        /**
         * Call the function who connect to APIs of GoogleMaps and calculate the distance
         */
        public async Task<List<MapsResult>> calculateDistance(List<AddressObj> address)
        {
            var objList = new List<GoogleReturnObj>();


            // Feeding array with results from google
            foreach (AddressObj addressObj in address)
            {
                // Making address in right request format
                var addressStr = addressObj.Address.Replace(" ", "+");

                // Get address info from google
                var ret = await getAddressInfo(addressStr);

                // Add to temp array var
                objList.Add(ret);
            }

            /**
             *  Now the program will calculate the distance beetwen the points
             */

            // Count variables

            // Bigest distance in the Earth is 23.068 Km  Route od L'Agulhas, South Africa to Magadan, Russia
            double closest = 23069;
            double further = 0;

            var retObj = new List<MapsResult>();

            // Now we calculate the distance
            for (int i = 0; i < objList.Count; i++)
            {
                for (int j = i + 1; j < objList.Count; j++)
                {
                    // Save Addresses
                    var addresses = new List<AddressObj>();

                    addresses.Add(new AddressObj { Address = objList[i].Results[0].FormattedAddress });
                    addresses.Add(new AddressObj { Address = objList[j].Results[0].FormattedAddress });

                    // Calculate the diference
                    var subLat = objList[i].Results[0].Geometry.Location.Lat - objList[j].Results[0].Geometry.Location.Lat;
                    var subLng = objList[i].Results[0].Geometry.Location.Lng - objList[j].Results[0].Geometry.Location.Lng;

                    // Find the distance using Pitagoras
                    var hipotenusa = Math.Sqrt(Math.Pow((subLat * 111.111111), 2) + Math.Pow((subLng * 111.111111), 2));

                    // Compare to save the closest and further
                    if (closest > hipotenusa) { closest = hipotenusa; }
                    if (further < hipotenusa) { further = hipotenusa; }

                    // Make return json obj
                    retObj.Add(new MapsResult
                    {
                        AddressArray = addresses,
                        Distance = hipotenusa,
                        Closeset = false,
                        Further = false
                    });
                }
            }

            // Mark as closest and further
            foreach (var compDistance in retObj)
            {
                if (compDistance.Distance == closest) { compDistance.Closeset = true; }
                if (compDistance.Distance == further) { compDistance.Further = true; }
            }

            return retObj;
        }

        /**
         * Get info from addresses with GoogleMaps API
         */
        public async Task<GoogleReturnObj> getAddressInfo(String strAddress)
        {
            try
            {

                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync("https://maps.googleapis.com/maps/api/geocode/json?address=" + 
                    strAddress + "&key=" + _googleCloud.ApiKey);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<GoogleReturnObj>();
                }

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}

using ApiRestMaps.Data.GoogleObj;
using ApiRestMaps.Data.VO;

namespace ApiRestMaps.Services
{
    public interface IMapsService
    {
        public Task<List<MapsResult>> calculateDistance(List<AddressObj> address);
        public Task<GoogleReturnObj> getAddressInfo(String strAddress);
    }
}

using Postcodes.Models.Request;
using Postcodes.Models.Response;

namespace Postcodes.Services
{
    public interface IPostcodeApiService
    {
        public Task<PostcodeResponse> GetPostcodeDetailsAsync(string postcode);
        public Task<List<PostcodeResponse>> GetPostcodeDetailsAsync(PostcodesRequest postcodesRequest);
        public void RunHealthCheck();
    }
}

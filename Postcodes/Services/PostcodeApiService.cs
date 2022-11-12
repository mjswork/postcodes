using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Postcodes.Models;
using Postcodes.Models.Request;
using Postcodes.Models.Response;
using Postcodes.Models.ThirdParty;
using System.Net;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;

namespace Postcodes.Services
{
    public class PostcodeApiService : IPostcodeApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PostcodeApiService> _logger;
        private readonly IConfiguration _configuration;
        private string GETEndpoint { get; set; }

        public PostcodeApiService(HttpClient httpClient, ILogger<PostcodeApiService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            GETEndpoint = _configuration.GetValue<string>("GETendpoint");
        }

        public async Task<PostcodeResponse> GetPostcodeDetailsAsync(string postcode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{GETEndpoint}/{postcode}");
                var result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.NotFound || String.IsNullOrEmpty(result))
                    throw new KeyNotFoundException(ErrorMessages.NotFoundMessage);

                return JsonConvert.DeserializeObject<PostcodeResponse>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task<List<PostcodeResponse>> GetPostcodeDetailsAsync(PostcodesRequest postcodesRequest)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{GETEndpoint}",postcodesRequest);
                var result = await response.Content.ReadAsStringAsync();

                var apiResult = JsonConvert.DeserializeObject<PostcodeMultiResult>(result);

                if (!apiResult?.result.Any(x => x.result != null) ?? false)
                    throw new KeyNotFoundException(ErrorMessages.CouldNotMatchAny);

                return ProcessMultiResults(apiResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public void RunHealthCheck()
        {
            var response = _httpClient.GetAsync($"random/postcodes");
            var result = response.Result.Content.ReadAsStringAsync();

            var apiResult = JsonConvert.DeserializeObject<PostcodeSingleResult>(result.Result);

            if (apiResult?.status != (int)HttpStatusCode.OK)
                _logger.LogCritical("https://postcodes.io/ appears to be down");
        }

        private List<PostcodeResponse> ProcessMultiResults(PostcodeMultiResult apiResult)
        {
            return apiResult.result.Select(x =>
            {
                var ret = new PostcodeResponse
                {
                    Postcode = x?.query ?? null,
                };

                if (x?.result != null)
                {
                    ret.Coordinates = new Coordinate
                    {
                        Latitude = x.result?.latitude.ToString() ?? null,
                        Longitude = x.result?.longitude.ToString() ?? null
                    };
                }
                return ret;
            }).ToList();
        }
    }
}

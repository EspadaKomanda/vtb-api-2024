using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using OpenApiService.Exceptions.RequestSender;

namespace OpenApiService.Utils;
public class RequestSender(ILogger<RequestSender> logger, HttpClient client)
{
    private readonly ILogger<RequestSender> _logger = logger;
    private readonly HttpClient _client = client;
    public string BuildRequestUri(string url, string? pathParameter, Dictionary<string,object>? queryParameters)
    {
        try
        {
            var uriBuilder = new UriBuilder(url + pathParameter);
            
            if(queryParameters !=null)
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                _logger.LogDebug("Building uri with query paramete");
                foreach(var queryParameter in queryParameters)
                {
                    query[queryParameter.Key]=queryParameter.Value.ToString();
                }
                uriBuilder.Query = query.ToString();
            }
            _logger.LogDebug("Uri built successfuly!");
            return uriBuilder.ToString();

        }
        catch (Exception ex)
        {
            var exception = new BuildUriException("Failed to build uri!", ex);
            _logger.LogError(exception,"Failed to build uri!");
            throw exception;
        }
        
    }
    public async Task<HttpResponseMessage> SendGetRequestAsync( Dictionary<string,IEnumerable<string>>? headers, string uri )
    {
        try
        {
            _logger.LogDebug("Building get request");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };
            foreach(var header in headers)
            {
                request.Headers.Add(header.Key,header.Value);
            }
            var response = await _client.SendAsync(request);
            _logger.LogDebug("Response aquired successfully");
            return response;
        }
        catch (Exception ex)
        {
            var exception = new SendRequestException("Failed to build or send request!", ex);
            _logger.LogError(exception,"Failed to build or send request!");
            throw exception;
        }
    }
    public async Task<HttpResponseMessage> SendPostRequestAsync(object? message, Dictionary<string,IEnumerable<string>>? headers, string uri )
    {
        try
        {
            _logger.LogDebug("Building post request");
            if(message == null)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(uri)
                };
                foreach(var header in headers)
                {
                    request.Headers.Add(header.Key,header.Value);
                }
                var response = await _client.SendAsync(request);
                _logger.LogDebug("Response aquired successfully");
                return response; 
            }
            else
            {
                using StringContent jsonContent = new(
                                JsonConvert.SerializeObject(message),
                                Encoding.UTF8,
                                "application/json");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(uri),
                    Content = jsonContent
                };
                foreach(var header in headers)
                {
                    request.Headers.Add(header.Key,header.Value);
                }
                var response = await _client.SendAsync(request);
                _logger.LogDebug("Response aquired successfully");
                return response; 
            }
            
        }
        catch (Exception ex)
        {
            var exception = new SendRequestException("Failed to build or send request!", ex);
            _logger.LogError(exception,"Failed to build or send request!");
            throw exception;
        }
    }
    public async Task<HttpResponseMessage> SendPutRequestAsync(object? message, Dictionary<string,IEnumerable<string>>? headers, string uri )
    {
        try
        {
            _logger.LogDebug("Building put request");
            if(message == null)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(uri)
                };
                foreach(var header in headers)
                {
                    request.Headers.Add(header.Key,header.Value);
                }
                var response = await _client.SendAsync(request);
                _logger.LogDebug("Response aquired successfully");
                return response; 
            }
            else
            {
                using StringContent jsonContent = new(
                                JsonConvert.SerializeObject(message),
                                Encoding.UTF8,
                                "application/json");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(uri),
                    Content = jsonContent
                };
                foreach(var header in headers)
                {
                    request.Headers.Add(header.Key,header.Value);
                }
                var response = await _client.SendAsync(request);
                _logger.LogDebug("Response aquired successfully");
                return response; 
            }
            
        }
        catch (Exception ex)
        {
            var exception = new SendRequestException("Failed to build or send request!", ex);
            _logger.LogError(exception,"Failed to build or send request!");
            throw exception;
        }
    }
    public async Task<HttpResponseMessage> SendDeleteRequestAsync(object? message, Dictionary<string,IEnumerable<string>>? headers, string uri )
    {
        try
        {
            _logger.LogDebug("Building delete request");
            if(message == null)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(uri)
                };
                foreach(var header in headers)
                {
                    request.Headers.Add(header.Key,header.Value);
                }
                var response = await _client.SendAsync(request);
                _logger.LogDebug("Response aquired successfully");
                return response; 
            }
            else
            {
                using StringContent jsonContent = new(
                                JsonConvert.SerializeObject(message),
                                Encoding.UTF8,
                                "application/json");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(uri),
                    Content = jsonContent
                };
                foreach(var header in headers)
                {
                    request.Headers.Add(header.Key,header.Value);
                }
                var response = await _client.SendAsync(request);
                _logger.LogDebug("Response aquired successfully");
                return response; 
            }
            
        }
        catch (Exception ex)
        {
            var exception = new SendRequestException("Failed to build or send request!", ex);
            _logger.LogError(exception,"Failed to build or send request!");
            throw exception;
        }
    }
    public async Task<HttpResponseMessage> SendPatchRequestAsync(object? message, Dictionary<string,IEnumerable<string>>? headers, string uri )
    {
        try
        {
            _logger.LogDebug("Building patch request");
            if(message == null)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri(uri)
                };
                foreach(var header in headers)
                {
                    request.Headers.Add(header.Key,header.Value);
                }
                var response = await _client.SendAsync(request);
                _logger.LogDebug("Response aquired successfully");
                return response; 
            }
            else
            {
                using StringContent jsonContent = new(
                                JsonConvert.SerializeObject(message),
                                Encoding.UTF8,
                                "application/json");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri(uri),
                    Content = jsonContent
                };
                foreach(var header in headers)
                {
                    request.Headers.Add(header.Key,header.Value);
                }
                var response = await _client.SendAsync(request);
                _logger.LogDebug("Response aquired successfully");
                return response; 
            }
            
        }
        catch (Exception ex)
        {
            var exception = new SendRequestException("Failed to build or send request!", ex);
            _logger.LogError(exception,"Failed to build or send request!");
            throw exception;
        }
    }
}
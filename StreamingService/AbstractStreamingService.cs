using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using RestSharp;
using StreamingServiceCompare.StreamingService.Deezer;
using StreamingServiceCompare.StreamingService.Qobuz;
using StreamingServiceCompare.StreamingService.Spotify;
using StreamingServiceCompare.StreamingService.Tidal;

namespace StreamingServiceCompare.StreamingService
{
    abstract class AbstractStreamingService
    {
        private static List<AbstractStreamingService> services;

        protected RestClient client;
        protected RegionInfo region;

        public AbstractStreamingService(string restServerUrl)
        {
            client = new RestClient(restServerUrl)
            {
                UserAgent = "okhttp/2.2.0",
                CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.CacheIfAvailable)
            };

            region = RegionInfo.CurrentRegion;
        }

        public static List<AbstractStreamingService> GetServices()
        {
            if (services == null)
            {
                services = new List<AbstractStreamingService>() {/*new SpotifyStreamingService(), new TidalStreamingService(), new QobuzStreamingService(),*/ new DeezerStreamingService()};
            }
            return services;
        }

        protected virtual string OffsetParameter { get; } = "offset";

        public abstract string Name { get; }

        public abstract bool HasArtist(string artist);

        public abstract bool HasAlbum(string artist, string album);

        public abstract bool HasSong(string artist, string song);

        protected virtual bool ShouldRetryRequest<T>(IRestRequest request, IRestResponse<T> response, Exception exception)
        {
            return false;
        }

        protected bool HasItem<T>(IRestRequest request, Predicate<T> criteria, int offset, Converter<T, int> limit,
                                  Converter<T, int> total) where T : new()
        {
            if (OffsetParameter != null || offset == 0)
            {
                request.Parameters.RemoveAll(p => p.Name.Equals(OffsetParameter));
                request.AddParameter(OffsetParameter, offset);
            }
            else
            {
                // This service doesn't support offsets, return false as the parent call didn't find anything yet
                return false;
            }

            IRestResponse<T> response = client.Execute<T>(request);
            if (response.ErrorException == null && response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    if (!criteria(response.Data))
                    {
                        if (offset + limit(response.Data) < total(response.Data))
                        {
                            // Try with data from the next page
                            return HasItem(request, criteria, offset + limit(response.Data), limit, total);
                        }
                        // We didn't find anything
                        return false;
                    }
                    // The criteria matched successfully
                    return true;
                }
                catch (InvalidResultException exception)
                {
                    // The request was successful but processing it showed that it represents an error state
                    return HandleRequestException(request, response, exception, criteria, offset, limit, total);
                }
            }
            // Try to handle the exception state
            return HandleRequestException(request, response, response.ErrorException, criteria, offset, limit, total);
        }

        private bool HandleRequestException<T>(IRestRequest request, IRestResponse<T> response, Exception exception,
                                            Predicate<T> criteria, int offset, Converter<T, int> limit,
                                            Converter<T, int> total) where T : new()
        {
            var webException = exception as WebException;
            if (webException != null)
            {
                if (webException.Status == WebExceptionStatus.ConnectionClosed ||
                    webException.Status == WebExceptionStatus.PipelineFailure ||
                    webException.Status == WebExceptionStatus.ReceiveFailure ||
                    webException.Status == WebExceptionStatus.KeepAliveFailure ||
                    webException.Status == WebExceptionStatus.RequestCanceled ||
                    webException.Status == WebExceptionStatus.SendFailure ||
                    webException.Status == WebExceptionStatus.Timeout ||
                    ShouldRetryRequest(request, response, webException))
                {
                    // Try again in certain recoverable error cases
                    return HasItem(request, criteria, offset, limit, total);
                }
            }
            else if (ShouldRetryRequest(request, response, exception))
            {
                // A service-specific exception occured from which we can recover
                return HasItem(request, criteria, offset, limit, total);
            }
            // We encountered an unrecoverable error
            return false;
        }

        // Source: http://archives.miloush.net/michkap/archive/2007/05/14/2629747.html
        protected string RemoveDiacritics(string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}

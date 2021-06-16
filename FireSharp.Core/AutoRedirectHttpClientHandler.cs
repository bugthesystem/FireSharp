using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FireSharp.Core 
{
    internal class AutoRedirectHttpClientHandler : DelegatingHandler 
    {
        private int maximumAutomaticRedirections;

        public int MaximumAutomaticRedirections 
        {
          get => maximumAutomaticRedirections;
          set 
          {
              if (value < 1) 
              {
                throw new ArgumentException("The specified value must be greater than 0.");
              }

              maximumAutomaticRedirections = value;
          }
        }

        public AutoRedirectHttpClientHandler()
        {
            HttpClientHandler handler = new() { AllowAutoRedirect = false };
            InnerHandler = handler;
            MaximumAutomaticRedirections = handler.MaxAutomaticRedirections;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) 
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!IsRedirect(response)) 
            {
                return response;
            }

            int redirectCount = 0;

            while (IsRedirect(response)) 
            {
                redirectCount++;

                if (redirectCount > MaximumAutomaticRedirections) 
                {
                   throw new WebException("Too many automatic redirections were attempted.");
                }

                request.RequestUri = response.Headers.Location;
                response = await SendAsync(request, cancellationToken).ConfigureAwait(false);
            }

            return response;
        }

        private static bool IsRedirect(HttpResponseMessage response) 
        {
            switch (response.StatusCode) 
            {
                case HttpStatusCode.MovedPermanently:
                case HttpStatusCode.Redirect:
                case HttpStatusCode.RedirectMethod:
                case HttpStatusCode.RedirectKeepVerb:
                    return true;
                default:
                    return false;
            }
        }
    }
}
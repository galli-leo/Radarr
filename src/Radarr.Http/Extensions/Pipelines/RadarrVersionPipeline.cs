using System;
using Nancy;
using Nancy.Bootstrapper;
using NzbDrone.Common.EnvironmentInfo;

namespace Radarr.Http.Extensions.Pipelines
{
    public class RadarrVersionPipeline : IRegisterNancyPipeline
    {
        public int Order => 0;

        public void Register(IPipelines pipelines)
        {
            pipelines.AfterRequest.AddItemToStartOfPipeline((Action<NancyContext>)Handle);
        }

        private void Handle(NancyContext context)
        {
            if (!context.Response.Headers.ContainsKey("X-Application-Version"))
            {
                context.Response.Headers.Add("X-Application-Version", BuildInfo.Version.ToString());
            }
        }
    }
}

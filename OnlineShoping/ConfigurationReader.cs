using Microsoft.Extensions.Configuration;

namespace OnlineShoping
{
    public class ConfigurationReader
    {
        private readonly IConfiguration _configuration;

        public string Url { get; private set; }
        public string StandardUser { get; private set; }
        public string LockedOutUser { get; private set; }
        public string ProblemUser { get; private set; }
        public string PerformanceGlitchUser { get; private set; }
        public string ErrorUser { get; private set; }
        public string VisualUser { get; private set; }
        public string Password { get; private set; }
        public string Browser { get; private set; }
        public bool IsHeadless { get; private set; }

        public ConfigurationReader()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            Url = _configuration["ApplicationSettings:Url"];
            StandardUser = _configuration["ApplicationSettings:StandardUser"];
            LockedOutUser = _configuration["ApplicationSettings:LockedOutUser"];
            ProblemUser = _configuration["ApplicationSettings:ProblemUser"];
            PerformanceGlitchUser = _configuration["ApplicationSettings:PerformanceGlitchUser"];
            ErrorUser = _configuration["ApplicationSettings:ErrorUser"];
            VisualUser = _configuration["ApplicationSettings:VisualUser"];
            Password = _configuration["ApplicationSettings:Password"];
            Browser = _configuration["ApplicationSettings:Browser"];
            IsHeadless = bool.TryParse(_configuration["ApplicationSettings:IsHeadless"], out bool result) ? result : false;
        }
    }
}

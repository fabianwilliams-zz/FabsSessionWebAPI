using System;
using Newtonsoft.Json;

public partial class FabsSession
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString("n");

        [JsonProperty("sessionNumber")]
        public string SessionNumber { get; set; }

        [JsonProperty("sessionName")]
        public string SessionName { get; set; }

        [JsonProperty("sessionDate")]
        public string SessionDate { get; set; }

        [JsonProperty("sessionCity")]
        public string SessionCity { get; set; }

        [JsonProperty("sessionRegionState")]
        public string SessionRegionState { get; set; }

        [JsonProperty("sessionCountry")]
        public string SessionCountry { get; set; }

        [JsonProperty("sessionReview")]
        public SessionReview[] SessionReview { get; set; }
    }

    public partial class SessionReview
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString("n");

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("zerotofiveRating")]
        public long ZerotofiveRating { get; set; }

        [JsonProperty("feedback")]
        public string Feedback { get; set; }

        [JsonProperty("requestContact")]
        public bool RequestContact { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class CreateSession 
    {
        [JsonProperty("sessionNumber")]
        public string SessionNumber { get; set; }

        [JsonProperty("sessionName")]
        public string SessionName { get; set; }

        [JsonProperty("sessionDate")]
        public string SessionDate { get; set; }

        [JsonProperty("sessionCity")]
        public string SessionCity { get; set; }

        [JsonProperty("sessionRegionState")]
        public string SessionRegionState { get; set; }

        [JsonProperty("sessionCountry")]
        public string SessionCountry { get; set; }

        [JsonProperty("sessionReview")]
        public SessionReview[] SessionReview { get; set; }
    }

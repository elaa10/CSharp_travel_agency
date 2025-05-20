using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RestServices
{
    public class Trip
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("touristAttraction")]
        public string TouristAttraction { get; set; }

        [JsonProperty("transportCompany")]
        public string TransportCompany { get; set; }

        [JsonProperty("departureTime")]
        public DateTime DepartureTime { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("availableSeats")]
        public int AvailableSeats { get; set; }

        public override string ToString()
        {
            return $"Trip [Id={Id}, Attraction={TouristAttraction}, Company={TransportCompany}, Time={DepartureTime}, Price={Price}, Seats={AvailableSeats}]";
        }
    }

    public class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Request:");
            Console.WriteLine(request.ToString());
            if (request.Content != null)
                Console.WriteLine(await request.Content.ReadAsStringAsync());
            Console.WriteLine();

            var response = await base.SendAsync(request, cancellationToken);

            Console.WriteLine("Response:");
            Console.WriteLine(response.ToString());
            if (response.Content != null)
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            Console.WriteLine();

            return response;
        }
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient(new LoggingHandler(new HttpClientHandler()));
        private static readonly string BASE_URL = "http://localhost:8080/trips";

        static async Task Main(string[] args)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // 1. Create a new Trip
            var trip = new Trip
            {
                TouristAttraction = "Castelul Peles",
                TransportCompany = "PelesTrans",
                DepartureTime = new DateTime(2025, 7, 5, 8, 30, 0),
                Price = 185.0,
                AvailableSeats = 40
            };

            Console.WriteLine("Creating Trip...");
            var created = await CreateTripAsync(BASE_URL, trip);
            Console.WriteLine("Created: " + created);

            // 2. Update Trip
            created.Price = 210.0;
            await UpdateTripAsync($"{BASE_URL}/{created.Id}", created);

            // 3. Get by ID
            var one = await GetTripAsync($"{BASE_URL}/{created.Id}");
            Console.WriteLine("Fetched by ID: " + one);

            // 4. Get all
            Console.WriteLine("All Trips:");
            var allTripsResponse = await client.GetAsync(BASE_URL);
            if (allTripsResponse.IsSuccessStatusCode)
            {
                var json = await allTripsResponse.Content.ReadAsStringAsync();
                var trips = JsonConvert.DeserializeObject<Trip[]>(json);
                foreach (var t in trips)
                    Console.WriteLine(t);
            }

            // 5. Delete
            await DeleteTripAsync($"{BASE_URL}/{created.Id}");
            Console.WriteLine("Deleted trip with ID: " + created.Id);

            // 6. Verify delete
            var afterDelete = await GetTripAsync($"{BASE_URL}/{created.Id}");
            Console.WriteLine("After delete: " + (afterDelete == null ? "Trip not found" : afterDelete.ToString()));
        }

        static async Task<Trip> GetTripAsync(string url)
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Trip>(json);
            }
            return null;
        }

        static async Task<Trip> CreateTripAsync(string url, Trip trip)
        {
            var response = await client.PostAsJsonAsync(url, trip);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Trip>(json);
            }
            return null;
        }

        static async Task UpdateTripAsync(string url, Trip trip)
        {
            await client.PutAsJsonAsync(url, trip);
        }

        static async Task DeleteTripAsync(string url)
        {
            await client.DeleteAsync(url);
        }
    }
}

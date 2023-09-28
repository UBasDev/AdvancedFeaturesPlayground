using AuthorService.Application.Attributes;
using AuthorService.Application.Contexts;
using AuthorService.Application.Enums;
using AuthorService.Application.Interfaces.Redis;
using AuthorService.Application.Models;
using AuthorService.Domain.Entities;
using Grpc.Net.Client;
using IdentityModel;
using Microservice1.protos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

namespace AuthorService.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _dbContext;
        private readonly IRedisCacheRepository1 _redisCacheRepository;
        private readonly HttpClient _httpClient;
        public AuthorController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext, IRedisCacheRepository1 redisCacheRepository, HttpClient httpClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _redisCacheRepository = redisCacheRepository;
            _httpClient = httpClient;
        }
        private RequestedUser RequestedUser
        {
            get
            {
                var idClaims = from c in User.Claims.Where(f => f.Type == JwtClaimTypes.Id) select new { c.Value };
                var roleClaims = from c in User.Claims.Where(f => f.Type == JwtClaimTypes.Role) select new { c.Value };
                var preferredUsernameClaims = from c in User.Claims.Where(f => f.Type == JwtClaimTypes.PreferredUserName) select new { c.Value };
                var fullnameClaims = from c in User.Claims.Where(f => f.Type == "fullname") select new { c.Value };
                return new RequestedUser
                {
                    Id = idClaims.FirstOrDefault()!.Value,
                    PreferredUsername = preferredUsernameClaims.FirstOrDefault()!.Value,
                    Role = (AuthRole)Enum.Parse(typeof(AuthRole), roleClaims.FirstOrDefault()!.Value, true),
                    FullName = fullnameClaims.FirstOrDefault()!.Value,
                };
            }
        }
        public static List<Author> authors = new List<Author>()
        {
            new()
            {
                Id = 1,
                AuthorName = "author1",
                Age= 11
            },
            new()
            {
                Id = 2,
                AuthorName = "author2",
                Age= 22
            },
            new()
            {
                Id = 3,
                AuthorName = "author3",
                Age= 33
            }
        };
        [HttpGet("[action]")]
        public IActionResult GetToken()
        {
            return Ok(TokenGenerator());
        }
        private string TokenGenerator() //Bu metod sayesinde token generate ederiz.
        {
            var tokenExpireDate = new TimeSpan(180, 0, 0, 0, 0);
            var claims = new List<Claim>
        {
            new(JwtClaimTypes.Id, "213129"),
            new(JwtClaimTypes.PreferredUserName, "Preferred1 Ahmet1"),
            new("fullname", "Ahmet1"),
            new(JwtClaimTypes.Role, "CEO"),
        };
            var token = GetJwtToken1(tokenExpireDate, claims);
            return token;
        }
        private string GetJwtToken1(TimeSpan expiration, IEnumerable<Claim> claims = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("nsaklnxuskaxnukassaxsaxasxasxasxasjkuasjkdajs")); //Tokenının security keyini belirttik.

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7033",

           audience: "https://localhost:4200",
        expires: DateTime.UtcNow.Add(expiration),

                claims: claims,

                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return tokenHandler.WriteToken(token);
        }

        [HttpGet("[action]")]
        [RoleAuthorizeAttribute1(AuthRole.CEO, AuthRole.OC)]
        public IActionResult Test1()
        {
            var x = RequestedUser;
            return Ok();
        }
        [HttpGet("[action]")]
        [FullnameAuthorizeAttribute1("Ahmet1", "Mehmet1")]
        public IActionResult Test2()
        {
            return Ok();
        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult Test3()
        {
            return Ok();
        }
        public class GetAllAuthorsDto
        {
            public string AuthorName { get; set; }
            public int Age { get; set; }
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test4()
        {
            var response = new List<GetAllAuthorsDto>();
            var connection = _dbContext.Database.GetDbConnection();
            await using (var command = _dbContext.Authors.Where(a => a.Id > 1).CreateDbCommand())
            {
                command.CommandTimeout = 300;
                await connection.OpenAsync();
                await using (var currentRow = await command.ExecuteReaderAsync())
                {
                    while (await currentRow.ReadAsync())
                    {
                        var currentAuthorName = String.IsNullOrEmpty(currentRow["AuthorName"].ToString()) ? "" : currentRow["AuthorName"].ToString();
                        var currentAuthorAge = String.IsNullOrEmpty(currentRow["Age"].ToString()) ? 0 : Convert.ToInt32(currentRow["Age"]);
                        var currentAuthor = new GetAllAuthorsDto()
                        {
                            AuthorName = currentAuthorName,
                            Age = currentAuthorAge
                        };
                        response.Add(currentAuthor);
                    }
                }
                await connection.CloseAsync();
            }
            var x1 = "sad";
            return Ok(response);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test5()
        {
            var authorsToCreate = new List<Author>() {
                new()
                {
                    Age = 10,
                    AuthorName = "Author1"
                },
                new()
                {
                    Age = 20,
                    AuthorName = "Author2"
                },
                new()
                {
                    Age = 30,
                    AuthorName = "Author3"
                },
                new()
                {
                    Age = 40,
                    AuthorName = "Author4"
                }
            };
            var authorToCreate = new Author();

            await _dbContext.Authors.AddRangeAsync(authorsToCreate);
            bool bool1 = await _dbContext.Authors.AllAsync(a => a.Id > 1);
            bool bool2 = await _dbContext.Authors.AnyAsync(a => a.Id == 1);
            var x1 = _dbContext.Authors.AverageAsync(x => x.Age);
            int x3 = await _dbContext.Authors.CountAsync();
            //var x4 = await _dbContext.Authors.Distinct();
            Author x5 = _dbContext.Authors.Where(x => x.Id > 10).ElementAt(3);
            Author? x6 = _dbContext.Authors.ElementAtOrDefault(3);

            IQueryable query1 = _dbContext.Authors.Where(a => a.Id > 2);
            var x7 = _dbContext.Authors.Entry(x5).State; //Böylece ChangeTracker tarafından takip edilen bu `author1` objectinin mevcut stateini döndürür.
            int x8 = _dbContext.Authors.Entry(x5).OriginalValues.GetValue<int>(nameof(Author.Age));
            int x9 = _dbContext.Authors.Entry(x5).CurrentValues.GetValue<int>(nameof(Author.Age));
            IEnumerable<EntityEntry<Author>> x13 = _dbContext.ChangeTracker.Entries<Author>();
            List<EntityEntry<Author>> x14 = x13.ToList();

            Author[] x20 = await _dbContext.Authors.ToArrayAsync();

            PropertyValues? x10 = await _dbContext.Authors.Entry(x5).GetDatabaseValuesAsync();
            int x11 = x10.GetValue<int>(nameof(Author.Age));
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test6()
        {
            var list1 = new List<string>() { "Author1", "Ahmet", "Ahmet", "Ahmet", "Mehmet", "Mehmet", "Mehmet", "Ziya", "Ziya" };

            var x1 = await _dbContext.Authors.Select(a => $"{a.AuthorName} - {a.Age}").ToListAsync();
            var x2 = list1.Distinct().ToList();
            var x3 = await _dbContext.Authors.Select(x => new
            {
                NewKey1 = x.AuthorName,
                NewKey2 = x.Age
            }).ToListAsync();
            var x21 = await _dbContext.Authors.Select(a => $"{a.AuthorName} - {a.Age}").ToListAsync();
            var x22 = await _dbContext.Authors.Where(a => list1.Contains(a.AuthorName)).ToListAsync();
            var x23 = await _dbContext.Authors.Select(x => x.AuthorName).Distinct().ToListAsync();
            var x24 = await _dbContext.Authors.ToListAsync();
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test7()
        {
            using var channel1 = GrpcChannel.ForAddress("http://localhost:5235");
            var client1 = new AuthorServiceApi.AuthorServiceApiClient(channel1);
            var response1 = await client1.GetAllAuthorsByMinAgeGrpcServiceAsync(new GetAllAuthorsByMinAgeRequest() { AuthorAge = 33 });
            return Ok(response1);
        }
        [HttpGet("[action]")]
        [CustomAttributeAsync1(property1: 21, property2: "Test1", property3: new string[] { "Ahmet1", "Mehmet1", "Fatma1" })]
        public async Task<IActionResult> Test8()
        {
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test9()
        {
            await _redisCacheRepository.SetStringResponseAsync("key1", "This is just a string", TimeSpan.FromSeconds(10));
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test10()
        {
            var someAuthors1 = await _dbContext.Authors.ToListAsync();
            var serializedResults1 = JsonConvert.SerializeObject(someAuthors1);
            await _redisCacheRepository.SetStringResponseAsync("key1", serializedResults1, TimeSpan.FromSeconds(10));
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test11()
        {
            var cachedResponse1 = await _redisCacheRepository.GetCachedStringResponseByKeyAsync("key1");
            return Ok(cachedResponse1);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test12()
        {
            var cachedResponse1 = await _redisCacheRepository.GetCachedResponseByKeyAsync<List<Author>>("key1");
            return Ok(cachedResponse1);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test13()
        {
            await _redisCacheRepository.RemoveWithMultipleWildCardsAsync(new string[] { "ke" });
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test14()
        {
            var videoFilePath1 = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\", "Core", "AuthorService.Application", "StaticFiles", "video1.mp4");
            var fileStreamOptions1 = new FileStreamOptions
            {
                BufferSize = 8192,
                Mode = FileMode.Open,
                Access = FileAccess.Read,
                Options = FileOptions.Asynchronous,
                Share = FileShare.Read
            };
            return PhysicalFile(
                videoFilePath1, //Fİleın pathını set ederiz.

                "video/mp4", //Responseun Content-Type`ını set ederiz.

                lastModified: DateTimeOffset.Now, //Fileın `lastModified` propertysini set ederiz.

                enableRangeProcessing: true, //???

                entityTag: Microsoft.Net.Http.Headers.EntityTagHeaderValue.Any //Fileın e-tag`ının herhangi bir value olacağını set ederiz.
            );
        }
        public class Class1
        {
            public int UserId { get; set; }
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test15(CancellationToken cancellationToken)
        {
            var videoFilePath1 = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\", "Core", "AuthorService.Application", "StaticFiles", "video1.mp4");


            var content = new ContentResult()
            {
                Content = "sadas",
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.OK
            };
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            var objectToSend1 = new Class1
            {
                UserId = 1,
                Id = 2,
                Title = "title1",
                Body = "body1"
            };
            var content2 = new StringContent(JsonConvert.SerializeObject(objectToSend1), encoding: Encoding.UTF8, mediaType: "application/json");

            var response1 = await _httpClient.PostAsync("https://jsonplaceholder.typicode.com/posts", content2, cancellationToken);
            if (!response1.IsSuccessStatusCode)
            {
                Console.WriteLine("Something wrong");
            }
            var x1 = await response1.RequestMessage.Content.ReadAsStringAsync();
            var x2 = await response1.RequestMessage.Content.ReadFromJsonAsync<Class1>();
            var x3 = response1.RequestMessage.Headers.FirstOrDefault(x => x.Key == "header1");
            var x4 = response1.RequestMessage.Method;
            var x5 = response1.RequestMessage.Options;
            var x6 = response1.RequestMessage.Properties;
            var x7 = response1.RequestMessage.RequestUri;
            var x8 = response1.RequestMessage.Version;
            var x9 = response1.RequestMessage.VersionPolicy;

            string responseBody1 = await response1.Content.ReadAsStringAsync(cancellationToken);
            Class1? responseBody2 = await response1.Content.ReadFromJsonAsync<Class1>(new System.Text.Json.JsonSerializerOptions()
            {

            }, cancellationToken);
            return Ok(responseBody1);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test16(CancellationToken cancellationToken)
        {
            var result1 = new ContentResult()
            {
                Content = "asd",
                ContentType = MediaTypeNames.Application.Json,
                StatusCode = (int)HttpStatusCode.Created
            };
            return result1;
        }
        [HttpGet("[action]")]
        public async Task Test17(CancellationToken cancellationToken, [FromQuery] string query1)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
            string x1 = HttpContext.Response.Headers.FirstOrDefault(x => x.Key == "header1").Value;
            HttpContext.Response.Headers.Add("header1", "value1");

            var contextFromIOC1 = HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>(); //Gelen requestin bulunduğu IOC içerisindeki aktif servislerden herhangi birisine erişmemizi sağlar.
            var x2 = await contextFromIOC1.Authors.ToListAsync();

            await HttpContext.Response.WriteAsJsonAsync(new
            {
                Property1 = "value1",
                Property2 = "value2",
            }, cancellationToken);
            var x3 = HttpContext.Request.Method;
            string x4 = HttpContext.Request.PathBase;
            string x5 = HttpContext.Request.Path;
            var x6 = HttpContext.Request.Query.FirstOrDefault(q => q.Key == "query1").Value;
            var x8 = Environment.MachineName;
            string x9 = Environment.UserName;
            var x33 = Environment.OSVersion;
            string x10 = Environment.OSVersion.VersionString;
            string x11 = Environment.OSVersion.ServicePack;
            string x12 = Environment.CurrentDirectory;
            var x13 = Environment.CurrentManagedThreadId;
            var x14 = Environment.ProcessId;
            var x15 = Environment.ProcessPath;
            var x16 = Environment.ProcessorCount;
            var x17 = Environment.Is64BitOperatingSystem;
            var x18 = Environment.Is64BitProcess;
            var x19 = Environment.UserDomainName;
            var x20 = Environment.UserInteractive;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Test18([FromQuery] string requestBody)
        {
            var allClaims1 = ParseClaimsFromJwt(requestBody);


            var fullname1 = allClaims1.FirstOrDefault(t => t.Type == "fullname")?.Value;

            var role1 = allClaims1.FirstOrDefault(t => t.Type == JwtClaimTypes.Role)?.Value;

            return Ok(allClaims1);
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test19()
        {
            var x1 = new DateTime();
            var x2 = new DateTime(2023, 5, 15, 18, 25, 45, 880, kind: DateTimeKind.Utc);
            var x3 = new DateTime(2023, 5, 15, 18, 25, 45, 880, kind: DateTimeKind.Local);
            var x4 = DateTime.Now;
            var x5 = DateTime.UtcNow;
            var x6 = DateTime.MaxValue;
            var x7 = DateTime.MinValue;
            var x13 = DateTime.UnixEpoch;
            var x14 = DateTime.Today;
            var x8 = DateTime.Parse(s: x2.ToString("dd/MM/yyyy HH:dd:ss"), provider: new CultureInfo(name: "tr-TR", useUserOverride: true), styles: DateTimeStyles.None);
            var x81 = DateTime.Parse(x2.ToString("dd/MM/yyyy HH:dd:ss"), provider: CultureInfo.GetCultureInfo("tr-TR"), styles: DateTimeStyles.None);
            var x9 = DateTime.Compare(x2, x3);
            var x10 = DateTime.DaysInMonth(2023, 10);
            var x11 = DateTime.IsLeapYear(2022);
            var x12 = DateTime.ParseExact(x2.ToString("dd/MM/yyyy HH:dd:ss"), format: "dd/MM/yyyy HH:dd:ss", provider: new CultureInfo("tr-TR", useUserOverride: true), style: DateTimeStyles.None);
            var x121 = DateTime.ParseExact(x2.ToString("dd/MM/yyyy HH:dd:ss"), format: "dd/MM/yyyy HH:dd:ss", provider: CultureInfo.GetCultureInfo("tr-TR"), style: DateTimeStyles.None);
            var x15 = DateTime.SpecifyKind(x2, DateTimeKind.Utc);
            //var x16 = DateTime.Parse(x2.ToString("dd/MM/yyyy HH:dd:ss"), provider: CultureInfo.InvariantCulture, styles: DateTimeStyles.None);
            var x17 = DateTime.Parse(x2.ToString("dd/MM/yyyy HH:dd:ss"), provider: CultureInfo.CurrentCulture, styles: DateTimeStyles.None);
            var x18 = DateTime.Parse(x2.ToString("dd/MM/yyyy HH:dd:ss"), provider: CultureInfo.GetCultureInfo("tr-TR"), styles: DateTimeStyles.None);
            var x19 = DateTime.Parse(x2.ToString("dd/MM/yyyy HH:dd:ss"), provider: CultureInfo.CurrentUICulture, styles: DateTimeStyles.None);
            var x20 = DateTime.Parse(x2.ToString("dd/MM/yyyy HH:dd:ss"), provider: CultureInfo.DefaultThreadCurrentCulture, styles: DateTimeStyles.None);
            var x21 = DateTime.Parse(x2.ToString("dd/MM/yyyy HH:dd:ss"), provider: CultureInfo.DefaultThreadCurrentUICulture, styles: DateTimeStyles.None);
            var x22 = DateTime.Parse(x2.ToString("dd/MM/yyyy HH:dd:ss"), provider: CultureInfo.InstalledUICulture, styles: DateTimeStyles.None);
            var x23 = DateTime.Parse(x2.ToString("dd/MM/yyyy HH:dd:ss"), provider: CultureInfo.DefaultThreadCurrentUICulture, styles: DateTimeStyles.None);

            var x24 = DateTime.Now;
            var x25 = x24.AddMinutes(1);
            var x26 = x24.AddHours(1);
            var x27 = x24.Minute;
            var x28 = x24.AddDays(1);
            var x29 = x24.Hour;
            var x30 = x24.Day;
            var x31 = x24.Month;
            var x32 = x24.Year;
            var x33 = x24.DayOfWeek;
            var x34 = x24.DayOfYear;
            var x35 = x24.Add(new TimeSpan(2, 6, 25, 45, 880));
            var x36 = x24.Add(TimeSpan.FromSeconds(1));
            var x37 = x24.AddMilliseconds(800);
            var x38 = x24.AddMonths(1);
            var x39 = x24.AddSeconds(1);
            var x40 = x24.AddTicks(100);
            var x41 = x24.AddYears(1);
            var x42 = x24.Subtract(TimeSpan.FromMinutes(1));
            var x422 = x24.Subtract(new TimeSpan());
            var x43 = x24.Subtract(DateTime.Now);
            var x44 = x24.CompareTo(DateTime.Now);
            var x45 = x24.Date;
            var x46 = x24.TimeOfDay;
            var x47 = x24.GetDateTimeFormats();
            var x48 = x24.IsDaylightSavingTime();
            var x49 = x24.Kind;
            var x50 = x24.ToString("dd/MM/yyyy HH:dd:ss");
            var x51 = x24.Second;
            var x52 = x24.Ticks;
            var x53 = x24.ToBinary();
            var x54 = x24.Millisecond;
            var x55 = x24.ToEpochTime();
            var x56 = x24.ToFileTime();
            var x57 = x24.ToFileTimeUtc();
            var x58 = x24.ToLocalTime();
            var x59 = x24.ToLongDateString();
            var x60 = x24.ToLongTimeString();
            var x61 = x24.ToOADate();
            var x62 = x24.ToShortDateString();
            var x63 = x24.ToShortTimeString();
            var x64 = x24.ToUniversalTime();
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Test20()
        {

            var x1 = TimeSpan.Zero;
            var x2 = TimeSpan.FromSeconds(1);
            var x3 = TimeSpan.FromMinutes(1);
            var x4 = TimeSpan.FromHours(1);
            var x5 = TimeSpan.FromDays(1);
            var x6 = TimeSpan.FromTicks(1);
            var x7 = TimeSpan.Compare(x1, x2);
            var x8 = TimeSpan.FromMilliseconds(800);
            var x9 = TimeSpan.MaxValue;
            var x10 = TimeSpan.MinValue;
            var x11 = TimeSpan.Parse(input: "3", formatProvider: CultureInfo.GetCultureInfo("tr-TR"));
            var x12 = TimeSpan.TicksPerDay;
            var x13 = TimeSpan.TicksPerSecond;
            var x14 = TimeSpan.TicksPerHour;
            var x15 = TimeSpan.TicksPerMillisecond;
            var x16 = TimeSpan.TicksPerMinute;

            var x17 = new TimeSpan(1);

            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Immutable;
using System.Globalization;

namespace BookService.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult Test1()
        {
            return Ok("Book Controller works!");
        }
        [HttpGet("[action]")]
        public IActionResult Test2()
        {
            var x1 = 123456.78912;
            Console.WriteLine($"Formatted valueN1: {x1:N1}");
            Console.WriteLine($"Formatted valueN2: {x1:N2}");
            Console.WriteLine($"Formatted valueN3: {x1:N3}");
            Console.WriteLine($"Formatted valueN4: {x1:N4}");
            Console.WriteLine($"Formatted valueC1: {x1:C1}");
            Console.WriteLine($"Formatted valueC2: {x1:C2}");
            Console.WriteLine($"Formatted valueC3: {x1:C3}");
            var x2 = .34567;
            Console.WriteLine($"Formatted valueP1: {x2:P1}");
            Console.WriteLine($"Formatted valueP2: {x2:P2}");
            Console.WriteLine($"Formatted valueP3: {x2:P8}");
            var x3 = $"Formatted valueP3: {x2:P8}";
            string x4 = "Text1";

            var list1 = new List<int>() { 1, 2, 3, 4, 5 };

            int x5 = 5;
            var x6 = x5.CompareTo(6);
            var x7 = x5.ToString(format: "D5", new CultureInfo("tr-TR"));
            var x8 = x5.ToString(format: "00000", new CultureInfo("tr-TR"));

            string x9 = string.Join(",", new string[] { "Ahmet", "Selin", "Mehmet" });
            string x10 = string.Join(",", new List<string> { "Ahmet", "Selin", "Mehmet" });
            int x11 = string.Compare("AHmet", "Selin", ignoreCase: true, culture: new CultureInfo("tr-TR"));
            string x12 = string.Concat("Ahmet1", "Ahmet2", "Ahmet3");
            string x13 = string.Concat(new string[] { "Ahmet1", "Ahmet2" });
            string x14 = string.Concat(new List<string> { "Ahmet1", "Ahmet2" });
            string x15 = string.Empty;
            string x16 = string.Format(provider: new CultureInfo("tr-TR"), format: "Hello {0}", "Ahmet1");
            bool x17 = string.IsNullOrEmpty("Ahmet1");
            bool x18 = string.IsNullOrWhiteSpace("Ahmet1");

            string[] x19 = x9.Split(separator: " ", options: StringSplitOptions.TrimEntries);
            bool x20 = x9.StartsWith("test", ignoreCase: true, culture: new CultureInfo("tr-TR"));
            bool x21 = x9.EndsWith("test", ignoreCase: true, culture: new CultureInfo("tr-TR"));
            bool x22 = x9.Contains("test", comparisonType: StringComparison.CurrentCulture);
            int x23 = x9.IndexOf(value: "a", startIndex: 0, count: 5, comparisonType: StringComparison.CurrentCulture);
            string x24 = x9.Substring(startIndex: 0, length: 5);

            string x25 = x9.Replace(oldChar: 'a', newChar: 'b');
            string x26 = x9.Replace(oldValue: "ahmet1", newValue: "ahmet11", ignoreCase: true, culture: new CultureInfo("tr-TR"));
            string[] x27 = { "Ahmet1", "Ahmet2" };

            var x30 = "Ahmet1";
            bool x31 = x30.All((x) => x != 'b');
            bool x32 = x30.Any((x) => x != 'a');
            IEnumerable<char> x33 = x30.Append('x');
            IEnumerable<char> x34 = x30.AsEnumerable<char>();
            ReadOnlyMemory<char> x35 = x30.AsMemory(start: 0, length: 3);
            IQueryable<char> x36 = x30.AsQueryable().Where(x => x != ' ');
            ReadOnlySpan<char> x37 = x30.AsSpan(start: 0, length: 3);
            IEnumerable<char> x38 = x30.Cast<char>();
            object x39 = x30.Clone();
            var x40 = x30.CompareTo(strB: "Mehmet1");
            var x41 = x30.Concat("sa");
            bool x42 = x30.Contains(value: 'a', comparisonType: StringComparison.OrdinalIgnoreCase);
            int x43 = x30.Count();
            char? x44 = x30.ElementAt(index: 3);
            char? x45 = x30.ElementAtOrDefault(index: 4);
            bool x46 = x30.EndsWith(value: "met1", ignoreCase: true, culture: new CultureInfo("tr-TR"));
            bool x47 = x30.EndsWith(value: 't');
            bool x48 = x30.Equals(value: "ahmet1", comparisonType: StringComparison.OrdinalIgnoreCase);
            IEnumerable<char> x49 = x30.Except("Mehmet1");
            char x50 = x30.First(predicate: (x) => x != 'a');
            char x51 = x30.FirstOrDefault(predicate: (x) => x == 'x');
            var x52 = x30.GetHashCode(comparisonType: StringComparison.OrdinalIgnoreCase);
            Type x53 = x30.GetType();
            var x54 = x30.GroupBy((x) => x).Select((x) => new
            {
                Total = x.Count(),
                Letter = x
            }).ToList();
            string x55 = x30.Insert(startIndex: 0, value: "Mehmet1");
            char x56 = x30.Last(predicate: (x) => x != 'a');
            char x57 = x30.Max();
            char x58 = x30.Min();
            IOrderedEnumerable<char> x59 = x30.Order();
            IEnumerable<char> x60 = x30.Prepend(element: 'a');
            string x61 = x30.Remove(startIndex: 0, count: 1);
            IEnumerable<char> x62 = x30.Reverse();
            bool x63 = x30.SequenceEqual(second: "");


            return Ok(x3);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> Test3()
        {
            var x30 = "AhAmeA1m";
            var newst1 = new char[] { 's', 'e', 'l', 'i', 'n', '2' };
            x30.CopyTo(sourceIndex: 0, destination: newst1, count: 4, destinationIndex: 2);
            IEnumerable<char> x31 = x30.DistinctBy(keySelector: (x) => x);
            IEnumerable<char> x32 = x30.DistinctBy(keySelector: (x) => x == 'A');
            IEnumerable<char> x33 = x30.DistinctBy(keySelector: (x) => x == 'm');
            char x34 = x30.MaxBy(keySelector: (x) => x);
            char x35 = x30.MaxBy(keySelector: (x) => x == 'A');
            char x36 = x30.MaxBy(keySelector: (x) => x == 'm');
            string x37 = x30.PadLeft(totalWidth: 15, paddingChar: 'A');
            string x38 = x30.PadLeft(totalWidth: 15, paddingChar: '\t');
            string x39 = x30.PadLeft(totalWidth: 15, paddingChar: '\n');
            IEnumerable<char> x63 = x30.Select(x => x);
            IEnumerable<object> x64 = x30.Select(selector: (x) => new
            {
                Key1 = x
            });
            var x65 = x30.Single(predicate: (x) => x == 'h');
            var x66 = x30.Single(predicate: (x) => x == '1');
            var x67 = x30.SingleOrDefault(predicate: (x) => x == 'P', defaultValue: 'X');
            IEnumerable<char> x68 = x30.Skip(count: 5);
            IEnumerable<char> x69 = x30.SkipLast(count: 5);
            IEnumerable<char> x70 = x30.SkipWhile(predicate: (x) => x != 'm');
            int x71 = x30.Sum(selector: (x) => x);
            IEnumerable<char> x72 = x30.Take(count: 5);
            IEnumerable<char> x73 = x30.TakeLast(count: 5);
            IEnumerable<char> x74 = x30.TakeWhile(predicate: (x) => x != 'm');
            char[] x75 = x30.ToArray();
            char[] x76 = x30.ToCharArray(startIndex: 0, length: 5);
            //Dictionary<string, char> x77 = x30.ToDictionary(keySelector: (x) => "Key1" + x.ToString(), elementSelector: (x) => x);
            HashSet<char> x78 = x30.ToHashSet();

            List<char> x79 = x30.ToList();
            ILookup<char, char> x80 = x30.ToLookup(keySelector: (x) => x, elementSelector: (x) => x);
            ImmutableArray<char> x81 = x30.ToImmutableArray();
            ImmutableDictionary<string, char> x82 = x30.ToImmutableDictionary(keySelector: (x) => "Key1" + x.ToString(), elementSelector: (x) => x);
            ImmutableHashSet<char> x83 = x30.ToImmutableHashSet();
            ImmutableList<char> x84 = x30.ToImmutableList();
            ImmutableSortedDictionary<string, char> x85 = x30.ToImmutableSortedDictionary(keySelector: (x) => "Key1" + x.ToString(), elementSelector: (x) => x);
            ImmutableSortedSet<char> x86 = x30.ToImmutableSortedSet();
            var x87 = x30.Trim(trimChars: new char[] { 'a', 'b' });
            string x88 = x30.ToString(provider: new CultureInfo("tr-TR"));
            IEnumerable<char> x89 = x30.Union(second: "test1");
            IEnumerable<char> x90 = x30.Union(second: new char[] { 't', 'e', 's', 't', '1' });
            var x91 = x30.UnionBy(second: "test1", keySelector: (x) => x);
            var x92 = x30.UnionBy(second: "test1", keySelector: (x) => x == 'A');
            IEnumerable<char> x93 = x30.Where(predicate: (x) => x != 'A');
            IEnumerable<(char, char)> x94 = x30.Zip(second: "Test1");
            IEnumerable<(char, char)> x95 = x30.Zip(second: "TestTestTest1");
            IEnumerable<(char, char, char)> x96 = x30.Zip(second: "Test1", third: "Test2");

            return Ok();
        }
        public class Class1
        {
            public string Key1 { get; set; }
        }
    }
}

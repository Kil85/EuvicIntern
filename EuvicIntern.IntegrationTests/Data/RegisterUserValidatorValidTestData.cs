using EuvicIntern.Models;
using Newtonsoft.Json;
using System.Collections;

namespace EuvicIntern.IntegrationTests.Data
{
    public class RegisterUserValidatorValidTestData : IEnumerable<object[]>
    {
        private readonly string FilePath = "Data/RegisterUserValidatorValidData.json";

        public IEnumerator<object[]> GetEnumerator()
        {
            var pojectPath = Directory.GetCurrentDirectory();
            var JsonFullPath = Path.Combine(pojectPath, FilePath);

            if (!File.Exists(JsonFullPath))
            {
                throw new FileNotFoundException(JsonFullPath);
            }

            var jsonData = File.ReadAllText(JsonFullPath);
            var deserializedJsonData = JsonConvert.DeserializeObject<RegisterUserDto[]>(jsonData);
            var data = new List<object[]>();
            foreach (var user in deserializedJsonData)
            {
                data.Add(new object[] { user });
            }
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShockHell {
  public static class SimpleHttpClient {
    private static readonly HttpClient client = new HttpClient();

    public static async Task<string> GetAsync(string url) {
      HttpResponseMessage response = await client.GetAsync(url);
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadAsStringAsync();
    }

    public static string Get(string url) {
      HttpResponseMessage response = client.GetAsync(url).Result;
      return response.Content.ReadAsStringAsync().Result;
    }

    public static async Task<string> PostAsync(string url, string jsonPayload) {
      var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await client.PostAsync(url, content);
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadAsStringAsync();
    }

    public static string Post(string url, string jsonPayload) {
      var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
      HttpResponseMessage response = client.PostAsync(url, content).Result;
      return response.Content.ReadAsStringAsync().Result;
    }
  }
}

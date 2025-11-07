using System.Text.Json;
using OvulaeShared.Models.WebApi;

namespace OvulaeDashboard.Helpers
{
    public class APIResponseHelper
    {
        public static string GetMessageFromResponse(GenericResult result)
        {
            try
            {
                string jsonPart = result.Message.Substring(result.Message.IndexOf('{'));

                var jsonDoc = JsonDocument.Parse(jsonPart);
                string actualMessage = jsonDoc.RootElement.GetProperty("message").GetString();

                return actualMessage ?? "An error occurred.";
            }
            catch
            {
                return "An erro occured";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace MicrosoftGraphDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GraphServiceClient graphClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
            {
                var accessToken = "<YOUR_ACCESS_TOKEN_HERE>";
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                return Task.FromResult(0);
            }));

            await ListDriveItems(graphClient.Me.Drive.Root);

            Console.ReadLine();
        }

        private static async Task ListDriveItems(IDriveItemRequestBuilder folder)
        {
            var driveItems = await folder.Children.Request().GetAsync();

            foreach (var item in driveItems)
            {
                if (item.Folder != null)
                {
                    await ListDriveItems(folder.Child(item.Name));
                }
                else
                {
                    Console.WriteLine($"File: {item.Name}, Size: {item.Size}, ID: {item.Id}");
                }
            }
        }
    }
}

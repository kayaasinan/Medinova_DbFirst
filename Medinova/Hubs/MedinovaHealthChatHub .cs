using Medinova.Services.AI;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Medinova.Hubs
{
    public class MedinovaHealthChatHub : Hub
    {
        public async Task SendMessage(string userMessage)
        {
            var aiChatService =
                DependencyResolver.Current.GetService<MedinovaAiChatService>();

            await Clients.Caller.receiveMessage("Hasta", userMessage);

            var aiResult =
                await aiChatService.GetDepartmentSuggestionAsync(userMessage);

            await Clients.Caller.receiveAiResult(aiResult);
        }
    }

}

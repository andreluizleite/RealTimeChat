using Microsoft.AspNetCore.SignalR;

  public class ChatHub : Hub
    {
        private static List<string> _messages = new List<string>();

        public async Task SendMessage(string user, string message)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            var formattedMessage = $"{timestamp} - {user}: {message}";

            _messages.Add(formattedMessage);
            await Clients.All.SendAsync("ReceiveMessage", formattedMessage);
        }

        public async Task<IEnumerable<string>> GetChatHistory()
        {
            return _messages;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "Chat Bot", "Welcome to the chat!");
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
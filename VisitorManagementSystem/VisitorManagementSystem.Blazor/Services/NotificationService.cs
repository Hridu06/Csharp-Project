using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace VisitorManagementSystem.Blazor.Services
{
    public class NotificationService : IAsyncDisposable
    {
        private HubConnection? _hubConnection;
        private int _employeeId;

        // Event to notify UI when a visitor requests a visit
        public event Action<string>? OnVisitorAdded;

        // ----------------------
        // Initialize SignalR connection for the employee
        // ----------------------
        public async Task InitializeAsync(int employeeId)
        {
            _employeeId = employeeId;

            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
                return;

            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7005/visitorhub") // adjust to your backend URL
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string>("ReceiveVisitorNotification", visitorName =>
            {
                OnVisitorAdded?.Invoke(visitorName);
            });

            _hubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(1000, 5000));
                try
                {
                    await _hubConnection.StartAsync();
                    await JoinEmployeeGroup();
                }
                catch
                {
                    // optionally log the error
                }
            };

            await _hubConnection.StartAsync();
            await JoinEmployeeGroup();
        }

        private async Task JoinEmployeeGroup()
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.SendAsync("JoinGroup", $"Employee_{_employeeId}");
            }
        }

        public async Task NotifyEmployeeAsync(int employeeId, string visitorName)
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.SendAsync("NotifyEmployee", $"Employee_{employeeId}", visitorName);
            }
        }

        public async Task DisconnectAsync()
        {
            if (_hubConnection != null)
                await _hubConnection.StopAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
                await _hubConnection.DisposeAsync();
        }
    }
}

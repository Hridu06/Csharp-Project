using VisitorManagementSystem.Application.DTOs;
using VisitorManagementSystem.Application.Interfaces;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace VisitorManagementSystem.Blazor.Services
{
    public class VisitRequestService : IVisitRequestService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public VisitRequestService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        // ✅ Helper method to set JWT header
        private async Task SetAuthHeaderAsync()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // ✅ Get all visit requests for a specific employee
        public async Task<List<VisitRequestDto>> GetRequestsForEmployeeAsync(int employeeId)
        {
            await SetAuthHeaderAsync();
            var result = await _http.GetFromJsonAsync<List<VisitRequestDto>>($"api/visitrequests/employee/{employeeId}");
            return result ?? new List<VisitRequestDto>();
        }

        // ✅ Create a new visit request (visitor -> employee)
        public async Task<VisitRequestDto?> CreateVisitRequestAsync(VisitRequestDto request)
        {
            await SetAuthHeaderAsync();
            var response = await _http.PostAsJsonAsync("api/visitrequests", request);

            if (response.IsSuccessStatusCode)
            {
                // Optionally, read the created request
                var createdRequest = await response.Content.ReadFromJsonAsync<VisitRequestDto>();
                return createdRequest;
            }

            return null;
        }

        // ✅ Approve request by employee
        public async Task<bool> ApproveRequest(int requestId)
        {
            await SetAuthHeaderAsync();
            var response = await _http.PutAsync($"api/visitrequests/approve/{requestId}", null);
            return response.IsSuccessStatusCode;
        }

        // ✅ Reject request by employee
        public async Task<bool> RejectRequest(int requestId)
        {
            await SetAuthHeaderAsync();
            var response = await _http.PutAsync($"api/visitrequests/reject/{requestId}", null);
            return response.IsSuccessStatusCode;
        }
    }
}

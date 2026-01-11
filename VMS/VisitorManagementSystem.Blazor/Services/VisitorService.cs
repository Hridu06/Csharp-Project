using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using VisitorManagementSystem.Application.Interfaces;
using VisitorManagementSystem.Application.DTOs;
using Microsoft.JSInterop;

namespace VisitorManagementSystem.Blazor.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public VisitorService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        private async Task AddAuthorizationHeader()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
            _http.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // ✅ Get all visitors
        public async Task<List<VisitorDTO>> GetAllVisitorsAsync()
        {
            await AddAuthorizationHeader();
            return await _http.GetFromJsonAsync<List<VisitorDTO>>("api/visitor") ?? new List<VisitorDTO>();
        }

        // ✅ Get visitor by Id
        public async Task<VisitorDTO> GetVisitorByIdAsync(int id)
        {
            await AddAuthorizationHeader();
            return await _http.GetFromJsonAsync<VisitorDTO>($"api/visitor/{id}");
        }

        // ✅ Add new visitor
        public async Task<VisitorDTO> AddVisitorAsync(VisitorDTO visitorDto)
        {
            await AddAuthorizationHeader();
            var response = await _http.PostAsJsonAsync("api/visitor", visitorDto);
            return await response.Content.ReadFromJsonAsync<VisitorDTO>();
        }

        // ✅ Update visitor
        public async Task<bool> UpdateVisitorAsync(VisitorDTO visitorDto)
        {
            await AddAuthorizationHeader();
            var response = await _http.PutAsJsonAsync($"api/visitor/{visitorDto.Id}", visitorDto);
            return response.IsSuccessStatusCode;
        }

        // ✅ Delete visitor
        public async Task<bool> DeleteVisitorAsync(int id)
        {
            await AddAuthorizationHeader();
            var response = await _http.DeleteAsync($"api/visitor/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}

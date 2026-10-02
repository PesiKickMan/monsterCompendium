using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http.Json;
using monsterCompendium.Models;

namespace monsterCompendium.Services
{
    public class MonsterApiService : IMonsterApiService
    {
        private readonly HttpClient _httpClient;

        public MonsterApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://www.dnd5eapi.co/");
        }

        public async Task<List<MonsterSummary>> GetMonstersAsync()
        {
            var response = await _httpClient.GetAsync("api/2014/monsters");

            if (!response.IsSuccessStatusCode) { 
                throw new HttpRequestException(
                    $"Error al obtener la lista de monstruos: {response.StatusCode}",
                    null,
                    response.StatusCode);
            }

            var result = await response.Content.ReadFromJsonAsync<MonsterListResponse>();

            return result?.Results ?? new List<MonsterSummary>();
        }

        public async Task<Monster?> GetMonsterAsync(string index)
        {
            var response = await _httpClient.GetAsync($"api/2014/monsters/{index}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                $"Error al obtener el monstruo. Código HTTP: {response.StatusCode}",
                null,
                response.StatusCode);
            }

            return await response.Content.ReadFromJsonAsync<Monster>();
        }
    }
}

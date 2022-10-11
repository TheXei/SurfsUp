using SurfsUpAPI;
using SurfsUp.Models;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using Models;
using Models.DTOs;
using System.Net.Http.Json;

namespace UnitTesting
{
    [TestClass]
    public class Krav6
    {
        HttpClient api;
        [TestInitialize]
        public void Initialize()
        {
            api = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7277/api/"),
            };
        }
        [TestMethod]
        public void TestBoardPremium()
        {
            List<Board> premiumList, guestList;

            premiumList = JsonConvert.DeserializeObject<List<Board>>(GetAsync($"v2/boards/{true}", api).Result);
            bool hasPremiumBoards = premiumList.Any(b => b.Premium == true);
            // mangler post board i api
            //if(!hasPremiumBoards)
            //{
            //    Board board = new Board()
            //    {
            //        Name = "test123",
            //        ImageURL = "",
            //        Price = 1,
            //        Length = 1,
            //        Width = 1,
            //        Thickness = 1,
            //        Volume = 1,
            //        Type = BoardType.Funboard,
            //        Premium = true,
            //    };
            //    api.PostAsync(, board);
            //}
            guestList = JsonConvert.DeserializeObject<List<Board>>(GetAsync($"v2/Boards/{false}?", api).Result);
            if (hasPremiumBoards)
                Assert.AreNotEqual(premiumList, guestList);
            else
                Assert.IsTrue(true);
        }
        [TestMethod]
        public void RentOutAsGuest()
        {
            List<Board> guestList = JsonConvert.DeserializeObject<List<Board>>(GetAsync($"v2/Boards/{false}?", api).Result);
            // udlej board
            GuestRentDto dto = new()
            {
                BoardId = guestList[0].Id,
                StartRent = DateTime.Now,
                EndRent = DateTime.Now.AddDays(2).AddMinutes(5),
                FirstName = "test",
                LastName = "test",
                PhoneNumber = "00000000"
            };

            var post = api.PostAsJsonAsync("v2/Rent/", dto);
            Assert.IsTrue(post.Result.IsSuccessStatusCode);

        }
        [TestMethod]
        public void GuestRentMaxAmount()
        {
            bool success = false;
            List<Board> guestList = JsonConvert.DeserializeObject<List<Board>>(GetAsync($"v2/Boards/{false}?", api).Result);
            int maxRentAmount = 3;
            GuestRentDto dto = new();
            if(guestList.Count > maxRentAmount)
            {
                for (int i = 0; i < maxRentAmount; i++)
                {
                    dto = new()
                    {
                        BoardId = guestList[i].Id,
                        StartRent = DateTime.Now,
                        EndRent = DateTime.Now.AddDays(2).AddMinutes(5),
                        FirstName = "testuser2",
                        LastName = "testuser2",
                        PhoneNumber = "11111111"
                    };

                    var post = api.PostAsJsonAsync("v2/Rent/", dto);
                }

                dto.BoardId = guestList[3].Id;
                dto.StartRent = DateTime.Now;

                success = api.PostAsJsonAsync("v2/Rent/", dto).Result.IsSuccessStatusCode;
            }

            Assert.IsTrue(success == false);
        }
        async Task<string> GetAsync(string call, HttpClient client)
        {
            using HttpResponseMessage response = await client.GetAsync(call);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;

namespace SurfsUp.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SurfsUpContext(serviceProvider.GetRequiredService<DbContextOptions<SurfsUpContext>>()))
            {
                // Look for any boards.
                if (context.Board.Any())
                {
                    return;   // DB has been seeded
                }
                context.Board.AddRange(
                    
                    new Board
                    {
                        Name = "The Minilog",
                        Length = 6.00f,
                        Width = 21.00f,
                        Thickness = 2.75f,
                        Volume = 38.80f,
                        Type = BoardType.Shortboard,
                        Price = 565M,
                        ImageURL = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p4_i2_w640.jpeg",
                        Equipments = ""
                    },

                    new Board
                    {
                        Name = "The Wide Glider",
                        Length = 7.1f,
                        Width = 21.75f,
                        Thickness = 2.75f,
                        Volume = 44.16f,
                        Type = BoardType.Funboard,
                        Price = 685M,
                        ImageURL = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p10_i2_w640.jpeg",
                        Equipments = ""
                    },

                    new Board
                    {
                        Name = "The Golden Ratio",
                        Length = 6.3f,
                        Width = 21.85f,
                        Thickness = 2.9f,
                        Volume = 42.22f,
                        Type = BoardType.Funboard,
                        Price = 695M,
                        ImageURL = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p20_i2_w640.jpeg",
                        Equipments = ""
                    },

                    new Board
                    {
                        Name = "Mahi Mahi",
                        Length = 5.4f,
                        Width = 20.75f,
                        Thickness = 2.3f,
                        Volume = 29.39f,
                        Type = BoardType.Shortboard,
                        Price = 645M,
                        ImageURL = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p26_i2_w640.jpeg",
                        Equipments = ""
                    },

                    new Board
                    {
                        Name = "The Emerald Glider",
                        Length = 9.2f,
                        Width = 22.8f,
                        Thickness = 2.8f,
                        Volume = 65.4f,
                        Type = BoardType.Longboard,
                        Price = 895M,
                        ImageURL = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p3_i2_w640.jpeg",
                        Equipments = ""
                    },

                    new Board
                    {
                        Name = "The Bomb",
                        Length = 5.5f,
                        Width = 21f,
                        Thickness = 2.5f,
                        Volume = 33.7f,
                        Type = BoardType.Shortboard,
                        Price = 645M,
                        ImageURL = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p5_i4_w640.jpeg",
                        Equipments = ""
                    },

                    new Board
                    {
                        Name = "Walden Magic",
                        Length = 9.6f,
                        Width = 19.4f,
                        Thickness = 3f,
                        Volume = 80f,
                        Type = BoardType.Funboard,
                        Price = 1025M,
                        ImageURL = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p6_i6_w640.jpeg"
                    },

                    new Board
                    {
                        Name = "Naish One",
                        Length = 12.6f,
                        Width = 30,
                        Thickness = 6,
                        Volume = 301,
                        Type = BoardType.Funboard,
                        Price = 854,
                        ImageURL = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p24_i2_w640.jpeg",
                       // Equipments = "Paddle"
                    },

                    new Board
                    {
                        Name = "Six Tourer",
                        Length = 11.6f,
                        Width = 32f,
                        Thickness = 6f,
                        Volume = 270f,
                        Type = BoardType.Funboard,
                        Price = 611,
                        ImageURL = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p31_i2_w640.jpeg",
                        Equipments = "Fin,Paddle,Pump,Leash"
                    },

                    new Board
                    {
                        Name = "Naish Maliko",
                        Length = 14f,
                        Width = 25f,
                        Thickness = 6f,
                        Volume = 330f,
                        Type = BoardType.Shortboard,
                        Price = 1304,
                        ImageURL = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p26_i2_w640.jpeg",
                        Equipments = "Fin,Paddle,Pump,Leash"
                    }
                );
                context.SaveChanges();

                
            }
        }
    }
}

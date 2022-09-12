using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using System.Data;

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
                        Price = 50f,
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
                        Price = 50f,
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
                        Price = 50f,
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
                        Price = 50f,
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
                        Price = 50f,
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
                        Price = 50f,
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
                        Price = 50f,
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
                        Price = 50f,
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
        /// <summary>
        /// It creates a user with the email "admin@surfsup.dk" and the password "secret" and assigns
        /// the role "Admin" to the user
        /// </summary>
        /// <param name="IServiceProvider">This is the service provider that is used to create the
        /// database context.</param>
        public static async Task InitializeUser(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<SurfsUpIdentityContext>();

            string[] roles = new string[] { StaticDetails.Role_User_Admin, StaticDetails.Role_User_Individual, StaticDetails.Role_User_Employee };

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    await roleStore.CreateAsync(new IdentityRole(role));
                }
            }

            var user = new ApplicationUser
            {
                //Id = "e0fc40d6-5290-495d-b978-bc06b6e668f9",
                UserName = "admin@surfsup.dk",
                NormalizedUserName = "ADMIN@SURFSUP.DK",
                Email = "admin@surfsup.dk",
                NormalizedEmail = "ADMIN@SURFSUP.DK",
                SecurityStamp = "YVS2T6AZ26RWAYSJ7FWQ6TT6FMZB736A",
                ConcurrencyStamp = "1d7397fb-2eaf-42c4-a6b4-822f30237181",
                PhoneNumber = "62731827",
                PhoneNumberConfirmed = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                EmailConfirmed = true,
                Name = "Admin",
                StreetAddress = "Admin Street 21",
                City = "Odense",
                State = "Syddanmark",
                PostalCode = "5000"
            };
            if (!context.ApplicationUsers.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "secret");
                user.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(context);
                var result = userStore.CreateAsync(user);

            }

            var assignResult = await AssignRoles(serviceProvider, user.Email, roles[0]);
            
            if (assignResult.Succeeded)
                await context.SaveChangesAsync();

        }
        /// <summary>
        /// This function takes in a user's email address and a role name, finds the user in the
        /// database, and assigns the role to the user
        /// </summary>
        /// <param name="IServiceProvider">This is the service provider that is used to get the
        /// UserManager<IdentityUser></param>
        /// <param name="id">The email address of the user</param>
        /// <param name="role">The role you want to assign to the user.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string id, string role)
        {
            UserManager<IdentityUser> _userManager = services.GetService<UserManager<IdentityUser>>();
            IdentityUser user = await _userManager.FindByEmailAsync(id);
            var result = await _userManager.AddToRoleAsync(user, role);

            return result;
        }
    }
}

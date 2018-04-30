using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DoorBash.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(DoorBashDbContext context)
        {
           // context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Items.Any())
            {
                return;
            }

            IList<Category> categories = new List<Category>
            {
                new Category
                {
                    Name = "Pizza",
                    Items = new List<Item> ()
                    {
                        new Item
                        {
                            Name = "Best Pizza",
                            Description = "The best pizza in the world!",
                            Hot = true,
                            Vegan = false,
                            Price = 500
                        },
                        new Item
                        {
                            Name = "Dominu Pizza",
                            Description = "Almost as popular as another pizza with a similiar name.",
                            Hot = true,
                            Vegan = true,
                            Price = 450
                        },
                        new Item
                        {
                            Name = "Nothin'Special Pizza",
                            Description = "It's \"just\" pizza.",
                            Hot = false,
                            Vegan = false,
                            Price = 230
                        },
                        new Item
                        {
                            Name = "Vegetarian Pizza",
                            Description = "No meat, no pineapples.",
                            Hot = false,
                            Vegan = true,
                            Price = 280
                        }
                    }
                },
                new Category
                {
                    Name = "Soup",
                    Items = new List<Item> ()
                    {
                        new Item
                        {
                            Name = "Bean Soup",
                            Description = "Your favorite beans all together for the best soup in the world.",
                            Hot = false,
                            Vegan = true,
                            Price = 450
                        },
                        new Item
                        {
                            Name = "S(o)up",
                            Description = "For the hip bois in town.",
                            Hot = true,
                            Vegan = false,
                            Price = 390
                        },
                        new Item
                        {
                            Name = "Nothin'Special Soup",
                            Description = "It's just soup.",
                            Hot = false,
                            Vegan = false,
                            Price = 230
                        },
                        new Item
                        {
                            Name = "Clothes",
                            Description = "WHY ARE YOU BUYING CLOTHES AT THE SOUP STORE",
                            Hot = true,
                            Vegan = true,
                            Price = 420
                        }
                    }
                },
                new Category
                {
                    Name = "Traditional",
                    Items = new List<Item> ()
                    {
                        new Item
                        {
                            Name = "Traditional Flatbread",
                            Description = "It's pizza, but different.",
                            Hot = false,
                            Vegan = false,
                            Price = 250
                        },
                        new Item
                        {
                            Name = "Traditional Bean Soup",
                            Description = "It's bean soup, but different.",
                            Hot = true,
                            Vegan = true,
                            Price = 250
                        },
                        new Item
                        {
                            Name = "Traditional Big Max",
                            Description = "It's Big Max, but different.",
                            Hot = false,
                            Vegan = false,
                            Price = 250
                        }
                    }
                },
                new Category
                {
                    Name = "Fast Food",
                    Items = new List<Item> ()
                    {
                        new Item
                        {
                            Name = "McCharles - Big Max",
                            Description = "Double mega meat, cheese burger.",
                            Hot = false,
                            Vegan = false,
                            Price = 400
                        },
                        new Item
                        {
                            Name = "McCharles - McFurry",
                            Description = "Probably wasn't ever \"best before\", but all time classic.",
                            Hot = false,
                            Vegan = true,
                            Price = 100
                        },
                        new Item
                        {
                            Name = "TFC - B Clever: Burger",
                            Description = "Clever choice for clever people. Now with extra secret ingredients!",
                            Hot = false,
                            Vegan = false,
                            Price = 280
                        },
                        new Item
                        {
                            Name = "TFC - B Clever: iSwirl",
                            Description = "Clever choice for clever people. Wrapped thingy with a lot of stuff!",
                            Hot = true,
                            Vegan = true,
                            Price = 280
                        },
                    }
                },
                new Category
                {
                    Name = "Bonus Meal",
                    Items = new List<Item> ()
                    {
                        new Item
                        {
                            Name = "Slippy T",
                            Description = "YES! IT'S TIME! BONUS MEAL!",
                            Hot = true,
                            Vegan = false,
                            Price = 399
                        }
                    }
                },
                new Category
                {
                    Name = "Drinks",
                    Items = new List<Item> ()
                    {
                        new Item
                        {
                            Name = "Best Drink in the World",
                            Description = "No! This is just a tribute.",
                            Hot = false,
                            Vegan = false,
                            Price = 666
                        },
                        new Item
                        {
                            Name = "Isded",
                            Description = "For all your fizzy needs.",
                            Hot = false,
                            Vegan = false,
                            Price = 250
                        },
                        new Item
                        {
                            Name = "Dr. Peppepepe",
                            Description = "Nothing's better than a good old Dr. Peppepepe.",
                            Hot = false,
                            Vegan = false,
                            Price = 240
                        },
                        new Item
                        {
                            Name = "Water",
                            Description = "Our most valuable water! Order 2 and the second is only 280!",
                            Hot = false,
                            Vegan = false,
                            Price = 280
                        }
                    }
                }

            };

            foreach (Category category in categories)
                context.Categories.Add(category);

            context.SaveChanges();
        }
    }
}

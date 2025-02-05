using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Data;
using Pizzeria.Models;

public class DbInit
{
    public static async Task InitializeAsync(ApplicationContext context, UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.FindByNameAsync("Admin") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (await roleManager.FindByNameAsync("Editor") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("Editor"));
        }

        if (await roleManager.FindByNameAsync("Client") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("Client"));
        }


        string adminEmail = "admin@gmail.com", adminPassword = "192837";
        if (await userManager.FindByNameAsync(adminEmail) == null)
        {
            User admin = new User
            {
                Email = adminEmail,
                UserName = adminEmail,
                PhoneNumber = "380970601478",
                Year = 1990,
                City = "Днепр",
                Address = "Титова 12, кв 33."
            };
            IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }


            User alex = new User
            {
                Email = "alex@gmail.com",
                UserName = "alex@gmail.com",
                PhoneNumber = "38096546798",
                Year = 2001,
                City = "Днепр",
                Address = "Карла Маркса 121, кв 32."
            };
            result = await userManager.CreateAsync(alex, "qwerty");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(alex, "Editor");
            }


            User tom = new User
            {
                Email = "tom@gmail.com",
                UserName = "tom@gmail.com",
                PhoneNumber = "380665459874",
                Year = 1995,
                City = "Днепр",
                Address = "Тополь 3, дом 44 кв 7."
            };
            result = await userManager.CreateAsync(tom, "1234567AS");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(tom, "Client");
            }


            User marry = new User
            {
                Email = "marry@in.ua",
                UserName = "marry@in.ua",
                PhoneNumber = "380964578796",
                Year = 1981,
                City = "Киев",
                Address = "Шевеченко, дом 7 кв 14."
            };
            result = await userManager.CreateAsync(marry, "2S91lds");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(marry, "Client");
            }
        }

        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Пицца",
                    Description = "Разнообразные виды пиццы",
                    Image = "https://images.pexels.com/photos/315755/pexels-photo-315755.jpeg"
                },
                new Category
                {
                    Name = "Салаты",
                    Description = "Салаты вкусные",
                    Image = "https://images.pexels.com/photos/4198263/pexels-photo-4198263.jpeg"
                },
                new Category
                {
                    Name = "Напитки",
                    Description = "Прохладительные и горячие напитки",
                    Image = "https://images.pexels.com/photos/159291/drink-cocktail-orange-juice-159291.jpeg"
                },
                new Category
                {
                    Name = "Десерты",
                    Description = "Сладкие угощения и торты",
                    Image = "https://images.pexels.com/photos/291528/pexels-photo-291528.jpeg"
                },
                new Category
                {
                    Name = "Бургеры",
                    Description = "Сочные бургеры с различными начинками",
                    Image = "https://images.pexels.com/photos/1633578/pexels-photo-1633578.jpeg"
                }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        if (!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync
            (
                new Product[]
                {
                    new Product
                    {
                        Name = "Пеперони", Description = "Пепперони, сыр Моцарелла, соус фирменный томатный, специи.",
                        Weight = 460,
                        Calories = 1160, Brand = "PizzaDay", Price = 140,
                        Category = context.Categories.FirstOrDefault(e => e.Name.Equals("Пицца")),
                        Type = ProductType.Dish, DateOfPublication = DateTime.Now
                    },


                    new Product
                    {
                        Name = "Чикен Чиз",
                        Description = "Kurka, sire mozzarella, sire cheddar, cucurudza, verticillium sauce.",
                        Weight = 520,
                        Calories = 1220, Brand = "PizzaDay", Price = 155,
                        Category = context.Categories.FirstOrDefault(e => e.Name.Equals("Пицца")),
                        Type = ProductType.Dish, DateOfPublication = DateTime.Now
                    },


                    new Product
                    {
                        Name = "Спрайт", Description = "Всегда освежает и бодрит!", Weight = 500,
                        Calories = 225, Brand = "PizzaDay", Price = 24,
                        Category = context.Categories.FirstOrDefault(e => e.Name.Equals("Напитки")),
                        Type = ProductType.Drink, DateOfPublication = DateTime.Now
                    },


                    new Product
                    {
                        Name = "Мясная",
                        Description =
                            "Ветчина, салями, бекон, сыр Моцарелла, помидор, маринованный лук, соус фирменный томатный.",
                        Weight = 570,
                        Calories = 1420, Brand = "Silpo", Price = 179,
                        Category = context.Categories.FirstOrDefault(e => e.Name.Equals("Пицца")),
                        Type = ProductType.Dish, DateOfPublication = DateTime.Now
                    },


                    new Product
                    {
                        Name = "Цезарь",
                        Description = "Курица, бекон, сыр Моцарелла, помидор, листья салата, яйца, соус фирменный.",
                        Weight = 300,
                        Calories = 680, Brand = "PizzaDay", Price = 95,
                        Category = context.Categories.FirstOrDefault(e => e.Name.Equals("Салаты")),
                        Type = ProductType.Dish, DateOfPublication = DateTime.Now
                    },
                }
            );
            await context.SaveChangesAsync();
        }
    }

    public static async Task CreateSeedDataAsync(ApplicationContext context, string[] categories, int rowCount = 50,
        bool clearData = true)
    {
        if (clearData)
        {
            ClearData(context);
        }

        context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));

        context.Database.ExecuteSqlRaw("DROP PROCEDURE IF EXISTS CreateSeedData");

        context.Database.ExecuteSqlRaw(@"
    CREATE PROCEDURE CreateSeedData 
    @RowCount INT,
    @pizzaCategoryId UNIQUEIDENTIFIER,
    @saladCategoryId UNIQUEIDENTIFIER,
    @drinkCategoryId UNIQUEIDENTIFIER
    AS
    BEGIN
        SET NOCOUNT ON;
        DECLARE @i INT = 0;

        BEGIN TRANSACTION;

        WHILE @i < @RowCount
        BEGIN
            INSERT INTO Products (Id, CategoryId, Image, Name, Description, Brand, Calories, Weight, Price, DateOfPublication, Type)
            VALUES (NEWID(), @pizzaCategoryId, '/productFiles/pizza.png', 
                    CONCAT('Pizza - ', @i), CONCAT('So tasty pizza - ', @i), 
                    CONCAT('Brand - St', @i), 
                    FLOOR(RAND() * (500 - 50 + 1) + 50), 
                    FLOOR(RAND() * (200 - 50 + 1) + 50), 
                    FLOOR(RAND() * (300 - 70 + 1) + 70), 
                    DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 3650), '2014-01-01'), 
                    0);
            SET @i = @i + 1;
        END

        SET @i = 0;
        WHILE @i < @RowCount
        BEGIN
            INSERT INTO Products (Id, CategoryId, Image, Name, Description, Brand, Calories, Weight, Price, DateOfPublication, Type)
            VALUES (NEWID(), @saladCategoryId, '/productFiles/salad.png', 
                    CONCAT('Salad - ', @i), CONCAT('Ooo`h so good salad - ', @i), 
                    CONCAT('Brand - Ki', @i), 
                    FLOOR(RAND() * (500 - 50 + 1) + 50), 
                    FLOOR(RAND() * (200 - 50 + 1) + 50), 
                    FLOOR(RAND() * (300 - 70 + 1) + 70), 
                    DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 3650), '2014-01-01'), 
                    2);
            SET @i = @i + 1;
        END

        SET @i = 0;
        WHILE @i < @RowCount
        BEGIN
            INSERT INTO Products (Id, CategoryId, Image, Name, Description, Brand, Calories, Weight, Price, DateOfPublication, Type)
            VALUES (NEWID(), @drinkCategoryId, '/productFiles/drink.png', 
                    CONCAT('Drink - ', @i), CONCAT('Drink fresh drinks - ', @i), 
                    CONCAT('Brand - Jh', @i), 
                    FLOOR(RAND() * (500 - 50 + 1) + 50), 
                    FLOOR(RAND() * (200 - 50 + 1) + 50), 
                    FLOOR(RAND() * (300 - 70 + 1) + 70), 
                    DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 3650), '2014-01-01'), 
                    1);
            SET @i = @i + 1;
        END

        COMMIT;
    END");

        using var transaction = context.Database.BeginTransaction();
        try
        {
            await context.Database.ExecuteSqlRawAsync(
                "EXEC CreateSeedData @RowCount = {0}, @pizzaCategoryId = {1}, @saladCategoryId = {2}, @drinkCategoryId = {3}",
                rowCount, Guid.Parse(categories[0]), Guid.Parse(categories[1]), Guid.Parse(categories[2])
            );

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }


    private static void ClearData(ApplicationContext context)
    {
        context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
        using var transaction = context.Database.BeginTransaction();
        try
        {
            context.Database.ExecuteSqlRaw("DELETE FROM Products");
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
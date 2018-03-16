namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'660ecb2d-45c5-4e25-94cc-0e3dd100eead', N'admin@vidly.com', 0, N'AB3yV4TkOd2+xdnuljybmlegrXaIbo2fXoSz6v68XquzQ7sll8I+25RyryYGej6ztA==', N'26ac49b1-c522-4fcc-8ae3-e8f097ca97a9', NULL, 0, 0, NULL, 1, 0, N'admin@vidly.com')
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'eb8c0ae1-68a4-4be9-b7b8-54bfd26c32d7', N'guest@vidly.com', 0, N'AMbuFBnjaAF+Teu7oSUB6TyW3hLAuY37CuCUkh5osc5DhDJvlNN9XdeoHOQvQ+CaMw==', N'b3640110-f494-40b6-be84-ea1f299e54c9', NULL, 0, 0, NULL, 1, 0, N'guest@vidly.com')

                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'a774f17f-96e9-4e14-9090-5dc190d522f3', N'CanManageMovies')

                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'660ecb2d-45c5-4e25-94cc-0e3dd100eead', N'a774f17f-96e9-4e14-9090-5dc190d522f3')
            ");
        }
        
        public override void Down()
        {
        }
    }
}

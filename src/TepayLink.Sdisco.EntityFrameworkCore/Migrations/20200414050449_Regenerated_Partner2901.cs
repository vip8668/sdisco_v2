using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TepayLink.Sdisco.Migrations
{
    public partial class Regenerated_Partner2901 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutMe",
                table: "AbpUsers",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AbpUsers",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "AbpUsers",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CityId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContryId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DistrictId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Dob",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Gender",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "HostUserId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageSpeak",
                table: "AbpUsers",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Point",
                table: "AbpUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "PrecintId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ranking",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubDomain",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "UserType",
                table: "AbpUsers",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "Work",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    BankCode = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Logo = table.Column<string>(nullable: true),
                    CardImage = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    ThumbImage = table.Column<string>(nullable: true),
                    Status = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookingRefunds",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    BookingDetailId = table.Column<long>(nullable: false),
                    RefundMethodId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<byte>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingRefunds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashoutMethodTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashoutMethodTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ProductType = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chatconversations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    FriendUserId = table.Column<long>(nullable: false),
                    UnreadCount = table.Column<int>(nullable: false),
                    ShardChatConversationId = table.Column<string>(nullable: true),
                    BookingId = table.Column<long>(nullable: true),
                    LastMessage = table.Column<string>(nullable: true),
                    Side = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chatconversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessageV2s",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    ChatConversationId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Side = table.Column<int>(nullable: false),
                    ReadState = table.Column<int>(nullable: false),
                    ReceiverReadState = table.Column<int>(nullable: false),
                    SharedMessageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessageV2s", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClaimReasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    CurrencySign = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Detinations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Image = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    IsTop = table.Column<bool>(nullable: false),
                    BookingCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HelpCategories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    CategoryName = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    OrderCode = table.Column<string>(nullable: true),
                    OrderType = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    OrderRef = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    BankCode = table.Column<string>(nullable: true),
                    CardId = table.Column<long>(nullable: false),
                    CardNumber = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    IssueDate = table.Column<string>(nullable: true),
                    NameOnCard = table.Column<string>(nullable: true),
                    TransactionId = table.Column<string>(nullable: true),
                    BookingId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartnerRevenues",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Userid = table.Column<long>(nullable: false),
                    RevenueType = table.Column<byte>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    Point = table.Column<decimal>(nullable: false),
                    Money = table.Column<decimal>(nullable: false),
                    Status = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerRevenues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartnerShips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerShips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefundReasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ReasonText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RevenueByMonths",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    Revenue = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueByMonths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SearchHistories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    Keyword = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShareTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    IP = table.Column<string>(nullable: true),
                    Point = table.Column<decimal>(nullable: false),
                    ProductId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShortLinks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    FullLink = table.Column<string>(nullable: true),
                    ShortCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Side = table.Column<byte>(nullable: false),
                    TransType = table.Column<byte>(nullable: false),
                    WalletType = table.Column<int>(nullable: false),
                    BookingDetailId = table.Column<long>(nullable: true),
                    RefId = table.Column<long>(nullable: true),
                    Descrition = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserReviewDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    Itineraty = table.Column<double>(nullable: false),
                    Service = table.Column<double>(nullable: false),
                    Transport = table.Column<double>(nullable: false),
                    GuideTour = table.Column<double>(nullable: false),
                    Food = table.Column<double>(nullable: false),
                    Rating = table.Column<double>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReviewDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserReviews",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    ReviewCount = table.Column<double>(nullable: false),
                    Itineraty = table.Column<double>(nullable: false),
                    Service = table.Column<double>(nullable: false),
                    Transport = table.Column<double>(nullable: false),
                    GuideTour = table.Column<double>(nullable: false),
                    Food = table.Column<double>(nullable: false),
                    Rating = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSubcribers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubcribers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Balance = table.Column<decimal>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WithDrawRequests",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    TransactionId = table.Column<long>(nullable: false),
                    BankAccountId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithDrawRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankBranchs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    BranchName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    BankId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankBranchs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankBranchs_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlogComments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Rating = table.Column<double>(nullable: false),
                    ReplyId = table.Column<long>(nullable: true),
                    WebSite = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    BlogPostId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogComments_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDefaultCashoutMethodTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    CashoutMethodTypeId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDefaultCashoutMethodTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDefaultCashoutMethodTypes_CashoutMethodTypes_CashoutMeth~",
                        column: x => x.CashoutMethodTypeId,
                        principalTable: "CashoutMethodTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDefaultCashoutMethodTypes_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingClaims",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    BookingDetailId = table.Column<long>(nullable: false),
                    ClaimReasonId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingClaims_ClaimReasons_ClaimReasonId",
                        column: x => x.ClaimReasonId,
                        principalTable: "ClaimReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    WebsiteUrl = table.Column<string>(nullable: true),
                    Languages = table.Column<string>(nullable: true),
                    SkypeId = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    AffiliateKey = table.Column<string>(nullable: true),
                    Status = table.Column<byte>(nullable: false),
                    AlreadyBecomeSdiscoPartner = table.Column<bool>(nullable: false),
                    HasDriverLicense = table.Column<bool>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    DetinationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partners_Detinations_DetinationId",
                        column: x => x.DetinationId,
                        principalTable: "Detinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partners_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HelpContents",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Question = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true),
                    HelpCategoryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HelpContents_HelpCategories_HelpCategoryId",
                        column: x => x.HelpCategoryId,
                        principalTable: "HelpCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: true),
                    DisplayAddress = table.Column<string>(nullable: true),
                    GoogleAddress = table.Column<string>(nullable: true),
                    Overview = table.Column<string>(nullable: false),
                    WhatToExpect = table.Column<string>(nullable: false),
                    Lat = table.Column<double>(nullable: false),
                    Long = table.Column<double>(nullable: false),
                    DetinationId = table.Column<long>(nullable: false),
                    PlaceCategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Places_Detinations_DetinationId",
                        column: x => x.DetinationId,
                        principalTable: "Detinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Places_PlaceCategories_PlaceCategoryId",
                        column: x => x.PlaceCategoryId,
                        principalTable: "PlaceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BankAccountInfos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    AccountName = table.Column<string>(nullable: true),
                    AccountNo = table.Column<string>(nullable: true),
                    BankId = table.Column<int>(nullable: false),
                    BankBranchId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccountInfos_BankBranchs_BankBranchId",
                        column: x => x.BankBranchId,
                        principalTable: "BankBranchs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankAccountInfos_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccountInfos_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NearbyPlaces",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PlaceId = table.Column<long>(nullable: true),
                    NearbyPlaceId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NearbyPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NearbyPlaces_Places_NearbyPlaceId",
                        column: x => x.NearbyPlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NearbyPlaces_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Overview = table.Column<string>(nullable: true),
                    Policies = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: false),
                    StartTime = table.Column<string>(nullable: true),
                    IncludeTourGuide = table.Column<bool>(nullable: false),
                    AllowRetail = table.Column<bool>(nullable: false),
                    TotalSlot = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    CostPrice = table.Column<decimal>(nullable: false),
                    InstantBook = table.Column<bool>(nullable: false),
                    Star = table.Column<int>(nullable: false),
                    TripLengh = table.Column<int>(nullable: false),
                    IsHotDeal = table.Column<bool>(nullable: false),
                    IsBestSeller = table.Column<bool>(nullable: false),
                    IsTrending = table.Column<bool>(nullable: false),
                    IsTop = table.Column<bool>(nullable: false),
                    BookingCount = table.Column<int>(nullable: false),
                    CoppyCount = table.Column<int>(nullable: false),
                    ShareCount = table.Column<int>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    LikeCount = table.Column<int>(nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    TripCoppyId = table.Column<long>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    ExtraData = table.Column<string>(nullable: true),
                    WhatWeDo = table.Column<string>(nullable: true),
                    LastBookTime = table.Column<DateTime>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    HostUserId = table.Column<long>(nullable: true),
                    PlaceId = table.Column<long>(nullable: true),
                    LanguageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_AbpUsers_HostUserId",
                        column: x => x.HostUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_AbpLanguages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "AbpLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlogProductRelateds",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    BlogPostId = table.Column<long>(nullable: true),
                    ProductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogProductRelateds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogProductRelateds_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlogProductRelateds_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    BookingId = table.Column<long>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TripLength = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ProductScheduleId = table.Column<long>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Fee = table.Column<decimal>(nullable: false),
                    HostPaymentStatus = table.Column<byte>(nullable: false),
                    HostUserId = table.Column<long>(nullable: false),
                    BookingUserId = table.Column<long>(nullable: false),
                    IsDone = table.Column<bool>(nullable: false),
                    AffiliateUserId = table.Column<long>(nullable: false),
                    RoomId = table.Column<long>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    CancelDate = table.Column<DateTime>(nullable: true),
                    RefundAmount = table.Column<decimal>(nullable: true),
                    ProductDetailComboId = table.Column<long>(nullable: true),
                    ProductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TripLength = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ProductScheduleId = table.Column<long>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Fee = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    GuestInfo = table.Column<string>(nullable: true),
                    CouponCode = table.Column<string>(nullable: true),
                    BonusAmount = table.Column<decimal>(nullable: false),
                    Contact = table.Column<string>(nullable: true),
                    CouponId = table.Column<long>(nullable: false),
                    TotalAmount = table.Column<decimal>(nullable: false),
                    ProductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ThumbImage = table.Column<string>(nullable: true),
                    ProductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ImageType = table.Column<int>(nullable: false),
                    Tag = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    ProductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductReviewDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    RatingAvg = table.Column<double>(nullable: false),
                    Intineraty = table.Column<double>(nullable: false),
                    Service = table.Column<double>(nullable: false),
                    Transport = table.Column<double>(nullable: false),
                    GuideTour = table.Column<decimal>(nullable: false),
                    Food = table.Column<double>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    BookingId = table.Column<long>(nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    ReplyComment = table.Column<string>(nullable: true),
                    ReplyId = table.Column<long>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Reviewer = table.Column<string>(nullable: true),
                    ProductId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReviewDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReviewDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductReviews",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    RatingAvg = table.Column<double>(nullable: false),
                    ReviewCount = table.Column<int>(nullable: false),
                    Intineraty = table.Column<double>(nullable: false),
                    Service = table.Column<double>(nullable: false),
                    Transport = table.Column<double>(nullable: false),
                    GuideTour = table.Column<double>(nullable: false),
                    Food = table.Column<double>(nullable: false),
                    ProductId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSchedules",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    TotalSlot = table.Column<int>(nullable: false),
                    TotalBook = table.Column<int>(nullable: false),
                    LockedSlot = table.Column<int>(nullable: false),
                    TripLength = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    TicketPrice = table.Column<decimal>(nullable: false),
                    CostPrice = table.Column<decimal>(nullable: false),
                    HotelPrice = table.Column<decimal>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    DepartureTime = table.Column<string>(nullable: true),
                    Revenue = table.Column<decimal>(nullable: false),
                    AllowBook = table.Column<bool>(nullable: false),
                    ProductId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSchedules_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductUtilities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ProductId = table.Column<long>(nullable: false),
                    UtilityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUtilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductUtilities_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductUtilities_Utilities_UtilityId",
                        column: x => x.UtilityId,
                        principalTable: "Utilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatedProducts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ProductId = table.Column<long>(nullable: true),
                    RelatedProductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RelatedProducts_Products_RelatedProductId",
                        column: x => x.RelatedProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaveItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ProductId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaveItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimilarProducts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ProductId = table.Column<long>(nullable: false),
                    SimilarProductId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimilarProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimilarProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SimilarProducts_Products_SimilarProductId",
                        column: x => x.SimilarProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuggestedProducts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ProductId = table.Column<long>(nullable: false),
                    SuggestedProductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuggestedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuggestedProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuggestedProducts_Products_SuggestedProductId",
                        column: x => x.SuggestedProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransPortdetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    TotalSeat = table.Column<int>(nullable: false),
                    IsTaxi = table.Column<bool>(nullable: false),
                    ProductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransPortdetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransPortdetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetailCombos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    RoomId = table.Column<long>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProductId = table.Column<long>(nullable: true),
                    ProductDetailId = table.Column<long>(nullable: true),
                    ItemId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetailCombos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetailCombos_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductDetailCombos_ProductDetails_ProductDetailId",
                        column: x => x.ProductDetailId,
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductDetailCombos_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountInfos_BankBranchId",
                table: "BankAccountInfos",
                column: "BankBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountInfos_BankId",
                table: "BankAccountInfos",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountInfos_TenantId",
                table: "BankAccountInfos",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountInfos_UserId",
                table: "BankAccountInfos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BankBranchs_BankId",
                table: "BankBranchs",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankBranchs_TenantId",
                table: "BankBranchs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_TenantId",
                table: "Banks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_BlogPostId",
                table: "BlogComments",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_TenantId",
                table: "BlogComments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_TenantId",
                table: "BlogPosts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogProductRelateds_BlogPostId",
                table: "BlogProductRelateds",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogProductRelateds_ProductId",
                table: "BlogProductRelateds",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogProductRelateds_TenantId",
                table: "BlogProductRelateds",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingClaims_ClaimReasonId",
                table: "BookingClaims",
                column: "ClaimReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingClaims_TenantId",
                table: "BookingClaims",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_ProductId",
                table: "BookingDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_TenantId",
                table: "BookingDetails",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRefunds_TenantId",
                table: "BookingRefunds",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ProductId",
                table: "Bookings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TenantId",
                table: "Bookings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutMethodTypes_TenantId",
                table: "CashoutMethodTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_TenantId",
                table: "Categories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Chatconversations_TenantId",
                table: "Chatconversations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessageV2s_TenantId",
                table: "ChatMessageV2s",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimReasons_TenantId",
                table: "ClaimReasons",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSettings_TenantId",
                table: "ClientSettings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_TenantId",
                table: "Countries",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_TenantId",
                table: "Coupons",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_TenantId",
                table: "Currencies",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Detinations_TenantId",
                table: "Detinations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpCategories_TenantId",
                table: "HelpCategories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpContents_HelpCategoryId",
                table: "HelpContents",
                column: "HelpCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpContents_TenantId",
                table: "HelpContents",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_NearbyPlaces_NearbyPlaceId",
                table: "NearbyPlaces",
                column: "NearbyPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_NearbyPlaces_PlaceId",
                table: "NearbyPlaces",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_NearbyPlaces_TenantId",
                table: "NearbyPlaces",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TenantId",
                table: "Orders",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerRevenues_TenantId",
                table: "PartnerRevenues",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_DetinationId",
                table: "Partners",
                column: "DetinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_TenantId",
                table: "Partners",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_UserId",
                table: "Partners",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerShips_TenantId",
                table: "PartnerShips",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceCategories_TenantId",
                table: "PlaceCategories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_DetinationId",
                table: "Places",
                column: "DetinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_PlaceCategoryId",
                table: "Places",
                column: "PlaceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_TenantId",
                table: "Places",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetailCombos_ItemId",
                table: "ProductDetailCombos",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetailCombos_ProductDetailId",
                table: "ProductDetailCombos",
                column: "ProductDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetailCombos_ProductId",
                table: "ProductDetailCombos",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetailCombos_TenantId",
                table: "ProductDetailCombos",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProductId",
                table: "ProductDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_TenantId",
                table: "ProductDetails",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_TenantId",
                table: "ProductImages",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviewDetails_ProductId",
                table: "ProductReviewDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviewDetails_TenantId",
                table: "ProductReviewDetails",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_TenantId",
                table: "ProductReviews",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_HostUserId",
                table: "Products",
                column: "HostUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_LanguageId",
                table: "Products",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PlaceId",
                table: "Products",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TenantId",
                table: "Products",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSchedules_ProductId",
                table: "ProductSchedules",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSchedules_TenantId",
                table: "ProductSchedules",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUtilities_ProductId",
                table: "ProductUtilities",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUtilities_TenantId",
                table: "ProductUtilities",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUtilities_UtilityId",
                table: "ProductUtilities",
                column: "UtilityId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundReasons_TenantId",
                table: "RefundReasons",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedProducts_ProductId",
                table: "RelatedProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedProducts_RelatedProductId",
                table: "RelatedProducts",
                column: "RelatedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedProducts_TenantId",
                table: "RelatedProducts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RevenueByMonths_TenantId",
                table: "RevenueByMonths",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveItems_ProductId",
                table: "SaveItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveItems_TenantId",
                table: "SaveItems",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistories_TenantId",
                table: "SearchHistories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareTransactions_TenantId",
                table: "ShareTransactions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortLinks_TenantId",
                table: "ShortLinks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarProducts_ProductId",
                table: "SimilarProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarProducts_SimilarProductId",
                table: "SimilarProducts",
                column: "SimilarProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarProducts_TenantId",
                table: "SimilarProducts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestedProducts_ProductId",
                table: "SuggestedProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestedProducts_SuggestedProductId",
                table: "SuggestedProducts",
                column: "SuggestedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestedProducts_TenantId",
                table: "SuggestedProducts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TenantId",
                table: "Transactions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TransPortdetails_ProductId",
                table: "TransPortdetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransPortdetails_TenantId",
                table: "TransPortdetails",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDefaultCashoutMethodTypes_CashoutMethodTypeId",
                table: "UserDefaultCashoutMethodTypes",
                column: "CashoutMethodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDefaultCashoutMethodTypes_TenantId",
                table: "UserDefaultCashoutMethodTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDefaultCashoutMethodTypes_UserId",
                table: "UserDefaultCashoutMethodTypes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReviewDetails_TenantId",
                table: "UserReviewDetails",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReviews_TenantId",
                table: "UserReviews",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubcribers_TenantId",
                table: "UserSubcribers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilities_TenantId",
                table: "Utilities",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_TenantId",
                table: "Wallets",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WithDrawRequests_TenantId",
                table: "WithDrawRequests",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccountInfos");

            migrationBuilder.DropTable(
                name: "BlogComments");

            migrationBuilder.DropTable(
                name: "BlogProductRelateds");

            migrationBuilder.DropTable(
                name: "BookingClaims");

            migrationBuilder.DropTable(
                name: "BookingDetails");

            migrationBuilder.DropTable(
                name: "BookingRefunds");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Chatconversations");

            migrationBuilder.DropTable(
                name: "ChatMessageV2s");

            migrationBuilder.DropTable(
                name: "ClientSettings");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "HelpContents");

            migrationBuilder.DropTable(
                name: "NearbyPlaces");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PartnerRevenues");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "PartnerShips");

            migrationBuilder.DropTable(
                name: "ProductDetailCombos");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductReviewDetails");

            migrationBuilder.DropTable(
                name: "ProductReviews");

            migrationBuilder.DropTable(
                name: "ProductSchedules");

            migrationBuilder.DropTable(
                name: "ProductUtilities");

            migrationBuilder.DropTable(
                name: "RefundReasons");

            migrationBuilder.DropTable(
                name: "RelatedProducts");

            migrationBuilder.DropTable(
                name: "RevenueByMonths");

            migrationBuilder.DropTable(
                name: "SaveItems");

            migrationBuilder.DropTable(
                name: "SearchHistories");

            migrationBuilder.DropTable(
                name: "ShareTransactions");

            migrationBuilder.DropTable(
                name: "ShortLinks");

            migrationBuilder.DropTable(
                name: "SimilarProducts");

            migrationBuilder.DropTable(
                name: "SuggestedProducts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "TransPortdetails");

            migrationBuilder.DropTable(
                name: "UserDefaultCashoutMethodTypes");

            migrationBuilder.DropTable(
                name: "UserReviewDetails");

            migrationBuilder.DropTable(
                name: "UserReviews");

            migrationBuilder.DropTable(
                name: "UserSubcribers");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "WithDrawRequests");

            migrationBuilder.DropTable(
                name: "BankBranchs");

            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "ClaimReasons");

            migrationBuilder.DropTable(
                name: "HelpCategories");

            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.DropTable(
                name: "Utilities");

            migrationBuilder.DropTable(
                name: "CashoutMethodTypes");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Detinations");

            migrationBuilder.DropTable(
                name: "PlaceCategories");

            migrationBuilder.DropColumn(
                name: "AboutMe",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "ContryId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Dob",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "HostUserId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "LanguageSpeak",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Point",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "PrecintId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Ranking",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "SubDomain",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Work",
                table: "AbpUsers");
        }
    }
}

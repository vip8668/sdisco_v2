using TepayLink.Sdisco.Cashout;
using TepayLink.Sdisco.Search;
using TepayLink.Sdisco.Client;

using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Help;
using TepayLink.Sdisco.Blog;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.AdminConfig;

using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using TepayLink.Sdisco.Authorization.Roles;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Chat;
using TepayLink.Sdisco.Editions;
using TepayLink.Sdisco.Friendships;
using TepayLink.Sdisco.MultiTenancy;
using TepayLink.Sdisco.MultiTenancy.Accounting;
using TepayLink.Sdisco.MultiTenancy.Payments;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.EntityFrameworkCore
{
    public class SdiscoDbContext : AbpZeroDbContext<Tenant, Role, User, SdiscoDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<Chatconversation> Chatconversations { get; set; }

        public virtual DbSet<UserDefaultCashoutMethodType> UserDefaultCashoutMethodTypes { get; set; }

        public virtual DbSet<CashoutMethodType> CashoutMethodTypes { get; set; }

        public virtual DbSet<TransPortdetail> TransPortdetails { get; set; }

        public virtual DbSet<Coupon> Coupons { get; set; }

        public virtual DbSet<SearchHistory> SearchHistories { get; set; }

        public virtual DbSet<Wallet> Wallets { get; set; }

        public virtual DbSet<PartnerShip> PartnerShips { get; set; }

        public virtual DbSet<Partner> Partners { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<ClientSetting> ClientSettings { get; set; }

        public virtual DbSet<BookingClaim> BookingClaims { get; set; }

        public virtual DbSet<ClaimReason> ClaimReasons { get; set; }

        public virtual DbSet<BookingDetail> BookingDetails { get; set; }

        public virtual DbSet<Booking> Bookings { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Currency> Currencies { get; set; }

        public virtual DbSet<UserSubcriber> UserSubcribers { get; set; }

        public virtual DbSet<ProductSchedule> ProductSchedules { get; set; }

        public virtual DbSet<ProductDetailCombo> ProductDetailCombos { get; set; }

        public virtual DbSet<ProductDetail> ProductDetails { get; set; }

        public virtual DbSet<UserReviewDetail> UserReviewDetails { get; set; }

        public virtual DbSet<UserReview> UserReviews { get; set; }

        public virtual DbSet<ProductReviewDetail> ProductReviewDetails { get; set; }

        public virtual DbSet<ProductReview> ProductReviews { get; set; }

        public virtual DbSet<ProductUtility> ProductUtilities { get; set; }

        public virtual DbSet<Utility> Utilities { get; set; }

        public virtual DbSet<SaveItem> SaveItems { get; set; }

        public virtual DbSet<HelpContent> HelpContents { get; set; }

        public virtual DbSet<HelpCategory> HelpCategories { get; set; }

        public virtual DbSet<BlogProductRelated> BlogProductRelateds { get; set; }

        public virtual DbSet<BlogComment> BlogComments { get; set; }

        public virtual DbSet<BlogPost> BlogPosts { get; set; }

        public virtual DbSet<BankAccountInfo> BankAccountInfos { get; set; }

        public virtual DbSet<BankBranch> BankBranchs { get; set; }

        public virtual DbSet<Bank> Banks { get; set; }

        public virtual DbSet<ProductImage> ProductImages { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Place> Places { get; set; }

        public virtual DbSet<PlaceCategory> PlaceCategories { get; set; }

        public virtual DbSet<Detination> Detinations { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public SdiscoDbContext(DbContextOptions<SdiscoDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
            modelBuilder.Entity<Chatconversation>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<UserDefaultCashoutMethodType>(u =>
            {
                u.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<CashoutMethodType>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<TransPortdetail>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Coupon>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<SearchHistory>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Wallet>(w =>
            {
                w.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<PartnerShip>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Partner>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Order>(o =>
            {
                o.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ClientSetting>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<BookingClaim>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ClaimReason>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<BookingDetail>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Booking>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Country>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Currency>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<UserSubcriber>(u =>
            {
                u.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ProductSchedule>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ProductDetailCombo>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ProductDetail>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<UserReviewDetail>(u =>
            {
                u.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<UserReview>(u =>
            {
                u.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ProductReviewDetail>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ProductReview>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ProductUtility>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Utility>(u =>
            {
                u.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<SaveItem>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<HelpContent>(h =>
            {
                h.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<HelpCategory>(h =>
            {
                h.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<BlogProductRelated>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<BlogComment>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<BlogPost>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<BankAccountInfo>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<BankBranch>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Bank>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ProductImage>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Product>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Category>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Place>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<PlaceCategory>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Detination>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}

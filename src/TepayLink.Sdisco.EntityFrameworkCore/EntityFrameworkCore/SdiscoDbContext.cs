using TepayLink.Sdisco.Help;
using TepayLink.Sdisco.Blog;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.Products;
using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

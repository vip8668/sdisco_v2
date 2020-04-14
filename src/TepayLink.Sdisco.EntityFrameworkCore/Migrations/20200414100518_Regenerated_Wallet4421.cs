using Microsoft.EntityFrameworkCore.Migrations;

namespace TepayLink.Sdisco.Migrations
{
    public partial class Regenerated_Wallet4421 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviewDetails_Products_ProductId",
                table: "ProductReviewDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ProductReviews",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ProductReviewDetails",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpUserLogins",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "AccessCode",
                table: "AbpUserLogins",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "AbpUserLogins",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_UserId1",
                table: "AbpUserLogins",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUserLogins_AbpUsers_UserId1",
                table: "AbpUserLogins",
                column: "UserId1",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviewDetails_Products_ProductId",
                table: "ProductReviewDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUserLogins_AbpUsers_UserId1",
                table: "AbpUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviewDetails_Products_ProductId",
                table: "ProductReviewDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_AbpUserLogins_UserId1",
                table: "AbpUserLogins");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpUserLogins");

            migrationBuilder.DropColumn(
                name: "AccessCode",
                table: "AbpUserLogins");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "AbpUserLogins");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ProductReviews",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ProductReviewDetails",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviewDetails_Products_ProductId",
                table: "ProductReviewDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

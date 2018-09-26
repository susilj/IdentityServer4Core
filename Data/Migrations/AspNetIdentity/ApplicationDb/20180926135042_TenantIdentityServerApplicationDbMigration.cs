using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServer4Core.Data.Migrations.AspNetIdentity.ApplicationDb
{
    public partial class TenantIdentityServerApplicationDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "AspNetUsers",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "AspNetRoles",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AspNetTenants",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    DomainName = table.Column<string>(maxLength: 250, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetTenants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_TenantId",
                table: "AspNetRoles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetTenants_Code",
                table: "AspNetTenants",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetTenants_DomainName",
                table: "AspNetTenants",
                column: "DomainName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetTenants_TenantId",
                table: "AspNetRoles",
                column: "TenantId",
                principalTable: "AspNetTenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetTenants_TenantId",
                table: "AspNetUsers",
                column: "TenantId",
                principalTable: "AspNetTenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetTenants_TenantId",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetTenants_TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AspNetTenants");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_TenantId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AspNetRoles");
        }
    }
}

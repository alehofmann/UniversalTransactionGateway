using Microsoft.EntityFrameworkCore.Migrations;

namespace TikiSoft.UniversalPaymentGateway.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MerchantInfo",
                columns: table => new
                {
                    MerchantID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantInfo", x => x.MerchantID);
                });

            migrationBuilder.CreateTable(
                name: "ProcessorConfig",
                columns: table => new
                {
                    ConfigItemId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TerminalId = table.Column<string>(nullable: false),
                    Processor = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessorConfig", x => x.ConfigItemId);
                });

            migrationBuilder.CreateTable(
                name: "MerchantConfig",
                columns: table => new
                {
                    ConfigItemId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MerchantId = table.Column<int>(nullable: false),
                    TerminalId = table.Column<string>(nullable: false),
                    Processor = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantConfig", x => x.ConfigItemId);
                    table.UniqueConstraint("AK_MerchantConfig_MerchantId_TerminalId_Processor_Key", x => new { x.MerchantId, x.TerminalId, x.Processor, x.Key });
                    table.ForeignKey(
                        name: "FK_MerchantConfig_MerchantInfo_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "MerchantInfo",
                        principalColumn: "MerchantID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MerchantConfig");

            migrationBuilder.DropTable(
                name: "ProcessorConfig");

            migrationBuilder.DropTable(
                name: "MerchantInfo");
        }
    }
}

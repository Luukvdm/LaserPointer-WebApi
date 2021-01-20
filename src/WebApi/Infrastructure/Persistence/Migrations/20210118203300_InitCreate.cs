using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LaserPointer.WebApi.Infrastructure.Persistence.Migrations
{
    public partial class InitCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HashAlgos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(nullable: false),
                    Format = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HashAlgos", x => x.Id);
                    table.UniqueConstraint("AK_HashAlgos_Type", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    HashAlgoId = table.Column<int>(nullable: false),
                    HashAlgoId1 = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_HashAlgos_HashAlgoId",
                        column: x => x.HashAlgoId,
                        principalTable: "HashAlgos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Jobs_HashAlgos_HashAlgoId1",
                        column: x => x.HashAlgoId1,
                        principalTable: "HashAlgos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hashes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobId = table.Column<int>(nullable: true),
                    Value = table.Column<byte[]>(nullable: false),
                    PlainValue = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hashes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hashes_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hashes_JobId",
                table: "Hashes",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_HashAlgoId",
                table: "Jobs",
                column: "HashAlgoId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_HashAlgoId1",
                table: "Jobs",
                column: "HashAlgoId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hashes");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "HashAlgos");
        }
    }
}

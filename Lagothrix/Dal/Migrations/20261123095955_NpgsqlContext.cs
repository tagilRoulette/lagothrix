using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class NpgsqlContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParameterEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParameterValueEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ParameterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterValueEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParameterValueEntity_ParameterEntity_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "ParameterEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LotEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(155)", maxLength: 155, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    EstimatedValue = table.Column<decimal>(type: "numeric", nullable: true),
                    DesiredExchange = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LotEntity_CategoryEntity_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LotEntity_UserEntity_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AiEstimatesEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EstimatedValue = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LotId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiEstimatesEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiEstimatesEntity_LotEntity_LotId",
                        column: x => x.LotId,
                        principalTable: "LotEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeOfferEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OfferLotId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetLotId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeOfferEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeOfferEntity_LotEntity_OfferLotId",
                        column: x => x.OfferLotId,
                        principalTable: "LotEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExchangeOfferEntity_LotEntity_TargetLotId",
                        column: x => x.TargetLotId,
                        principalTable: "LotEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExchangeOfferEntity_UserEntity_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExchangeOfferEntity_UserEntity_SenderId",
                        column: x => x.SenderId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LotEntityParameterEntity",
                columns: table => new
                {
                    LotsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParametersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotEntityParameterEntity", x => new { x.LotsId, x.ParametersId });
                    table.ForeignKey(
                        name: "FK_LotEntityParameterEntity_LotEntity_LotsId",
                        column: x => x.LotsId,
                        principalTable: "LotEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LotEntityParameterEntity_ParameterEntity_ParametersId",
                        column: x => x.ParametersId,
                        principalTable: "ParameterEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LotImageEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    LotId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotImageEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LotImageEntity_LotEntity_LotId",
                        column: x => x.LotId,
                        principalTable: "LotEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AiEstimatesEntity_LotId",
                table: "AiEstimatesEntity",
                column: "LotId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOfferEntity_OfferLotId",
                table: "ExchangeOfferEntity",
                column: "OfferLotId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOfferEntity_ReceiverId",
                table: "ExchangeOfferEntity",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOfferEntity_SenderId",
                table: "ExchangeOfferEntity",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOfferEntity_TargetLotId",
                table: "ExchangeOfferEntity",
                column: "TargetLotId");

            migrationBuilder.CreateIndex(
                name: "IX_LotEntity_CategoryId",
                table: "LotEntity",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LotEntity_UserId",
                table: "LotEntity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LotEntityParameterEntity_ParametersId",
                table: "LotEntityParameterEntity",
                column: "ParametersId");

            migrationBuilder.CreateIndex(
                name: "IX_LotImageEntity_LotId",
                table: "LotImageEntity",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterValueEntity_ParameterId",
                table: "ParameterValueEntity",
                column: "ParameterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AiEstimatesEntity");

            migrationBuilder.DropTable(
                name: "ExchangeOfferEntity");

            migrationBuilder.DropTable(
                name: "LotEntityParameterEntity");

            migrationBuilder.DropTable(
                name: "LotImageEntity");

            migrationBuilder.DropTable(
                name: "ParameterValueEntity");

            migrationBuilder.DropTable(
                name: "LotEntity");

            migrationBuilder.DropTable(
                name: "ParameterEntity");

            migrationBuilder.DropTable(
                name: "CategoryEntity");

            migrationBuilder.DropTable(
                name: "UserEntity");
        }
    }
}

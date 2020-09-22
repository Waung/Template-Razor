using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace gls_core_skeleton.Migrations
{
    public partial class gls_model_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "m_feature_group",
                columns: table => new
                {
                    m_feature_group_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    feature_group_name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_feature_group", x => x.m_feature_group_id);
                });

            migrationBuilder.CreateTable(
                name: "m_parameters",
                columns: table => new
                {
                    m_parameter_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    parameter_group = table.Column<string>(nullable: false),
                    parameter_key = table.Column<string>(nullable: false),
                    parameter_value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_parameters", x => x.m_parameter_id);
                });

            migrationBuilder.CreateTable(
                name: "m_user_group",
                columns: table => new
                {
                    m_user_group_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_group_name = table.Column<string>(maxLength: 100, nullable: false),
                    user_group_default = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_user_group", x => x.m_user_group_id);
                });

            migrationBuilder.CreateTable(
                name: "m_feature",
                columns: table => new
                {
                    m_feature_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    feature_name = table.Column<string>(maxLength: 100, nullable: false),
                    feature_sequence = table.Column<int>(nullable: false),
                    feature_url = table.Column<string>(maxLength: 255, nullable: false),
                    feature_icon = table.Column<string>(maxLength: 50, nullable: false),
                    feature_private = table.Column<bool>(nullable: false),
                    m_feature_group_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_feature", x => x.m_feature_id);
                    table.ForeignKey(
                        name: "FK_m_feature_m_feature_group_m_feature_group_id",
                        column: x => x.m_feature_group_id,
                        principalTable: "m_feature_group",
                        principalColumn: "m_feature_group_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "m_user",
                columns: table => new
                {
                    m_user_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_name = table.Column<string>(maxLength: 50, nullable: false),
                    user_password = table.Column<string>(maxLength: 100, nullable: false),
                    user_email = table.Column<string>(maxLength: 50, nullable: true),
                    user_active = table.Column<bool>(nullable: false),
                    m_user_group_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_user", x => x.m_user_id);
                    table.ForeignKey(
                        name: "FK_m_user_m_user_group_m_user_group_id",
                        column: x => x.m_user_group_id,
                        principalTable: "m_user_group",
                        principalColumn: "m_user_group_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "feature_map",
                columns: table => new
                {
                    feature_map_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    m_user_group_id = table.Column<int>(nullable: false),
                    m_feature_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feature_map", x => x.feature_map_id);
                    table.ForeignKey(
                        name: "FK_feature_map_m_feature_m_feature_id",
                        column: x => x.m_feature_id,
                        principalTable: "m_feature",
                        principalColumn: "m_feature_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_feature_map_m_user_group_m_user_group_id",
                        column: x => x.m_user_group_id,
                        principalTable: "m_user_group",
                        principalColumn: "m_user_group_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_feature_map_m_feature_id",
                table: "feature_map",
                column: "m_feature_id");

            migrationBuilder.CreateIndex(
                name: "IX_feature_map_m_user_group_id",
                table: "feature_map",
                column: "m_user_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_feature_m_feature_group_id",
                table: "m_feature",
                column: "m_feature_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_user_m_user_group_id",
                table: "m_user",
                column: "m_user_group_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feature_map");

            migrationBuilder.DropTable(
                name: "m_parameters");

            migrationBuilder.DropTable(
                name: "m_user");

            migrationBuilder.DropTable(
                name: "m_feature");

            migrationBuilder.DropTable(
                name: "m_user_group");

            migrationBuilder.DropTable(
                name: "m_feature_group");
        }
    }
}

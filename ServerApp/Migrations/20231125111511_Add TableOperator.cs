using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTableOperator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operators_Tables_Table number",
                table: "Operators");

            migrationBuilder.DropIndex(
                name: "IX_Operators_Table number",
                table: "Operators");

            migrationBuilder.DropColumn(
                name: "Operator Id",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "Table number",
                table: "Operators");

            migrationBuilder.CreateTable(
                name: "TableOperator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableId = table.Column<int>(type: "int", nullable: false),
                    OperatorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableOperator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableOperator_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableOperator_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TableOperator_OperatorId",
                table: "TableOperator",
                column: "OperatorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TableOperator_TableId",
                table: "TableOperator",
                column: "TableId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableOperator");

            migrationBuilder.AddColumn<int>(
                name: "Operator Id",
                table: "Tables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Table number",
                table: "Operators",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Operators_Table number",
                table: "Operators",
                column: "Table number",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Operators_Tables_Table number",
                table: "Operators",
                column: "Table number",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

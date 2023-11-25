using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTableOperatorsDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableOperator_Operators_Operator Id",
                table: "TableOperator");

            migrationBuilder.DropForeignKey(
                name: "FK_TableOperator_Tables_Table Id",
                table: "TableOperator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TableOperator",
                table: "TableOperator");

            migrationBuilder.RenameTable(
                name: "TableOperator",
                newName: "TableOperators");

            migrationBuilder.RenameIndex(
                name: "IX_TableOperator_Table Id",
                table: "TableOperators",
                newName: "IX_TableOperators_Table Id");

            migrationBuilder.RenameIndex(
                name: "IX_TableOperator_Operator Id",
                table: "TableOperators",
                newName: "IX_TableOperators_Operator Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TableOperators",
                table: "TableOperators",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TableOperators_Operators_Operator Id",
                table: "TableOperators",
                column: "Operator Id",
                principalTable: "Operators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TableOperators_Tables_Table Id",
                table: "TableOperators",
                column: "Table Id",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableOperators_Operators_Operator Id",
                table: "TableOperators");

            migrationBuilder.DropForeignKey(
                name: "FK_TableOperators_Tables_Table Id",
                table: "TableOperators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TableOperators",
                table: "TableOperators");

            migrationBuilder.RenameTable(
                name: "TableOperators",
                newName: "TableOperator");

            migrationBuilder.RenameIndex(
                name: "IX_TableOperators_Table Id",
                table: "TableOperator",
                newName: "IX_TableOperator_Table Id");

            migrationBuilder.RenameIndex(
                name: "IX_TableOperators_Operator Id",
                table: "TableOperator",
                newName: "IX_TableOperator_Operator Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TableOperator",
                table: "TableOperator",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TableOperator_Operators_Operator Id",
                table: "TableOperator",
                column: "Operator Id",
                principalTable: "Operators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TableOperator_Tables_Table Id",
                table: "TableOperator",
                column: "Table Id",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTableOperatorConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableOperator_Operators_OperatorId",
                table: "TableOperator");

            migrationBuilder.DropForeignKey(
                name: "FK_TableOperator_Tables_TableId",
                table: "TableOperator");

            migrationBuilder.RenameColumn(
                name: "TableId",
                table: "TableOperator",
                newName: "Table Id");

            migrationBuilder.RenameColumn(
                name: "OperatorId",
                table: "TableOperator",
                newName: "Operator Id");

            migrationBuilder.RenameIndex(
                name: "IX_TableOperator_TableId",
                table: "TableOperator",
                newName: "IX_TableOperator_Table Id");

            migrationBuilder.RenameIndex(
                name: "IX_TableOperator_OperatorId",
                table: "TableOperator",
                newName: "IX_TableOperator_Operator Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableOperator_Operators_Operator Id",
                table: "TableOperator");

            migrationBuilder.DropForeignKey(
                name: "FK_TableOperator_Tables_Table Id",
                table: "TableOperator");

            migrationBuilder.RenameColumn(
                name: "Table Id",
                table: "TableOperator",
                newName: "TableId");

            migrationBuilder.RenameColumn(
                name: "Operator Id",
                table: "TableOperator",
                newName: "OperatorId");

            migrationBuilder.RenameIndex(
                name: "IX_TableOperator_Table Id",
                table: "TableOperator",
                newName: "IX_TableOperator_TableId");

            migrationBuilder.RenameIndex(
                name: "IX_TableOperator_Operator Id",
                table: "TableOperator",
                newName: "IX_TableOperator_OperatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TableOperator_Operators_OperatorId",
                table: "TableOperator",
                column: "OperatorId",
                principalTable: "Operators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TableOperator_Tables_TableId",
                table: "TableOperator",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

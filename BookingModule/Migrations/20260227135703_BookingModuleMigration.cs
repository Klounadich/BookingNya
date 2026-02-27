using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingModule.Migrations
{
    /// <inheritdoc />
    public partial class BookingModuleMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Bookings",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "ProcessedCommands",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    saga_id = table.Column<Guid>(type: "uuid", nullable: false),
                    command_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    command_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    result_payload = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    error_message = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedCommands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SagaEventLogs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    saga_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    event_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    level = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    payload = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    source_module = table.Column<string>(type: "text", nullable: false),
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    occurred_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SagaEventLogs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SagaStates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    saga_id = table.Column<Guid>(type: "uuid", nullable: false),
                    saga_type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    current_step = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    retry_count = table.Column<int>(type: "integer", nullable: false),
                    max_retries = table.Column<int>(type: "integer", nullable: false),
                    error_message = table.Column<string>(type: "text", nullable: true),
                    error_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    metadata = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SagaStates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SagaSteps",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    saga_id = table.Column<Guid>(type: "uuid", nullable: false),
                    step_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    step_order = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    compensated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_compensated = table.Column<bool>(type: "boolean", nullable: false),
                    error_message = table.Column<string>(type: "text", nullable: true),
                    error_code = table.Column<string>(type: "text", nullable: true),
                    request_payload = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    responce_payload = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SagaSteps", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedCommands_saga_id",
                table: "ProcessedCommands",
                column: "saga_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SagaEventLogs_saga_id",
                table: "SagaEventLogs",
                column: "saga_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SagaStates_saga_id",
                table: "SagaStates",
                column: "saga_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SagaSteps_saga_id",
                table: "SagaSteps",
                column: "saga_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessedCommands");

            migrationBuilder.DropTable(
                name: "SagaEventLogs");

            migrationBuilder.DropTable(
                name: "SagaStates");

            migrationBuilder.DropTable(
                name: "SagaSteps");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "Bookings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");
        }
    }
}

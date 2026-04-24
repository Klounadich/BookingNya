using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingModule.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdInBookingsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessedCommands");

            migrationBuilder.DropTable(
                name: "SagaEventLogs");

            migrationBuilder.DropTable(
                name: "SagaSteps");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "Bookings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_id",
                table: "Bookings");

            migrationBuilder.CreateTable(
                name: "ProcessedCommands",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    command_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    command_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    error_message = table.Column<string>(type: "text", nullable: true),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    result_payload = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    saga_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false)
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
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    event_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    event_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    level = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    occurred_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    payload = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    saga_id = table.Column<Guid>(type: "uuid", nullable: false),
                    source_module = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SagaEventLogs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SagaSteps",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    compensated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    error_code = table.Column<string>(type: "text", nullable: true),
                    error_message = table.Column<string>(type: "text", nullable: true),
                    is_compensated = table.Column<bool>(type: "boolean", nullable: false),
                    request_payload = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    responce_payload = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    saga_id = table.Column<Guid>(type: "uuid", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    step_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    step_order = table.Column<int>(type: "integer", nullable: false)
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
                name: "IX_SagaSteps_saga_id",
                table: "SagaSteps",
                column: "saga_id",
                unique: true);
        }
    }
}

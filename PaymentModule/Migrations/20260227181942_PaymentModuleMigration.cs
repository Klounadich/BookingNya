using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentModule.Migrations
{
    /// <inheritdoc />
    public partial class PaymentModuleMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    saga_id = table.Column<Guid>(type: "uuid", nullable: true),
                    booking_id = table.Column<Guid>(type: "uuid", nullable: true),
                    transaction_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    reservation_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    payment_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    payment_gateway = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    customer_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    customer_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    customer_phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    gateway_response = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    failure_reason = table.Column<string>(type: "text", nullable: true),
                    authorized_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    captured_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    refunded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    metadata = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsMethods",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    payment_method_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    provider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    provider_token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    is_verified = table.Column<bool>(type: "boolean", nullable: false),
                    card_last4 = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    card_brand = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    card_expiry_month = table.Column<int>(type: "integer", nullable: true),
                    card_expiry_year = table.Column<int>(type: "integer", nullable: true),
                    billing_address = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    failure_count = table.Column<int>(type: "integer", nullable: false),
                    disabled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    metadata = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsMethods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    payment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    saga_id = table.Column<Guid>(type: "uuid", nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    gateway_transaction_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    gateway_response = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    failure_reason = table.Column<string>(type: "text", nullable: true),
                    parent_transaction_id = table.Column<Guid>(type: "uuid", nullable: true),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    metadata = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PaymentsMethods");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");
        }
    }
}

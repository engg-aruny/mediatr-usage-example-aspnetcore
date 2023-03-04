using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MediatR.Usage.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "ID", "DateOfBirth", "Email", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 3, 17, 34, 43, 874, DateTimeKind.Local).AddTicks(4395), "testmohit@gmail.com", "Mohit", "Yadav" },
                    { 2, new DateTime(2023, 3, 3, 17, 34, 43, 874, DateTimeKind.Local).AddTicks(4406), "testankitsharma@gmail.com", "Ankit", "Sharma" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "ID", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "testramesh@gmail.com", "Ramesh", "Kumar", "1234567890" },
                    { 2, "testamitsharma@gmail.com", "Amit ", "Sharma", "1234517890" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "ID",
                keyValue: 2);
        }
    }
}

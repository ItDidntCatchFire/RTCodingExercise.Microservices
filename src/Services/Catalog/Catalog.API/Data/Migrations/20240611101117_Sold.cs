﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Migrations
{
    public partial class Sold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReserved",
                table: "Plates");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Plates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Plates");

            migrationBuilder.AddColumn<bool>(
                name: "IsReserved",
                table: "Plates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

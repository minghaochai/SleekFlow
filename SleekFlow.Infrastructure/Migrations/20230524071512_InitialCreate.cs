﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SleekFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    DueAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AddAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    AddBy = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    EditAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    EditBy = table.Column<string>(type: "NVARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDos");
        }
    }
}

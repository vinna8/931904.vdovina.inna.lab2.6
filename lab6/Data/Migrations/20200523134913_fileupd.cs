using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
 
namespace lab6.Data.Migrations
{
    public partial class fileupd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Files");

            migrationBuilder.AddColumn<byte[]>(
                name: "file",
                table: "Files",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Files",
                nullable: false,
                defaultValue: "");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using ShpaginApp.Models;

#nullable disable

namespace ShpaginApp.Migrations
{
  /// <inheritdoc />
  public partial class Firstmigration : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterDatabase()
          .Annotation("Npgsql:Enum:public.age_rating", "EIGHTEEN,SIX,SIXTEEN,TWELVE,ZERO")
          .Annotation("Npgsql:Enum:public.book_status", "COMPLETED,READING,WANT_TO_READ");

      migrationBuilder.CreateTable(
          name: "authors",
          columns: table => new
          {
            id = table.Column<Guid>(type: "uuid", nullable: false),
            name = table.Column<string>(type: "varchar(255)", nullable: false),
            description = table.Column<string>(type: "text", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_authors", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "books",
          columns: table => new
          {
            id = table.Column<Guid>(type: "uuid", nullable: false),
            name = table.Column<string>(type: "varchar(255)", nullable: false),
            year = table.Column<int>(type: "integer", nullable: false),
            age_rating = table.Column<RussianAgeRatingEnum>(type: "age_rating", nullable: false),
            publisher = table.Column<string>(type: "varchar(255)", nullable: true),
            annotation = table.Column<string>(type: "text", nullable: true),
            created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
            updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_books", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "tags",
          columns: table => new
          {
            id = table.Column<Guid>(type: "uuid", nullable: false),
            value = table.Column<string>(type: "varchar(64)", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_tags", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "users",
          columns: table => new
          {
            id = table.Column<Guid>(type: "uuid", nullable: false),
            is_admin = table.Column<bool>(type: "bool", nullable: false),
            username = table.Column<string>(type: "varchar(32)", nullable: false),
            password_hash = table.Column<string>(type: "varchar(44)", nullable: false),
            email = table.Column<string>(type: "varchar(255)", nullable: false),
            display_name = table.Column<string>(type: "varchar(32)", nullable: true),
            birth_date = table.Column<DateOnly>(type: "date", nullable: true),
            created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
            updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_users", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "books_authors",
          columns: table => new
          {
            book_id = table.Column<Guid>(type: "uuid", nullable: false),
            author_id = table.Column<Guid>(type: "uuid", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_books_authors", x => new { x.book_id, x.author_id });
            table.ForeignKey(
                      name: "fk_books_authors_author_id_authors",
                      column: x => x.author_id,
                      principalTable: "authors",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "fk_books_authors_book_id_books",
                      column: x => x.book_id,
                      principalTable: "books",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "books_tags",
          columns: table => new
          {
            book_id = table.Column<Guid>(type: "uuid", nullable: false),
            tag_id = table.Column<Guid>(type: "uuid", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_books_tags", x => new { x.book_id, x.tag_id });
            table.ForeignKey(
                      name: "fk_books_tags_book_id_books",
                      column: x => x.book_id,
                      principalTable: "books",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "fk_books_tags_tag_id_tags",
                      column: x => x.tag_id,
                      principalTable: "tags",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "users_books",
          columns: table => new
          {
            user_id = table.Column<Guid>(type: "uuid", nullable: false),
            book_id = table.Column<Guid>(type: "uuid", nullable: false),
            favorite = table.Column<bool>(type: "bool", nullable: true),
            rating = table.Column<int>(type: "integer", nullable: true),
            status = table.Column<BookStatusEnum>(type: "book_status", nullable: true),
            added_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
            updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_users_books", x => new { x.user_id, x.book_id });
            table.ForeignKey(
                      name: "fk_users_books_book_id_books",
                      column: x => x.book_id,
                      principalTable: "books",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "fk_users_books_user_id_users",
                      column: x => x.user_id,
                      principalTable: "users",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_books_authors_author_id",
          table: "books_authors",
          column: "author_id");

      migrationBuilder.CreateIndex(
          name: "IX_books_tags_tag_id",
          table: "books_tags",
          column: "tag_id");

      migrationBuilder.CreateIndex(
          name: "uq_users_email",
          table: "users",
          column: "email",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "uq_users_username",
          table: "users",
          column: "username",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_users_books_book_id",
          table: "users_books",
          column: "book_id");

      migrationBuilder.Sql(@"
        CREATE OR REPLACE FUNCTION update_updated_at_column()
        RETURNS TRIGGER AS $$
        BEGIN
          NEW.updated_at = now();
          RETURN NEW;
        END;
        $$ LANGUAGE 'plpgsql';
      ");

      migrationBuilder.Sql(@"
        CREATE TRIGGER update_books_updated_at
        BEFORE UPDATE ON ""books""
        FOR EACH ROW
        EXECUTE FUNCTION update_updated_at_column();
      ");

      migrationBuilder.Sql(@"
        CREATE TRIGGER update_users_updated_at
        BEFORE UPDATE ON ""users""
        FOR EACH ROW
        EXECUTE FUNCTION update_updated_at_column();
      ");

      migrationBuilder.Sql(@"
        CREATE TRIGGER update_users_books_updated_at
        BEFORE UPDATE ON ""users_books""
        FOR EACH ROW
        EXECUTE FUNCTION update_updated_at_column();
      ");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.Sql(@"
        DROP TRIGGER IF EXISTS update_users_books_updated_at ON ""users_books"";
      ");

      migrationBuilder.Sql(@"
        DROP TRIGGER IF EXISTS update_users_updated_at ON ""users"";
      ");

      migrationBuilder.Sql(@"
        DROP TRIGGER IF EXISTS update_books_updated_at ON ""book"";
      ");

      migrationBuilder.Sql(@"
        DROP FUNCTION IF EXISTS update_updated_at_column();
      ");

      migrationBuilder.DropTable(
          name: "books_authors");

      migrationBuilder.DropTable(
          name: "books_tags");

      migrationBuilder.DropTable(
          name: "users_books");

      migrationBuilder.DropTable(
          name: "authors");

      migrationBuilder.DropTable(
          name: "tags");

      migrationBuilder.DropTable(
          name: "books");

      migrationBuilder.DropTable(
          name: "users");
    }
  }
}

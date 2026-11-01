using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chincisan_Laura_Lab2.Migrations
{
    public partial class AddAuthors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // cream tabelul pentru autori
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    FirstName = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: false),

                    LastName = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.ID);
                });

            // adaugam cheia straina in tabelul Book
            migrationBuilder.AddColumn<int>(
                name: "AuthorsID",
                table: "Book",
                type: "int",
                nullable: true);

            // transferam autorii existenti din backup
            migrationBuilder.Sql(@"
                INSERT INTO Authors (FirstName, LastName)
                SELECT DISTINCT
                    CASE
                        WHEN CHARINDEX(' ', LTRIM(RTRIM(Author))) > 0
                        THEN LEFT(
                            LTRIM(RTRIM(Author)),
                            CHARINDEX(' ', LTRIM(RTRIM(Author))) - 1
                        )
                        ELSE ''
                    END,
                    CASE
                        WHEN CHARINDEX(' ', LTRIM(RTRIM(Author))) > 0
                        THEN SUBSTRING(
                            LTRIM(RTRIM(Author)),
                            CHARINDEX(' ', LTRIM(RTRIM(Author))) + 1,
                            LEN(Author)
                        )
                        ELSE LTRIM(RTRIM(Author))
                    END
                FROM BookAuthorsBackup
                WHERE Author IS NOT NULL
                  AND LTRIM(RTRIM(Author)) <> '';
            ");

            // asociem fiecare carte cu autorul ei
            migrationBuilder.Sql(@"
                UPDATE b
                SET b.AuthorsID = a.ID
                FROM Book AS b
                INNER JOIN BookAuthorsBackup AS bk
                    ON b.ID = bk.ID
                INNER JOIN Authors AS a
                    ON CONCAT(
                        a.FirstName,
                        CASE
                            WHEN a.FirstName <> '' THEN ' '
                            ELSE ''
                        END,
                        a.LastName
                    ) = LTRIM(RTRIM(bk.Author));
            ");

            // cream indexul pentru cheia straina
            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorsID",
                table: "Book",
                column: "AuthorsID");

            // cream relatia dintre Book si Authors
            migrationBuilder.AddForeignKey(
                name: "FK_Book_Authors_AuthorsID",
                table: "Book",
                column: "AuthorsID",
                principalTable: "Authors",
                principalColumn: "ID");

            // stergem vechea coloana Author
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Book");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // readaugam vechea coloana Author
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // reconstruim numele autorilor
            migrationBuilder.Sql(@"
                UPDATE b
                SET b.Author = LTRIM(RTRIM(
                    CONCAT(a.FirstName, ' ', a.LastName)
                ))
                FROM Book AS b
                INNER JOIN Authors AS a
                    ON b.AuthorsID = a.ID;
            ");

            // stergem relatia dintre Book si Authors
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Authors_AuthorsID",
                table: "Book");

            // stergem indexul
            migrationBuilder.DropIndex(
                name: "IX_Book_AuthorsID",
                table: "Book");

            // stergem cheia straina
            migrationBuilder.DropColumn(
                name: "AuthorsID",
                table: "Book");

            // stergem tabelul Authors
            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
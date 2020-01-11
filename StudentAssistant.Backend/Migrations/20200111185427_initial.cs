using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentAssistant.Backend.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeacherModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NumberWeekModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NumberWeek = table.Column<int>(nullable: false),
                    CourseScheduleDatabaseModelId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberWeekModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudyGroupModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CourseScheduleDatabaseModelId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyGroupModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseScheduleDatabaseModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParityWeek = table.Column<bool>(nullable: false),
                    NameOfDayWeek = table.Column<string>(nullable: true),
                    CourseName = table.Column<string>(nullable: true),
                    CourseNumber = table.Column<int>(nullable: false),
                    CourseType = table.Column<int>(nullable: false),
                    TeacherModelId = table.Column<Guid>(nullable: true),
                    CoursePlace = table.Column<string>(nullable: true),
                    StudyGroupModelId = table.Column<Guid>(nullable: true),
                    StartOfClasses = table.Column<string>(nullable: true),
                    EndOfClasses = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseScheduleDatabaseModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseScheduleDatabaseModels_StudyGroupModel_StudyGroupModelId",
                        column: x => x.StudyGroupModelId,
                        principalTable: "StudyGroupModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseScheduleDatabaseModels_TeacherModel_TeacherModelId",
                        column: x => x.TeacherModelId,
                        principalTable: "TeacherModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamScheduleDatabaseModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CourseName = table.Column<string>(nullable: true),
                    Month = table.Column<string>(nullable: true),
                    NumberDate = table.Column<string>(nullable: true),
                    DayOfWeek = table.Column<string>(nullable: true),
                    CourseType = table.Column<int>(nullable: false),
                    TeacherModelId = table.Column<Guid>(nullable: true),
                    CoursePlace = table.Column<string>(nullable: true),
                    StudyGroupModelId = table.Column<Guid>(nullable: true),
                    StartOfClasses = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamScheduleDatabaseModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamScheduleDatabaseModels_StudyGroupModel_StudyGroupModelId",
                        column: x => x.StudyGroupModelId,
                        principalTable: "StudyGroupModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamScheduleDatabaseModels_TeacherModel_TeacherModelId",
                        column: x => x.TeacherModelId,
                        principalTable: "TeacherModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CourseScheduleDatabaseModels_StudyGroupModelId",
                table: "CourseScheduleDatabaseModels",
                column: "StudyGroupModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseScheduleDatabaseModels_TeacherModelId",
                table: "CourseScheduleDatabaseModels",
                column: "TeacherModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamScheduleDatabaseModels_StudyGroupModelId",
                table: "ExamScheduleDatabaseModels",
                column: "StudyGroupModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamScheduleDatabaseModels_TeacherModelId",
                table: "ExamScheduleDatabaseModels",
                column: "TeacherModelId");

            migrationBuilder.CreateIndex(
                name: "IX_NumberWeekModel_CourseScheduleDatabaseModelId",
                table: "NumberWeekModel",
                column: "CourseScheduleDatabaseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyGroupModel_CourseScheduleDatabaseModelId",
                table: "StudyGroupModel",
                column: "CourseScheduleDatabaseModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_NumberWeekModel_CourseScheduleDatabaseModels_CourseScheduleDatabaseModelId",
                table: "NumberWeekModel",
                column: "CourseScheduleDatabaseModelId",
                principalTable: "CourseScheduleDatabaseModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroupModel_CourseScheduleDatabaseModels_CourseScheduleDatabaseModelId",
                table: "StudyGroupModel",
                column: "CourseScheduleDatabaseModelId",
                principalTable: "CourseScheduleDatabaseModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseScheduleDatabaseModels_StudyGroupModel_StudyGroupModelId",
                table: "CourseScheduleDatabaseModels");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ExamScheduleDatabaseModels");

            migrationBuilder.DropTable(
                name: "NumberWeekModel");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "StudyGroupModel");

            migrationBuilder.DropTable(
                name: "CourseScheduleDatabaseModels");

            migrationBuilder.DropTable(
                name: "TeacherModel");
        }
    }
}

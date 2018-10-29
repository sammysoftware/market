
CREATE TABLE "AspNetRoles" (
    "Id"               NVARCHAR (450) NOT NULL,
    "ConcurrencyStamp" NVARCHAR (MAX) NULL,
    "Name"             NVARCHAR (256) NULL,
    "NormalizedName"   NVARCHAR (256) NULL,
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY CLUSTERED ("Id" ASC)
);

GO
CREATE NONCLUSTERED INDEX "RoleNameIndex"
    ON "AspNetRoles"("NormalizedName" ASC);


CREATE TABLE "AspNetRoleClaims" (
    "Id"         INT            IDENTITY (1, 1) NOT NULL,
    "ClaimType"  NVARCHAR (MAX) NULL,
    "ClaimValue" NVARCHAR (MAX) NULL,
    "RoleId"     NVARCHAR (450) NOT NULL,
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY CLUSTERED ("Id" ASC),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX "IX_AspNetRoleClaims_RoleId"
    ON "AspNetRoleClaims"("RoleId" ASC);

CREATE TABLE "AspNetUsers" (
    "Id"                   NVARCHAR (450)     NOT NULL,
    "AccessFailedCount"    INT                NOT NULL,
    "ConcurrencyStamp"     NVARCHAR (MAX)     NULL,
    "Email"                NVARCHAR (256)     NULL,
    "EmailConfirmed"       BIT                NOT NULL,
    "LockoutEnabled"       BIT                NOT NULL,
    "LockoutEnd"           DATETIMEOFFSET (7) NULL,
    "NormalizedEmail"      NVARCHAR (256)     NULL,
    "NormalizedUserName"   NVARCHAR (256)     NULL,
    "PasswordHash"         NVARCHAR (MAX)     NULL,
    "PhoneNumber"          NVARCHAR (MAX)     NULL,
    "PhoneNumberConfirmed" BIT                NOT NULL,
    "SecurityStamp"        NVARCHAR (MAX)     NULL,
    "TwoFactorEnabled"     BIT                NOT NULL,
    "UserName"             NVARCHAR (256)     NULL,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY CLUSTERED ("Id" ASC)
);


GO
CREATE NONCLUSTERED INDEX "EmailIndex"
    ON "AspNetUsers"("NormalizedEmail" ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX "UserNameIndex"
    ON "AspNetUsers"("NormalizedUserName" ASC);


CREATE TABLE "AspNetUserClaims" (
    "Id"         INT            IDENTITY (1, 1) NOT NULL,
    "ClaimType"  NVARCHAR (MAX) NULL,
    "ClaimValue" NVARCHAR (MAX) NULL,
    "UserId"     NVARCHAR (450) NOT NULL,
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY CLUSTERED ("Id" ASC),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX "IX_AspNetUserClaims_UserId"
    ON "AspNetUserClaims"("UserId" ASC);


CREATE TABLE "AspNetUserLogins" (
    "LoginProvider"       NVARCHAR (450) NOT NULL,
    "ProviderKey"         NVARCHAR (450) NOT NULL,
    "ProviderDisplayName" NVARCHAR (MAX) NULL,
    "UserId"              NVARCHAR (450) NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY CLUSTERED ("LoginProvider" ASC, "ProviderKey" ASC),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX "IX_AspNetUserLogins_UserId"
    ON "AspNetUserLogins"("UserId" ASC);


CREATE TABLE "AspNetUserRoles" (
    "UserId" NVARCHAR (450) NOT NULL,
    "RoleId" NVARCHAR (450) NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY CLUSTERED ("UserId" ASC, "RoleId" ASC),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX "IX_AspNetUserRoles_RoleId"
    ON "AspNetUserRoles"("RoleId" ASC);


GO
CREATE NONCLUSTERED INDEX "IX_AspNetUserRoles_UserId"
    ON "AspNetUserRoles"("UserId" ASC);

CREATE TABLE "AspNetUserTokens" (
    "UserId"        NVARCHAR (450) NOT NULL,
    "LoginProvider" NVARCHAR (450) NOT NULL,
    "Name"          NVARCHAR (450) NOT NULL,
    "Value"         NVARCHAR (MAX) NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY CLUSTERED ("UserId" ASC, "LoginProvider" ASC, "Name" ASC)
);






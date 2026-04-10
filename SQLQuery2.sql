
UPDATE AspNetRoles SET NormalizedName = 'ADMIN' WHERE Name = 'admin';
UPDATE AspNetUsers SET NormalizedEmail = UPPER(Email), NormalizedUserName = UPPER(UserName);
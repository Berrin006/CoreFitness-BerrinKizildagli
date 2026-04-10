-- 1. AspNetUsers tablosunda ConcurrencyStamp kolonu var mı kontrol et, yoksa ekle
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'ConcurrencyStamp')
BEGIN
    ALTER TABLE [dbo].[AspNetUsers] ADD [ConcurrencyStamp] NVARCHAR(MAX) NULL;
END

-- 2. Eğer NormalizedEmail veya NormalizedUserName gibi diğer kritik kolonlar da eksikse onları da tamamlayalım
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'NormalizedUserName')
BEGIN
    ALTER TABLE [dbo].[AspNetUsers] ADD [NormalizedUserName] NVARCHAR(256) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'NormalizedEmail')
BEGIN
    ALTER TABLE [dbo].[AspNetUsers] ADD [NormalizedEmail] NVARCHAR(256) NULL;
END

-- 3. AspNetRoles tablosunda da ConcurrencyStamp eksik olabilir, onu da garantiye alalım
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND name = 'ConcurrencyStamp')
BEGIN
    ALTER TABLE [dbo].[AspNetRoles] ADD [ConcurrencyStamp] NVARCHAR(MAX) NULL;
END
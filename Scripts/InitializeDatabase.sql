DROP TABLE IF EXISTS hashs;
DROP TABLE IF EXISTS users;

DROP PROCEDURE IF EXISTS CreateAccount;
DROP PROCEDURE IF EXISTS GetUserHash;
DROP PROCEDURE IF EXISTS Authenticate;

Create Table  users (
    user_account_id BIGINT IDENTITY(1,1) NOT NULL,
    username VARCHAR(128),
    passphrase VARCHAR(128) NOT NULL,
    email VARCHAR(128) UNIQUE NOT NULL,
    first_name VARCHAR(128),
    last_name VARCHAR(128), 
    authorization_level VARCHAR(50) NOT NULL,
    enabled_status bit NOT NULL,
    confirmed bit NOT NULL,
	PRIMARY KEY(user_account_id)
  );

CREATE TABLE hashs (
	user_account_id BIGINT NULL,
	user_hash VARCHAR(128) PRIMARY KEY,
	CONSTRAINT hash_fk FOREIGN KEY(user_account_id) REFERENCES users(user_account_id)
);

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateAccount]
(
	@email VARCHAR(128),
	@passphrase VARCHAR(128),
	@authorization_level VARCHAR(50),
	@enabled bit,
	@confirmed bit,
	@user_hash VARCHAR(128)
)
AS
BEGIN
	DECLARE @account_id BIGINT;
	INSERT INTO users (email, passphrase, authorization_level, enabled_status, confirmed) VALUES (@email, @passphrase, @authorization_level, @enabled, @confirmed);
	SET @account_id = @@IDENTITY;
	INSERT INTO hashs (user_account_id, user_hash) VALUES (@account_id, @user_hash);
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserHash]
(
	@email VARCHAR(128),
	@Result VARCHAR(128) OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY;
		SET @Result = (SELECT user_hash FROM hashs WHERE user_account_id = (SELECT user_account_id FROM users WHERE email = @email));
		IF(@Result  IS NOT NULL)
			RETURN 1;
	END TRY
	BEGIN CATCH
		RETURN 0;
	END CATCH;
	RETURN 0;
END


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Authenticate]
(
	@email VARCHAR(128),
	@Result VARCHAR(128) OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY;
		SET @Result = (SELECT user_hash FROM hashs WHERE user_account_id = (SELECT user_account_id FROM users WHERE email = @email));
		IF(@Result  IS NOT NULL)
			RETURN 1;
	END TRY
	BEGIN CATCH
		RETURN 0;
	END CATCH;
	RETURN 0;
END
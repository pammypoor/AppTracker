DROP TABLE IF EXISTS hashs;
DROP TABLE IF EXISTS users;

DROP PROCEDURE IF EXISTS CreateAccount;
DROP PROCEDURE IF EXISTS GetUserHash;
DROP PROCEDURE IF EXISTS Authenticate;
DROP PROCEDURE IF EXISTS VerifyAccount;

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
			RETURN @Result;
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
CREATE PROCEDURE dbo.[Authenticate] 
	-- Add the parameters for the stored procedure here
	@Email VARCHAR(128),
    @Passphrase VARCHAR(128),
	@Result int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		SET @Result = (SELECT COALESCE(
				(SELECT CASE 
						WHEN CAST(passphrase as BINARY) = CAST(@Passphrase as BINARY) THEN 1
						ELSE 2
					END AS Result
					FROM users
					WHERE email = @Email), 0
				)
			)
	RETURN @Result;
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.[VerifyAccount] 
	-- Add the parameters for the stored procedure here
	@Email VARCHAR(128),
    @AuthorizationLevel VARCHAR(40),
	@Result int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TranCounter int;
	SET @TranCounter = @@TRANCOUNT;
	IF @TranCounter > 0
		SAVE TRANSACTION ProcedureSave
	ELSE
		BEGIN TRAN
			BEGIN TRY;
				-- Insert statements for procedure here 

				-- 0 = No Account found, 1 = Success, 2 = Not Confirmed, 3 = Not Enabled, 4 = Rollback occurred
				SET @Result = (SELECT COALESCE(
					(SELECT CASE 
						WHEN confirmed = 1 AND enabled_status = 1 THEN 1
						WHEN confirmed = 0 AND enabled_status = 0 THEN 2
						WHEN confirmed = 1 AND enabled_status = 0 THEN 3
					END AS Verified
					FROM users
					WHERE email = @Email AND authorization_level = @AuthorizationLevel), 0)
				)
				COMMIT TRANSACTION
			END TRY
			BEGIN CATCH
			IF @TranCounter = 0
				BEGIN
					SET @Result = 4
					ROLLBACK TRANSACTION
				END
			ELSE
				IF XACT_STATE() <> -1
					BEGIN
						SET @Result = 4
						ROLLBACK TRANSACTION ProcedureSave
					END
			END CATCH
	RETURN @Result;
END

SELECT * FROM hashs
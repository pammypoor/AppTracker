DROP TABLE IF EXISTS applications;
DROP TABLE IF EXISTS hashs;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS responses;

DROP PROCEDURE IF EXISTS CreateAccount;
DROP PROCEDURE IF EXISTS GetUserHash;
DROP PROCEDURE IF EXISTS Authenticate;
DROP PROCEDURE IF EXISTS VerifyAccount;
DROP PROCEDURE IF EXISTS GetMessage;
DROP PROCEDURE IF EXISTS CreateApplication;

CREATE TABLE dbo.[responses](
	response_id BIGINT IDENTITY(1,1) NOT NULL,
	response VARCHAR(100) NOT NULL,
	response_message VARCHAR(150) NOT NULL,
	code INT NOT NULL
);

CREATE TABLE  dbo.[users] (
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

CREATE TABLE dbo.[hashs] (
	user_account_id BIGINT NULL,
	user_hash VARCHAR(128) PRIMARY KEY,
	CONSTRAINT hash_fk FOREIGN KEY(user_account_id) REFERENCES users(user_account_id)
);

CREATE TABLE dbo.[applications] (
	application_id BIGINT IDENTITY(1,1) NOT NULL,
	user_hash VARCHAR(128) NOT NULL,
	submission_datetime DATETIME,
	company VARCHAR(150) NOT NULL,
	position VARCHAR(150) NOT NULL,
	application_type VARCHAR(50),
	application_status VARCHAR(100),
	link VARCHAR(200),
	company_state VARCHAR(100),
	company_city VARCHAR(100),
	company_country VARCHAR(150),
	application_description VARCHAR(750),
	is_remote bit,
	deleted bit,
	CONSTRAINT applications_fk FOREIGN KEY (user_hash) REFERENCES hashs(user_hash)
);

INSERT INTO responses (response, response_message, code) VALUES
	('success', 'Success', 200),
	('unhandledException', 'Unhandeled Exception', 500),
	('operationCancelled', 'Operation Cancelled', 500),
	('operationTimeExceeded', 'Operation Time Limit Exceeded', 500),
	('principalNotSet', 'Principal was not set correctly.', 500),
	('databaseConnectionFail', 'Unable to connect to database', 503),
	('accountVerificationSuccess', 'Account verified', 200),
	('accountVerificationFail', 'Cannot Verify Account', 500),
	('accountNotEnabled', 'Account not enabled', 401),
	('accountNotConfirmed', 'Account not confirmed', 401),
	('accountNotFound', 'Account not found', 404),
	('authenticationSuccess', 'Account authenticated', 200),
	('getUserHashSuccess', 'User hash retrieved', 200),
	('invalidPassword', 'Invalid Password', 401),
	('tokenRefreshSuccess', 'JWT Token Refreshed', 200),
	('alreadyAuthenticated', 'User Is Already Authenticated', 401),
	('notAuthenticated', 'User Not Authenticated', 401),
	('invalidApplication', 'Invalid Application', 400),
	('notAuthorized', 'Not Authorized', 403),
	('unknownRole', 'Unknown Role', 400);
	

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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.[GetMessage]
(
	@Response VARCHAR(100),
	@ResultMessage VARCHAR(150) OUTPUT, 
	@ResultCode INT OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY;
		SET @ResultMessage = (SELECT response_message FROM responses WHERE response = @Response);
		IF(@ResultMessage  IS NOT NULL)
			SET @ResultCode = (SELECT code FROM responses WHERE response = @Response);
		ELSE
			RETURN @ResultMessage;
	END TRY
	BEGIN CATCH
		RETURN 0;
	END CATCH;
	RETURN 1;
END


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateApplication]
(
	@UserHash VARCHAR(128),
	@SubmissionDateTime DATETIME,
	@Company VARCHAR(150),
	@Position VARCHAR(150),
	@Type VARCHAR(50),
	@Status VARCHAR(100),
	@Link VARCHAR(200),
	@State VARCHAR(100),
	@City VARCHAR(100),
	@Country VARCHAR(150),
	@Description VARCHAR(750),
	@IsRemote BIT,
	@Deleted BIT
)
AS
BEGIN
	INSERT INTO applications (user_hash, submission_datetime, company, position, application_type, application_status, link, company_state, company_city, company_country, application_description, is_remote, deleted)
		VALUES(@UserHash, @SubmissionDateTime, @Company, @Position, @Type, @Status, @Link, @State, @City, @Country, @Description, @IsRemote, @Deleted);
END
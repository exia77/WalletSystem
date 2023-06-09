DROP DATABASE IF EXISTS wallet
GO
CREATE DATABASE wallet
GO
USE [wallet]
GO
/****** Object:  StoredProcedure [dbo].[Users_SetNewUsers]    Script Date: 4/23/2023 9:04:19 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[Users_SetNewUsers]
GO
/****** Object:  StoredProcedure [dbo].[Users_GetVerifyUsers]    Script Date: 4/23/2023 9:04:19 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[Users_GetVerifyUsers]
GO
/****** Object:  StoredProcedure [dbo].[Users_GetTransactionHistory]    Script Date: 4/23/2023 9:04:19 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[Users_GetTransactionHistory]
GO
/****** Object:  StoredProcedure [dbo].[Users_CheckUserExist]    Script Date: 4/23/2023 9:04:19 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[Users_CheckUserExist]
GO
/****** Object:  StoredProcedure [dbo].[Transaction_SetUpdateTransaction]    Script Date: 4/23/2023 9:04:19 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[Transaction_SetUpdateTransaction]
GO
/****** Object:  StoredProcedure [dbo].[Transaction_CheckAccount]    Script Date: 4/23/2023 9:04:19 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[Transaction_CheckAccount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransactionHistory]') AND type in (N'U'))
ALTER TABLE [dbo].[TransactionHistory] DROP CONSTRAINT IF EXISTS [FK_TransactionHistory_AccountsTo]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransactionHistory]') AND type in (N'U'))
ALTER TABLE [dbo].[TransactionHistory] DROP CONSTRAINT IF EXISTS [FK_TransactionHistory_AccountFrom]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounts]') AND type in (N'U'))
ALTER TABLE [dbo].[Accounts] DROP CONSTRAINT IF EXISTS [FK_Accounts_Users]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
ALTER TABLE [dbo].[Users] DROP CONSTRAINT IF EXISTS [DF_Users_RegisterDate]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransactionHistory]') AND type in (N'U'))
ALTER TABLE [dbo].[TransactionHistory] DROP CONSTRAINT IF EXISTS [DF_TransactionHistory_Id]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounts]') AND type in (N'U'))
ALTER TABLE [dbo].[Accounts] DROP CONSTRAINT IF EXISTS [DF_Accounts_UpdateDate]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounts]') AND type in (N'U'))
ALTER TABLE [dbo].[Accounts] DROP CONSTRAINT IF EXISTS [DF_Transactions_Id]
GO
/****** Object:  Index [UQ__Users__536C85E41A6C365E]    Script Date: 4/23/2023 9:04:19 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
ALTER TABLE [dbo].[Users] DROP CONSTRAINT IF EXISTS [UQ__Users__536C85E41A6C365E]
GO
/****** Object:  Index [UQ__Accounts__BE2ACD6F95F28088]    Script Date: 4/23/2023 9:04:19 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounts]') AND type in (N'U'))
ALTER TABLE [dbo].[Accounts] DROP CONSTRAINT IF EXISTS [UQ__Accounts__BE2ACD6F95F28088]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/23/2023 9:04:19 AM ******/
DROP TABLE IF EXISTS [dbo].[Users]
GO
/****** Object:  Table [dbo].[TransactionHistory]    Script Date: 4/23/2023 9:04:19 AM ******/
DROP TABLE IF EXISTS [dbo].[TransactionHistory]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 4/23/2023 9:04:19 AM ******/
DROP TABLE IF EXISTS [dbo].[Accounts]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 4/23/2023 9:04:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[AccountNumber] [varchar](12) NOT NULL,
	[Balance] [decimal](17, 2) NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionHistory]    Script Date: 4/23/2023 9:04:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionHistory](
	[Id] [uniqueidentifier] NOT NULL,
	[TransactionType] [int] NOT NULL,
	[AccountIdFrom] [uniqueidentifier] NOT NULL,
	[AccountIdTo] [uniqueidentifier] NOT NULL,
	[Amount] [decimal](17, 2) NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[EndBalance] [decimal](17, 2) NOT NULL,
 CONSTRAINT [PK_TransactionHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/23/2023 9:04:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[RegisterDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Accounts] ([Id], [UserId], [AccountNumber], [Balance], [UpdatedDate]) VALUES (N'f9cf9840-4046-4dc8-af22-b9e9cd1234c0', N'45a20359-6c48-4ff1-8630-76481ede9518', N'542502500676', CAST(0.00 AS Decimal(17, 2)), CAST(N'2023-04-22T19:00:42.313' AS DateTime))
INSERT [dbo].[Accounts] ([Id], [UserId], [AccountNumber], [Balance], [UpdatedDate]) VALUES (N'f74568d9-6cbd-47bc-b940-f32500c2e1cd', N'20442011-1175-4d31-bc7a-d1feaf59be1d', N'794540851215', CAST(0.00 AS Decimal(17, 2)), CAST(N'2023-04-22T19:00:34.420' AS DateTime))
GO
INSERT [dbo].[Users] ([Id], [Username], [Password], [RegisterDate]) VALUES (N'45a20359-6c48-4ff1-8630-76481ede9518', N'testUser1', N'1b4f0e9851971998e732078544c96b36c3d01cedf7caa33235', CAST(N'2023-04-22T19:00:42.313' AS DateTime))
INSERT [dbo].[Users] ([Id], [Username], [Password], [RegisterDate]) VALUES (N'20442011-1175-4d31-bc7a-d1feaf59be1d', N'testUser', N'9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd1', CAST(N'2023-04-22T19:00:34.420' AS DateTime))
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Accounts__BE2ACD6F95F28088]    Script Date: 4/23/2023 9:04:20 AM ******/
ALTER TABLE [dbo].[Accounts] ADD  CONSTRAINT [UQ__Accounts__BE2ACD6F95F28088] UNIQUE NONCLUSTERED 
(
	[AccountNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__536C85E41A6C365E]    Script Date: 4/23/2023 9:04:20 AM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Accounts] ADD  CONSTRAINT [DF_Transactions_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Accounts] ADD  CONSTRAINT [DF_Accounts_UpdateDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[TransactionHistory] ADD  CONSTRAINT [DF_TransactionHistory_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_RegisterDate]  DEFAULT (getdate()) FOR [RegisterDate]
GO
ALTER TABLE [dbo].[Accounts]  WITH NOCHECK ADD  CONSTRAINT [FK_Accounts_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_Users]
GO
ALTER TABLE [dbo].[TransactionHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_TransactionHistory_AccountFrom] FOREIGN KEY([AccountIdFrom])
REFERENCES [dbo].[Accounts] ([Id])
GO
ALTER TABLE [dbo].[TransactionHistory] CHECK CONSTRAINT [FK_TransactionHistory_AccountFrom]
GO
ALTER TABLE [dbo].[TransactionHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_TransactionHistory_AccountsTo] FOREIGN KEY([AccountIdTo])
REFERENCES [dbo].[Accounts] ([Id])
GO
ALTER TABLE [dbo].[TransactionHistory] CHECK CONSTRAINT [FK_TransactionHistory_AccountsTo]
GO
/****** Object:  StoredProcedure [dbo].[Transaction_CheckAccount]    Script Date: 4/23/2023 9:04:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Transaction_CheckAccount] 
	-- Add the parameters for the stored procedure here
	@AccountNo BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT [UpdatedDate],[Balance] FROM Accounts WHERE AccountNumber = @AccountNo
END
GO
/****** Object:  StoredProcedure [dbo].[Transaction_SetUpdateTransaction]    Script Date: 4/23/2023 9:04:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Transaction_SetUpdateTransaction] 
	-- Add the parameters for the stored procedure here
	@AccountNoFrom BIGINT,
	@AccountNoTo BIGINT = 0,
	@TransactionType INT,
	@Amount DECIMAL(17,2),
	@UpdatedDate Datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY 
		DECLARE @CheckUpdate DATETIME
		DECLARE @Balance DECIMAL(17,2)
		DECLARE @EndBalance DECIMAL(17,2)
		DECLARE @AccountFromID UNIQUEIDENTIFIER
		DECLARE @AccountToID UNIQUEIDENTIFIER
		DECLARE @TransactionDate DATETIME = GETDATE()
		IF @TransactionType = 0 OR @TransactionType = 1
			BEGIN
				SELECT @CheckUpdate = [UpdatedDate],
					   @Balance = [Balance],
					   @AccountFromID = [Id],
					   @AccountToID = [Id]
				FROM [Accounts] 
				WHERE [AccountNumber] = @AccountNoFrom
			END
		ELSE
			BEGIN
				SELECT @CheckUpdate = [UpdatedDate],
					   @Balance = [Balance],
					   @AccountFromID = [Id],
					   @AccountToID = (SELECT [Id] FROM [Accounts] WHERE [AccountNumber] = @AccountNoTo)
				FROM [Accounts] 
				WHERE [AccountNumber] = @AccountNoFrom
			END

		BEGIN TRANSACTION UpdateTransaction	
		
		IF @UpdatedDate != @CheckUpdate
		BEGIN
			RAISERROR ( 'Transaction was rolled back. %s', 16,18,  'UpdatedDate version not matched'); 
		END
			BEGIN
				
				SET @EndBalance = 
					CASE @TransactionType
						WHEN 0 
							THEN (SELECT @Balance+@Amount)
						ELSE
							(SELECT @Balance-@Amount)
					END

				IF @EndBalance < 0
					RAISERROR ('Error', 16,18,  'Not enough balance.'); 

				 UPDATE [Accounts]
				 SET [Balance] = @EndBalance,
				     [UpdatedDate] = (SELECT GETDATE())
				 WHERE [AccountNumber] = @AccountNoFrom

				 IF @TransactionType = 2
					BEGIN
					    DECLARE @AccountToBalance DECIMAL(17,2) = (SELECT Balance FROM [Accounts] WHERE AccountNumber = @AccountNoTo)
					    UPDATE [Accounts]
					    SET [Balance] = @AccountToBalance + @Amount,
				            [UpdatedDate] = (SELECT GETDATE())
				        WHERE [AccountNumber] = @AccountNoTo
					END

				 INSERT INTO [TransactionHistory]
				 (
					[Amount],
					[TransactionType],
					[AccountIdFrom],
					[AccountIdTo],
					[TransactionDate],
					[EndBalance]
				 )
				 VALUES
				 (
					@Amount,
					@TransactionType,
					@AccountFromID,
					@AccountToID,
					@TransactionDate,
					@EndBalance
				 )
			END
		COMMIT TRANSACTION UpdateTransaction

		SELECT @Balance AS 'Amount', @EndBalance AS 'EndBalance', @TransactionDate AS 'TransactionDate', @TransactionType AS 'TransactionType'
					
	END TRY
	
	BEGIN CATCH 
    
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION UpdateTransaction

		DECLARE @RC int
		DECLARE @ErrMessage varchar(200) 
		DECLARE @Severity BIGINT 
		DECLARE @State BIGINT

		SET @Severity = ERROR_SEVERITY()
		SET @State = ERROR_STATE()
		SET @ErrMessage = ERROR_MESSAGE()
		
		RAISERROR ( 'Transaction was rolled back. %s', -- Message text.  
				@Severity, -- Severity.  
				@State, -- State.  
				@ErrMessage);

	END CATCH 
END
GO
/****** Object:  StoredProcedure [dbo].[Users_CheckUserExist]    Script Date: 4/23/2023 9:04:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Users_CheckUserExist] 
	-- Add the parameters for the stored procedure here
	@Username NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
		IF EXISTS (SELECT [Username] FROM Users WHERE [Username] = @Username)
			SELECT CONVERT(BIT,1) AS 'Result'
		ELSE
			SELECT CONVERT(BIT,0) AS 'Result'
					
END
GO
/****** Object:  StoredProcedure [dbo].[Users_GetTransactionHistory]    Script Date: 4/23/2023 9:04:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Users_GetTransactionHistory] 
	-- Add the parameters for the stored procedure here
	@AccountNo BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
SELECT CASE th.TransactionType 
		   WHEN 0 THEN 'Deposit' 
		   WHEN 1 THEN 'Withdraw' 
		   ELSE 'Transfer' END AS TransactionType, 
		   th.Amount, 
		   af.AccountNumber AS 'AccountNumberFrom', 
		   at.AccountNumber AS 'AccountNumberTo', 
           th.TransactionDate, 
		   th.EndBalance
FROM     dbo.TransactionHistory AS th 
		 INNER JOIN
         dbo.Accounts AS af 
		 ON th.AccountIdFrom = af.Id 
		 INNER JOIN
         dbo.Accounts AS at 
		 ON th.AccountIdTo = at.Id
WHERE   af.AccountNumber = @AccountNo
ORDER BY th.TransactionDate DESC
					
END
GO
/****** Object:  StoredProcedure [dbo].[Users_GetVerifyUsers]    Script Date: 4/23/2023 9:04:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Users_GetVerifyUsers] 
	-- Add the parameters for the stored procedure here
	@Username NVARCHAR(50),
	@Password NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
		IF EXISTS (SELECT [Username] FROM Users WHERE [Username] = @Username AND [Password] = @Password)
			SELECT CONVERT(BIT,1) AS 'Result'
		ELSE
			SELECT CONVERT(BIT,0) AS 'Result'
					
END
GO
/****** Object:  StoredProcedure [dbo].[Users_SetNewUsers]    Script Date: 4/23/2023 9:04:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Users_SetNewUsers] 
	-- Add the parameters for the stored procedure here
	@Username NVARCHAR(50),
	@Password NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY 
		BEGIN TRANSACTION InsertTransaction			
			DECLARE @Id UNIQUEIDENTIFIER = NEWID();
			DECLARE @AccountNumber BIGINT = (SELECT CAST((RAND() * 9 + 1) * 100000000000 AS BIGINT))
			BEGIN
				INSERT INTO [dbo].[Users] 
				(
				[Id],
				[Username],
				[Password]
				)
				VALUES 
				(
				@Id,
				@Username,
				@Password
				);					
			END

			BEGIN
				INSERT INTO [dbo].[Accounts] 
				(
				[UserId],
				[AccountNumber],
				[Balance]
				)
				VALUES 
				(
				@Id,
				@AccountNumber,
				0
				);
			END

		COMMIT TRANSACTION InsertTransaction

		SELECT CONVERT(BIT,1) Result
					
	END TRY
	
	BEGIN CATCH 
    
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION InsertTransaction

		DECLARE @RC int
		DECLARE @ErrMessage varchar(200) 
		DECLARE @Severity BIGINT 
		DECLARE @State BIGINT

		SET @Severity = ERROR_SEVERITY()
		SET @State = ERROR_STATE()
		SET @ErrMessage = ERROR_MESSAGE()
		
		RAISERROR ( 'Transaction was rolled back. %s', -- Message text.  
				@Severity, -- Severity.  
				@State, -- State.  
				@ErrMessage);

	END CATCH 
END
GO

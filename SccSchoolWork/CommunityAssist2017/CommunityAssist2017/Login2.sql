USE [CommunityAssist2017]
GO

/****** Object:  StoredProcedure [dbo].[usp_Login2]    Script Date: 2/27/2018 10:11:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--create stored porcedure for
--handling logins with hashed password
alter Proc [dbo].[usp_Login2]
@Email nvarchar(255),
@password nvarchar(50)
As
--get the random seed

Declare @personKey int
if exists
		(Select PersonKey,PersonPassSeed
		from Person
		Where PersonEmail=@Email)
	begin
		Declare @PS varchar(50)
		Select @PS = cast(PersonPassSeed as varchar(50)), @personKey=PersonKey from Person
		Where PersonEmail=@Email
		--create a new hash based on the seed and password
	--	Declare @newHash varbinary(500)
	--	Set @newHash=dbo.setPassword(@password, @seed)



		
		if (@PS=@password)
		Begin
		print cast(@personKey as nvarchar(20))
		print 'Login successful'
			--get PersonKey
			Insert into LoginHistory(PersonKey, LoginHistoryTimeStamp)
			Values(@PersonKey, GetDate())
			Declare @loginHistoryKey int
			Set @loginHistoryKey=IDENT_CURRENT('LoginHistory')

			
		end
		else
		begin
			print 'invalid login'
			return -1
		end

end
	else
	begin
		print 'invalid login'
		return -1
	end
	return @personKey
GO



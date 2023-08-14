create PROCEDURE [dbo].[PR_City_SelectAll]
AS
SELECT [dbo].[LOC_City].[CityID],
[dbo].loc_city.[CountryID]
 ,[dbo].[LOC_City].[CityName]
 ,[dbo].[LOC_City].[StateID]
 ,[dbo].[LOC_State].[StateName]
 ,[dbo].[LOC_City].[citycode]
 ,[dbo].[LOC_City].[CreationDate]
 ,[dbo].[LOC_Country].[CountryName]
FROM [dbo].[LOC_City]
INNER JOIN [dbo].[LOC_State]
ON [dbo].[LOC_State].[StateID] = [dbo].[LOC_City].[StateID]
INNER JOIN [dbo].[LOC_Country]
ON [dbo].[LOC_Country].[CountryID] = [dbo].[LOC_State].[CountryID]
ORDER BY [dbo].[LOC_Country].[CountryName]
 ,[dbo].[LOC_State].[StateName]
 ,[dbo].[LOC_City].[CityName]

 exec  [dbo].[PR_City_SelectAll]

 CREATE PROCEDURE [dbo].[PR_City_SelectByPK]
 (
 @cityid int
 )
AS
SELECT [dbo].[LOC_City].[CityID]
 ,[dbo].[LOC_City].[CityName]
 ,[dbo].[LOC_City].[StateID]
 ,[dbo].[LOC_State].[StateName]
 ,[dbo].[LOC_City].[citycode]
 ,[dbo].[LOC_City].[CreationDate]
 ,[dbo].[LOC_Country].[CountryName]
FROM [dbo].[LOC_City]
INNER JOIN [dbo].[LOC_State]
ON [dbo].[LOC_State].[StateID] = [dbo].[LOC_City].[StateID]
INNER JOIN [dbo].[LOC_Country]
ON [dbo].[LOC_Country].[CountryID] = [dbo].[LOC_State].[CountryID]
where cityid=@cityid
ORDER BY [dbo].[LOC_Country].[CountryName]
 ,[dbo].[LOC_State].[StateName]
 ,[dbo].[LOC_City].[CityName]

 exec [dbo].[PR_City_SelectByPK] 2

 --------City=============================================----------------
 create procedure [dbo].[PR_STATE_SelectALL]

 AS
	select [dbo].[LOC_State].[StateID],
	[dbo].[LOC_State].[StateName],
	[dbo].[LOC_State].[StateCode],
	[dbo].[LOC_State].[Created],
	[dbo].[LOC_State].[Modified],
	[dbo].[LOC_Country].CountryName,
	[dbo].[LOC_Country].CountryID
	from
	LOC_Country
	inner join 
	LOC_State
	on
	LOC_Country.CountryID=LOC_State.CountryID
	order by LOC_Country.CountryName,
	LOC_State.StateName

	exec [dbo].[PR_STATE_SelectALL]

 create procedure [dbo].[PR_STATE_SelectByPk]
 (
 @stateid int
 )

 AS
	select [dbo].[LOC_State].[StateID],
	[dbo].[LOC_State].[StateName],
	[dbo].[LOC_State].[StateCode],
	[dbo].[LOC_State].[Created],
	[dbo].[LOC_State].[Modified],
	[dbo].[LOC_Country].CountryName,
	[dbo].[LOC_Country].CountryID
	from
	LOC_Country
	inner join 
	LOC_State
	on
	LOC_Country.CountryID=LOC_State.CountryID
	where LOC_State.StateID=@stateid
	order by LOC_Country.CountryName,
	LOC_State.StateName

	exec [dbo].PR_STATE_SelectByPk 3

-----====================COUNTRY=========================

create proc PR_Country_SelectAll
as
	select LOC_Country.CountryID,
	LOC_Country.CountryName,
	LOC_Country.CountryCode,
	LOC_Country.Created,
	LOC_Country.Modified

	FROM LOC_Country

EXEC PR_Country_SelectByPK

create proc PR_Country_SelectByPK
(
	@countryid int 
)
as
	select LOC_Country.CountryID,
	LOC_Country.CountryName,
	LOC_Country.CountryCode,
	LOC_Country.Created,
	LOC_Country.Modified

	FROM LOC_Country
	where LOC_Country.CountryID=@countryid

	exec PR_Country_SelectByPK 2

-------------------==================================================----------------------------------

create proc PR_Country_Insert
(
	@CountryName varchar(100),
	@CountryCode VARCHAR(50)
	
)
as 
begin

	insert into LOC_Country
	(
		CountryName,
		CountryCode
	)
	values(@CountryName,@CountryCode)

end

exec PR_Country_Insert 'zzzz','zzzzzz'
exec PR_Country_SelectAll
---------------------=============
create proc PR_State_Insert
(
	@StateName varchar(100),
	@CountryID int,
	@StateCode VARCHAR(50)
	
)
as 
begin

	insert into LOC_State
	(
		StateName,
		CountryID,
		StateCode
	)
	values(@StateName,@CountryID,@StateCode)

end

exec PR_STATE_SelectALL
exec PR_State_Insert 'Lahore',1002,'LH'

------City Add

create  proc PR_City_Insert
(
	@CityName varchar(100),
	@StateID int,
	@CountryID int,
	@CityCode VARCHAR(50)
	
)
as 
begin

	insert into LOC_City
	(
		CityName,
		StateID,
		CountryID,
		CityCode
	)
	values(@CityName,@StateID,@CountryID, @CityCode) 

end

exec PR_Country_SelectAll
exec PR_STATE_SelectALL
exec PR_City_Insert 'Rajkot',1,1,'RJT'

exec PR_City_SelectAll

---============   UPDATE   =============== 
 
 create proc PR_Country_Update
 (
	@CountryID int,
	@CountryName varchar(50),
	@cOUNTRYcODE varchar(50)
 )
 as
	
 begin 
	update LOC_Country
	set
		CountryName=@CountryName,
		CountryCode=@cOUNTRYcODE
	where
		CountryID=@CountryID
 end

 exec PR_Country_Update 1,'INDIA','IND'

 -------proc state update===================
 create proc PR_state_Update
 (
	@StateID	int,
	@StateName	varchar(50),
	@CountryId	int,
	@StateCode	varchar(50)
 )
 as
	
 begin 
	update LOC_State
	set
		StateName  =@StateName,
		CountryId  =@CountryId,
		StateCode  =@StateCode,
		Modified  =getdate()
	where
		StateID=@StateID
 end

 exec PR_STATE_SelectALL
 exec PR_state_Update 1,'GUJARAT',2,'GUJ'

 --===============city update==============----------

 create  proc PR_City_Update
 (
	@CityID	int,
	@CityName	varchar(50),
	@StateId	int,
	@CountryId	int,
	@cityCode	varchar(50)
 )
 as
	
 begin 
	update LOC_City
	set
		CityName	=@CityName,
		StateID		=@StateId,
		CountryId	=@CountryId,
		Citycode	=@Citycode,
		Modified	=getdate()
	where
		CityID=@CityID
 end

 exec PR_Country_SelectAll
 exec PR_City_SelectAll
 exec PR_City_SelectAll
 exec PR_City_Update 2,'BHUJ',1,1,'BHJ'


----============ ---DELETE ---==================--

==========================
create proc PR_Country_delete
(
	@CountryId int
)
as

begin
		delete from LOC_Country
		where CountryID=@CountryId
end

--------------------------------
=====================

create proc PR_State_delete
(
	@Stateid int
)
as

begin
		delete from LOC_State
		where StateID=@Stateid
end

-----------------------------------
create proc PR_City_delete
(
	@Cityid int
)
as

begin
		delete from LOC_City
		where CityID=@Cityid
end
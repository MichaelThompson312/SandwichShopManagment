/* Check whether the database exists and delete if so. */
IF EXISTS (SELECT 1 FROM master.dbo.sysdatabases
			WHERE name = 'SaindwhichDB')
BEGIN
	DROP DATABASE [SaindwhichDB] 
	PRINT '' PRINT '*** Dropping SaindwhichDB'
END
GO

PRINT '' PRINT '*** Creating SaindwhichDB'
GO
CREATE DATABASE [SaindwhichDB]
GO

PRINT '' PRINT '*** Using SaindwhichDB'
GO
USE [SaindwhichDB]
GO

PRINT '' PRINT '*** Creating employee table'
GO

CREATE TABLE [dbo].[Employee] (
	[EmployeeID]		[int] IDENTITY(1000000,1) 	NOT NULL,
	[FirstName]			[nvarchar](50) 				NOT NULL,
	[LastName]			[nvarchar](50) 				NOT NULL,
	[PhoneNumber]		[nvarchar](11) 				NOT NULL,
	[Email]				[nvarchar](250) 			NOT NULL,
	[PasswordHash]		[nvarchar](100)				NOT NULL DEFAULT
	'9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E',
	[Active]			[bit]						NOT NULL DEFAULT 1,
	CONSTRAINT [pk_EmployeeID] PRIMARY KEY([EmployeeID] ASC),
	CONSTRAINT [ak_Email]	UNIQUE([Email] ASC)
)
GO


PRINT '' PRINT '*** Creating Order table'
GO
CREATE TABLE [dbo].[order] (
	[OrderID]			[int] IDENTITY(100,1) 	NOT NULL,
	[EmployeeID] 		[int] 					 	NOT NULL,
	[Status]			[nvarchar](50)				NOT NULL DEFAULT 'Unstarted',
	CONSTRAINT [pk_OrderID] PRIMARY KEY([OrderID] ASC),
	CONSTRAINT [fk_employee_employeeID] FOREIGN KEY([EmployeeID]) REFERENCES [Employee]([EmployeeID]) ON UPDATE CASCADE,
)
GO



PRINT '' PRINT '*** Creating Ingredient table'
GO
CREATE TABLE [dbo].[ingredient] (
	[IngredientID]			[int] IDENTITY(500, 1)		NOT NULL,
	[IngredientName] [nvarchar](50) 					NOT NULL,
	[IngredientDescription] [nvarchar](250) 			NOT NULL
	CONSTRAINT [pk_IngredientID] PRIMARY KEY([IngredientID] ASC)
)
GO

PRINT '' PRINT '*** Creating Standard Item table'
GO
CREATE TABLE [dbo].[StandardItem] (
	[StandardItemID]	[int] IDENTITY(10,1) 		NOT NULL,
	[Name]				[nvarchar](50)				NOT NULL DEFAULT 'Base', 
	[Description]		[nvarchar](250) 			NOT NULL DEFAULT 'Base, empty Sandwhich',
	CONSTRAINT [pk_StandardItemID] PRIMARY KEY([StandardItemID] ASC)
)
GO

PRINT '' PRINT '*** Creating OrderItems table'
GO
CREATE TABLE [dbo].[OrderItems] (		 
	[OrderID]			[int]       			NOT NULL,
	[StandardItemID]	[int] 					NOT NULL,
	CONSTRAINT [pk_OrderItems] PRIMARY KEY(OrderID, StandardItemID),
	CONSTRAINT [fk_OrderItems_Order_OrderID] FOREIGN KEY (OrderID) REFERENCES [Order]([OrderID]) ON UPDATE CASCADE,
	CONSTRAINT [fk_OrderItem_StandardItem_StandardItemID] FOREIGN KEY (StandardItemID) REFERENCES [StandardItem]([StandardItemID]) ON UPDATE CASCADE,	
)
GO

PRINT '' PRINT '*** Creating Order Items Add Ons table'
GO
CREATE TABLE [dbo].[OrderItemAddOns] (
	[OrderID]			[int]       				NOT NULL,
	[StandardItemID]	[int] 						NOT NULL,
	[IngredientID]		[int]		 				NOT NULL,
	[Quantity]			[int]						NOT NULL DEFAULT 1,
	CONSTRAINT [pk_OrderItemAddOns] PRIMARY KEY(OrderID, StandardItemID, IngredientID),
	CONSTRAINT [fk_OrderItemAddOns_Order_OrderID] FOREIGN KEY (OrderID) REFERENCES [Order]([OrderID]) ON UPDATE CASCADE,
	CONSTRAINT [fk_OrderItemAddOns_StandardItem_StandardItemID] FOREIGN KEY (StandardItemID) REFERENCES [StandardItem]([StandardItemID]) ON UPDATE CASCADE,
	CONSTRAINT [fk_OrderItemAddOns_Ingredient_IngredientID] FOREIGN KEY (IngredientID) REFERENCES [Ingredient]([IngredientID]) ON UPDATE CASCADE
)
GO


PRINT '' PRINT '*** Adding Index for LastName on Employee Table'
GO
CREATE NONCLUSTERED INDEX [ix_lastName]
	ON[Employee]([LastName] ASC)
GO

PRINT '' PRINT '*** Creating role table'
GO
CREATE TABLE [dbo].[Role] (
	[RoleID]			[nvarchar](50)				NOT NULL,
	[Description]		[nvarchar](250) 			NULL,
	CONSTRAINT	[pk_RoleID]	PRIMARY KEY([RoleID] ASC)
)
GO

PRINT '' PRINT '*** Creating employeeRole table'
GO
CREATE TABLE [dbo].[employeeRole] (
	[EmployeeID]		[int] 					 	NOT NULL,
	[RoleID]			[nvarchar](50)				NOT NULL,
	CONSTRAINT	[pk_EmployeeID_RoleID]	
		PRIMARY KEY([EmployeeID] ASC, [RoleID] ASC),
	CONSTRAINT	[fk_employeeRole_employeeID] FOREIGN KEY([EmployeeID])
		REFERENCES[Employee]([EmployeeID]) ON UPDATE CASCADE,
	CONSTRAINT	[fk_employeeRole_roleID] FOREIGN KEY([RoleID])
		REFERENCES[Role]([RoleID]) ON UPDATE CASCADE
)
GO

PRINT '' PRINT '*** Creating sp_insert_employee'
GO
CREATE PROCEDURE [sp_insert_employee]
(
	@FirstName		[nvarchar](50),
	@LastName		[nvarchar](50),
	@PhoneNumber	[nvarchar](11),
	@Email			[nvarchar](250)
)
AS
BEGIN
	INSERT INTO [dbo].[Employee]
		([FirstName], [LastName], [PhoneNumber], [Email])
	VALUES
		(@FirstName, @LastName, @PhoneNumber, LOWER(@Email))
	SELECT SCOPE_IDENTITY()
END
GO

PRINT '' PRINT '*** Creating sp_insert_order'
GO
CREATE PROCEDURE [sp_insert_order]
(
	@EmployeeID 	[int] 
)
AS
BEGIN
	INSERT INTO [dbo].[Order]
		([EmployeeID])
	VALUES
		(@EmployeeID)
	SELECT SCOPE_IDENTITY()
END
GO

PRINT '' PRINT '*** Creating sp_insert_orderitem'
GO
CREATE PROCEDURE [sp_insert_orderitem]
(
	@OrderID 		[int],
	@StandardItemID	[int] 
)
AS
BEGIN
	INSERT INTO [dbo].[OrderItems]
           ([OrderID],
           [StandardItemID])
     VALUES
           (@OrderID,
		   @StandardItemID)
	SELECT @@ROWCOUNT
END
GO

PRINT '' PRINT '*** Creating sp_insert_standarditem'
GO
CREATE PROCEDURE [sp_insert_standarditem]

AS
BEGIN
	INSERT INTO [dbo].[StandardItem]
	DEFAULT VALUES
	SELECT SCOPE_IDENTITY()
END
GO

PRINT '' PRINT '*** Creating sp_insert_addon'
GO
CREATE PROCEDURE [sp_insert_addon]
(
	@OrderID 			[int],
	@StandardItemID 	[int],
	@IngredientID 		[int] 
)
AS
BEGIN
	INSERT INTO [dbo].[OrderItemAddOns]
           ([OrderID]
           ,[StandardItemID]
           ,[IngredientID])
     VALUES
           (@OrderID, @StandardItemID, @IngredientID)
	SELECT @@ROWCOUNT
END
GO

PRINT '' PRINT '*** Creating sp_get_order_by_status'
GO
CREATE PROCEDURE [sp_get_order_by_status]
(
	@Status			[nvarchar](50)
)
AS BEGIN
	SELECT o.OrderID, [StandardItem].StandardItemID, [ingredient].IngredientName, [ingredient].IngredientDescription
	FROM [dbo].[order] o
	LEFT JOIN [OrderItems]
	ON o.OrderID = OrderItems.OrderID
	LEFT JOIN [dbo].[StandardItem]
	ON OrderItems.StandardItemId = StandardItem.StandardItemId
	LEFT JOIN [dbo].[OrderItemAddOns]
	ON o.OrderID = OrderItemAddOns.OrderID 
	AND orderItems.StandardItemId = OrderItemAddOns.StandardItemid
	LEFT JOIN [dbo].[ingredient]
	ON OrderItemAddOns.IngredientID = Ingredient.IngredientID
	WHERE 
		o.Status = @Status
END
GO

PRINT '' PRINT '*** Creating sp_get_all_active_orders'
GO
CREATE PROCEDURE [sp_get_all_active_orders]
AS BEGIN
	SELECT o.OrderID, [StandardItem].StandardItemID, [ingredient].IngredientName, [ingredient].IngredientDescription
	FROM [dbo].[order] o
	LEFT JOIN [OrderItems]
	ON o.OrderID = OrderItems.OrderID
	LEFT JOIN [dbo].[StandardItem]
	ON OrderItems.StandardItemId = StandardItem.StandardItemId
	LEFT JOIN [dbo].[OrderItemAddOns]
	ON o.OrderID = OrderItemAddOns.OrderID 
	AND orderItems.StandardItemId = OrderItemAddOns.StandardItemid
	LEFT JOIN [dbo].[ingredient]
	ON OrderItemAddOns.IngredientID = Ingredient.IngredientID
	WHERE 
		o.Status != 'Delivered'
END
GO




PRINT '' PRINT '*** Creating sp_create_order_items_record'
GO
CREATE PROCEDURE [sp_create_order_items_record]
(
	@OrderID 	[int],
	@StandardItemID	[int]
)
AS
BEGIN
	INSERT INTO [dbo].[OrderItems]
		([OrderID], [StandardItemID])
	VALUES
		(@OrderID, @StandardItemID)
	SELECT @@ROWCOUNT
END
GO

PRINT '' PRINT '*** Creating sp_update_order_status'
GO
CREATE PROCEDURE [sp_update_order_status]
(
	@OrderID 	[int],
	@Status	[nvarchar](50)
)
AS
BEGIN
	UPDATE	[dbo].[order]
	SET		[Status] = (@Status)
	WHERE	[OrderID] = (@OrderID)
	RETURN	@@ROWCOUNT
END
GO

PRINT '' PRINT '*** Creating Sample Employee Records'
GO
INSERT INTO [dbo].[Employee]
	([FirstName], [LastName], [PhoneNumber], [Email])
	VALUES
	('System', 'Admin', '13191230000', 'admin@company.com'),
	('James', 'Smith', '13191231111', 'James@company.com'),
	('Adam', 'Jones', '13191232222', 'Adam@company.com'),
	('Leo', 'Williams', '13191233333', 'leo@company.com')
GO


PRINT '' PRINT '*** Creating Ingredients Records'
GO
INSERT INTO [dbo].[ingredient]
	([IngredientName], [IngredientDescription])
	VALUES
	('Pita', 'Bread'),
	('White', 'Bread'),
	('Wheat', 'Bread'),
	('Beef', 'Meat'),
	('Turkey', 'Meat'),
	('Chicken', 'Meat'),
	('Bean', 'Meat'),
	('Pork', 'Meat'),
	('Tomato', 'Vegtable'),
	('Lettuce', 'Vegtable'),
	('Cucumber', 'Vegtable'),
	('Onion', 'Vegtable'), 
	('Peppers', 'Vegtable'), 
	('Red Onion', 'Vegtable'),
	('Olives', 'Vegtable'), 
	('Spinach', 'Vegtable'),
	('Avacado', 'Vegtable'), 
	('Cabbage', 'Vegtable'),
	('Mayo', 'Condiment'), 
	('Ranch', 'Condiment'),
	('Mustard', 'Condiment'), 
	('Oil', 'Condiment'), 
	('Ketchup', 'Condiment'), 
	('Mozzarella', 'Cheese'),
	('Cheddar', 'Cheese'), 
	('Gouda', 'Cheese'),
	('Provalone', 'Cheese'), 
	('Havarti', 'Cheese')
GO

PRINT '' PRINT '*** Creating Sample Deactivated Employee'
GO
INSERT INTO [dbo].[Employee]
		([FirstName], [LastName], [PhoneNumber], [Email], [Active])
	VALUES
	('Kevin', 'Malone', '13191239999', 'Kevin@company.com', 0)
GO

PRINT '' PRINT '*** Creating Sample Role Records'
GO
INSERT INTO [dbo].[Role]
	([RoleID], [Description])
	VALUES
	('Administrator', 'System Administrator'),
	('Server', 'Server of Products'),
	('Manager', 'Manager of Restaurant'),
	('Cook', 'Cook of orders')
GO

PRINT '' PRINT '*** Inserting Sample EmployeeRole Records'
GO
INSERT INTO [dbo].[EmployeeRole]
	([EmployeeID], [RoleID])
	VALUES
	(1000000, 'Administrator'),
	(1000001, 'Server'),
	(1000002, 'Manager'),
	(1000003, 'Cook')
GO

PRINT '' PRINT'*** Creating sp_authenticate_user'
GO
CREATE PROCEDURE [sp_authenticate_user]
(
	@Email			[nvarchar](250),
	@PasswordHash	[nvarchar](100)
)
AS
BEGIN
	SELECT 	COUNT([EmployeeID])
	FROM	[dbo].[Employee]
	WHERE	[Email] = LOWER(@Email)
	AND		[PasswordHash] = @PasswordHash
	AND		[Active] = 1
END
GO

PRINT '' PRINT '*** Creating sp_update_email'
GO
CREATE PROCEDURE [sp_update_email]
(
	@OldEmail			[nvarchar](250),
	@NewEmail			[nvarchar](250),
	@PasswordHash		[nvarchar](100)
)
AS
BEGIN
	UPDATE	[dbo].[Employee]
	SET		[Email] = LOWER(@NewEmail)
	WHERE	[Email] = LOWER(@OldEmail)
	AND		[PasswordHash] = @PasswordHash
	AND		[Active] = 1
	RETURN	@@ROWCOUNT
END
GO

PRINT '' PRINT '*** Creating sp_select_user_by_email'
GO
CREATE PROCEDURE [sp_select_user_by_email]
(
	@Email		[nvarchar](250)
)
AS
BEGIN
	SELECT [EmployeeID],[FirstName],[LastName],[PhoneNumber]
	FROM [Employee]
	WHERE [Email] = @Email
END
GO

PRINT '' PRINT '*** Creating sp_select_roles_by_userid'
GO
CREATE PROCEDURE [sp_select_roles_by_userid]
(
	@EmployeeID		[int]
)
AS
BEGIN
	SELECT [RoleID]
	FROM [EmployeeRole]
	WHERE [EmployeeID] = @EmployeeID
END
GO


PRINT '' PRINT '*** Creating sp_update_password'
GO
CREATE PROCEDURE [sp_update_password]
(
	@EmployeeID			[int],
	@OldPasswordHash 	[nvarchar](100),
	@NewPasswordHash 	[nvarchar](100)
)
AS
BEGIN
	UPDATE	[dbo].[Employee]
	SET		[PasswordHash] = @NewPasswordHash
	WHERE	[EmployeeID] = @EmployeeID
	AND		[PasswordHash] = @OldPasswordHash
	AND		[Active] = 1
	RETURN	@@ROWCOUNT
END
GO

PRINT '' PRINT '*** Creating sp_select_users_by_active'
GO
CREATE PROCEDURE sp_select_users_by_active
(
	@Active		[bit]
)
AS
BEGIN
	SELECT 	[EmployeeID],[FirstName],[LastName],[PhoneNumber], [Email]
	FROM	[dbo].[Employee]
	WHERE 	[Active] = @Active
END
GO

PRINT '' PRINT '*** Creating sp_select_all_addons'
GO
CREATE PROCEDURE sp_select_all_addons
AS
BEGIN
	SELECT 	[IngredientName],
			[IngredientID]
	FROM	[dbo].[ingredient]
END
GO

PRINT '' PRINT '*** Creating sp_select_employee_by_id'
GO
CREATE PROCEDURE sp_select_employee_by_id
(
	@EmployeeID	[int]
)
AS
BEGIN
	SELECT [EmployeeID],[FirstName],[LastName],[PhoneNumber],[Email],[Active]
	FROM [dbo].[Employee]
	WHERE [EmployeeID] = @EmployeeID
END
GO

PRINT '' PRINT '*** Creating sp_update_employee'
GO
CREATE PROCEDURE sp_update_employee
(
	@EmployeeID		[int],
	
	@NewFirstName	[nvarchar](50),
	@NewLastName	[nvarchar](50),
	@NewPhoneNumber	[nvarchar](11),
	@NewEmail		[nvarchar](250),
	
	@OldFirstName	[nvarchar](50),
	@OldLastName	[nvarchar](50),
	@OldPhoneNumber	[nvarchar](11),
	@OldEmail		[nvarchar](250)
)
AS
BEGIN
	UPDATE [dbo].[Employee]
		SET [FirstName] = @NewFirstName,
			[LastName] = @NewLastName,
			[PhoneNumber] = @NewPhoneNumber,
			[Email] = @NewEmail
	WHERE [EmployeeID] = @EmployeeID
	AND		[FirstName] = @OldFirstName
	AND		[LastName] = @OldLastName
	AND 	[PhoneNumber] = @OldPhoneNumber
	AND		[Email] = @OldEmail

	RETURN @@ROWCOUNT
END
GO

PRINT '' PRINT '*** Creating sp_deactivate_employee'
GO
CREATE PROCEDURE sp_deactivate_employee
(
	@EmployeeID		[int]
)
AS
BEGIN
	UPDATE	[dbo].[Employee]
		SET [Active] = 0
	WHERE [EmployeeID] = @EmployeeID
	RETURN @@ROWCOUNT
END
GO

PRINT '' PRINT '*** Creating sp_reactivate_employee'
GO
CREATE PROCEDURE sp_reactivate_employee
(
	@EmployeeID		[int]
)
AS
BEGIN
	UPDATE	[dbo].[Employee]
		SET [Active] = 1
	WHERE [EmployeeID] = @EmployeeID
	RETURN @@ROWCOUNT
END
GO

PRINT '' PRINT '*** Creating sp_insert_employee_role'
GO
CREATE PROCEDURE sp_insert_employee_role
(
	@EmployeeID		[int],
	@RoleID			[nvarchar](50)
)
AS
BEGIN
	INSERT INTO [dbo].[EmployeeRole]
	([EmployeeID],[RoleID])
	VALUES
	(@EmployeeID, @RoleID)
END
GO

PRINT '' PRINT '*** Creating sp_delete_employee_role'
GO
CREATE PROCEDURE sp_delete_employee_role
(
	@EmployeeID		[int],
	@RoleID			[nvarchar](50)
)
AS
BEGIN
	DELETE FROM [dbo].[EmployeeRole]
	WHERE [EmployeeID] = @EmployeeID
	AND		[RoleID] = @RoleID
END
GO

PRINT '' PRINT '*** Creating sp_select_all_roles'
GO
CREATE PROCEDURE sp_select_all_roles
AS
BEGIN
	SELECT [RoleID]
	FROM [dbo].[Role]
	ORDER BY [RoleId]
END
GO



PRINT '' PRINT 'Insert an orders'
GO
DECLARE	@return_value int

EXEC	@return_value = [dbo].[sp_insert_order]
		@EmployeeID = 1000002
GO


PRINT '' PRINT 'Insert a standard item'
GO
EXEC	[dbo].[sp_insert_standarditem]
GO


PRINT '' PRINT 'Add item to order'
GO
DECLARE	@return_value int

EXEC	@return_value = [dbo].[sp_insert_orderitem]
		@OrderID = 100,
		@StandardItemID = 10

SELECT	'Return Value' = @return_value
GO

PRINT '' PRINT 'Insert another standard item'
GO
EXEC	[dbo].[sp_insert_standarditem]
GO

PRINT '' PRINT 'Add another item to the order'
GO
DECLARE	@return_value int

EXEC	@return_value = [dbo].[sp_insert_orderitem]
		@OrderID = 100,
		@StandardItemID = 11

SELECT	'Return Value' = @return_value

GO

PRINT '' PRINT 'Adding add ons to sample orders'
GO
EXEC	[dbo].[sp_insert_addon]
		@OrderID = 100,
		@StandardItemID = 10,
		@IngredientID = 501

GO

PRINT '' PRINT 'Adding add ons to sample orders'
GO
EXEC	[dbo].[sp_insert_addon]
		@OrderID = 100,
		@StandardItemID = 10,
		@IngredientID = 506

GO
PRINT '' PRINT 'Adding add ons to sample orders'
GO
EXEC	[dbo].[sp_insert_addon]
		@OrderID = 100,
		@StandardItemID = 11,
		@IngredientID = 502

GO
PRINT '' PRINT 'Adding add ons to sample orders'
GO
EXEC	[dbo].[sp_insert_addon]
		@OrderID = 100,
		@StandardItemID = 11,
		@IngredientID = 510

GO


























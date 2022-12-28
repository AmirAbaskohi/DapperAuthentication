-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
USE [DeveloperTest]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Amirhossein Abaskohi
-- Create date: 12/25/2022
-- Description:	This procedure reads that messages with Status=1(crteated) and then updates their status to 2(read)
-- =============================================
ALTER PROCEDURE [dbo].[ChangeMessageStatus]
AS
BEGIN
	SELECT * FROM [dbo].[AmirhosseinMessage] WHERE [Status] = 1;

    UPDATE [dbo].[AmirhosseinMessage]
    SET [Status] = 2
    WHERE [Status] = 1;
END
GO
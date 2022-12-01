USE master;  
GO  
CREATE DATABASE DapperAuth  
ON   
( NAME = DapperAuth_dat,  
    FILENAME = '[YOUR ADDRESS]\dapperauth.mdf',  
    SIZE = 10,  
    MAXSIZE = 50,  
    FILEGROWTH = 5 )  
LOG ON  
( NAME = DapperAuth_log,  
    FILENAME = '[YOUR ADDRESS]\dapperauthlog.ldf',  
    SIZE = 5MB,  
    MAXSIZE = 25MB,  
    FILEGROWTH = 5MB );  
GO  
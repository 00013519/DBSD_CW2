-- create the database
create database ScooterManagementSystem;
go

-- use the database
use ScooterManagementSystem;
go

-- create the Station table
create table Station (
    StationId int primary key,
    Location varchar(100) not null,
    Capacity int not null,
    CurrentAmount int not null
);
go

-- create the Scooter table
create table Scooter (
    ScooterId int primary key,
    StationId int foreign key references Station(StationId) not null,
    Model varchar(100) not null,
    BatteryLevel decimal(5, 2) not null,
    Status varchar(50) not null
);
go

-- create the Employee table
create table Employee (
    EmployeeId int primary key,
    FirstName varchar(50) not null,
    LastName varchar(50) not null,
    DateEmployed date not null,
    Position varchar(50) not null,
    Salary decimal(10, 2) not null,
    IsActive bit not null,
    ProfilePicture varbinary(max) not null,
    StationId int foreign key references Station(StationID) not null
);
go

-- create the MaintenanceRecord table
create table MaintenanceRecord (
    MaintenanceId int primary key,
    EmployeeId int foreign key references Employee(EmployeeId) not null,
    ScooterId int foreign key references Scooter(ScooterId) not null,
    IssueDescription varchar(max) not null,
    RepairCost decimal(10, 2) not null,
    RepairStatus varchar(50) not null,
    RepairDate date not null,
    TotalRepairCost decimal(10, 2) not null
);
go

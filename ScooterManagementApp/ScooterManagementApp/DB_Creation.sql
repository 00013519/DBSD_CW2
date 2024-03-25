create table Station(
    StationId int identity(1,1),
    City nvarchar(255) not null,
    District nvarchar(50) not null,
    Street nvarchar(255) not null,
    ChargersAmount int not null,
    Capacity int not null,
    constraint pk_station_stataionId primary key(StationID),
    constraint ck_station_chargersAmount_capacity check(Capacity >= 0 and ChargersAmount >= 0)
)

create table Scooter(
    ScooterID int identity(1,1),
    Model nvarchar(255) not null,
    Status nvarchar(50) not null,
    BatteryCapacity decimal(7, 2) not null,
    MaxSpeed int not null,
    StationID int null,
    constraint pk_scooter_scooterId primary key(ScooterID),
    constraint fk_scooter_stationId foreign key(StationID) references Station(StationID),
    constraint ck_scooter_batteryCapacity check(BatteryCapacity > 0),
    constraint ck_scooter_maxSpeed check(MaxSpeed > 0)
)

create table Employee (
    EmployeeId int identity(1,1),
    FirstName varchar(50) not null,
    LastName varchar(50) not null,
    DateEmployed date not null,
    Position varchar(50) not null,
    Salary decimal(10, 2) not null,
    IsActive bit not null,
    ProfilePicture varbinary(max) null,
    StationId int not null,
    constraint pk_employee_employeeId primary key(EmployeeId),
    constraint fk_employee_station foreign key(StationId) references Station(StationId)
)

create table MaintenanceRecord (
    MaintenanceId int identity(1,1),
    EmployeeId int not null,
    ScooterId int not null,
    IssueDescription varchar(max) not null,
    RepairCost decimal(10, 2) not null,
    RepairStatus varchar(50) not null,
    RepairDate date not null,
    TotalRepairCost decimal(10, 2) null,
    constraint pk_mRecord_maintenanceId primary key(MaintenanceId),
    constraint fk_mRecord_employee foreign key(EmployeeId) references Employee(EmployeeId),
    constraint fk_mRecord_scooter foreign key(ScooterId) references Scooter(ScooterId)
)
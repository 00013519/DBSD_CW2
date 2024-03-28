-- Custom Access Trigger
create or alter trigger tr_Station_Check_OfficeHours
on Station
after insert, update, delete
as
begin
    declare @TimeNow time = CONVERT(time, GETDATE())
    
    if @TimeNow < '07:30' or @TimeNow > '17:00'
    begin
        rollback transaction;
        throw 51000, N'Operations on Station table are only allowed during office hours (7:30-17:00).', 1;
    end
end;
go

create or alter trigger tr_MaintenanceRecord_Check_OfficeHours
on MaintenanceRecord
after insert, update, delete
as
begin
    declare @TimeNow time = CONVERT(time, GETDATE())

    if @TimeNow < '07:30' or @TimeNow > '17:00'
    begin
        rollback transaction;
        throw 51001, N'Operations on MaintenanceRecord table are only allowed during office hours (7:30-17:00).', 1;
    end
end;
go

-- A Trigger for Data Logging
create or alter trigger tr_MaintenanceRecord_UpdateTotalRepairCost
on MaintenanceRecord
after insert, update, delete
as
begin
    declare @ScooterId int;

    if exists (select 1 from inserted)
        select @ScooterId = ScooterId from inserted;
    else
        select @ScooterId = ScooterId from deleted;

    update Scooter
    set TotalRepairCost = (select sum(RepairCost) from MaintenanceRecord where ScooterId = @ScooterId)
    where ScooterId = @ScooterId;
end;
go

-- A Trigger that will perform validation of some business rule (constraint)
create or alter TRIGGER tr_Employee_EmployeesAtStationLimit
on Employee
after insert
as
begin
    declare @StationId int;

    select @StationId = StationId from inserted;

    declare @EmployeeCount int;
    select @EmployeeCount = COUNT(*) from Employee where StationId = @StationId;

    if @EmployeeCount > 5
    begin
        rollback transaction;
        throw 51002, N'The number of employees working at a station cannot exceed 5.', 1;
    end
end;
go
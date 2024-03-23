-- Get All Employees
create or alter procedure udpGetAllEmployees
as
begin
    select EmployeeId, FirstName, LastName, DateEmployed, Position, Salary, IsActive, ProfilePicture, StationId
    from Employee;
end;
go

-- Insert Employee
create or alter procedure udpInsertEmployee(
    @FirstName nvarchar(200),
    @LastName nvarchar(200),
    @DateEmployed date,
    @Position nvarchar(50),
    @Salary decimal(10, 2),
    @IsActive bit,
    @ProfilePicture varbinary(max),
    @StationId int,
    @Errors nvarchar(1000) out
)
as
begin
    begin try
        insert into Employee(FirstName, LastName, DateEmployed, Position, Salary, IsActive, ProfilePicture, StationId)
        output inserted.EmployeeId
        values (@FirstName, @LastName, @DateEmployed, @Position, @Salary, @IsActive, @ProfilePicture, @StationId);

        return (0);
    end try
    begin catch
        set @Errors = error_message();
        return (1);
    end catch;
end;
go

-- Update Employee
create or alter procedure udpUpdateEmployee(
    @EmployeeId int,
    @FirstName nvarchar(200),
    @LastName nvarchar(200),
    @DateEmployed date,
    @Position nvarchar(50),
    @Salary decimal(10, 2),
    @IsActive bit,
    @ProfilePicture varbinary(max),
    @StationId int,
    @Errors nvarchar(1000) out
)
as
begin
    begin try
        update Employee
        set FirstName = @FirstName,
            LastName = @LastName,
            DateEmployed = @DateEmployed,
            Position = @Position,
            Salary = @Salary,
            IsActive = @IsActive,
            ProfilePicture = @ProfilePicture,
            StationId = @StationId
        where EmployeeId = @EmployeeId;

        return (0);
    end try
    begin catch
        set @Errors = error_message();
        return (1);
    end catch;
end;
go

-- Delete Employee
create or alter procedure udpDeleteEmployee(
    @EmployeeId int,
    @Errors nvarchar(1000) out
)
as
begin
    begin try
        delete from Employee
        where EmployeeId = @EmployeeId;

        return (0);
    end try
    begin catch
        set @Errors = error_message();
        return (1);
    end catch;
end;
go
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

--filter
create or alter procedure udpFilterEmployees(
    @DateEmployed date,
    @Position nvarchar(50),
    @StationId int,
    @SortField nvarchar(200) = 'EmployeeId',
    @SortDesc bit = 0,
    @Page int = 1,
    @PageSize int = 2
)
as
begin
  declare @SortDir nvarchar(10) = ' ASC '
  if @SortDesc = 1
    set @SortDir = ' DESC '

  declare @paramsDef nvarchar(2000) = '
    @DateEmployed date,
    @Position nvarchar(50),
    @StationId int,
    @PageSize int,
    @Page int '

  declare @sql nvarchar(2000) = '
  select e.EmployeeId, e.FirstName, 
  e.LastName, e.DateEmployed, e.Position, e.Salary,
  e.StationId, concat(s.City , '' , '', s.District, '' , '', s.Street) as "Station Address",
  (select count(*) from MaintenanceRecord where EmployeeId = e.EmployeeId) as "Total repairs",
  count(*) over () TotalCount
  from Employee e 
  join Station s on e.StationId = s.StationId
  where e.Position like concat(@Position, ''%'')
            and e.StationId like concat(@StationId, ''%'')
            and e.DateEmployed >= coalesce(@DateEmployed, ''1900-01-01'')
  order by '+ @SortField + @SortDir +'
  offset @PageSize * (@Page-1)  rows
  fetch next @PageSize rows only'

   exec sp_executesql @sql, @paramsDef,
                            @DateEmployed = @DateEmployed,
                            @Position = @Position,
                            @StationId = @StationId,
                            @PageSize = @PageSize,
                            @Page = @Page
end


go 
--test
exec udpFilterEmployees
  @DateEmployed = null,
  @Position = null,
  @StationId = null,
  @SortField = 'e.Salary',
  @SortDesc = 0,
  @Page = 1,
  @PageSize = 10
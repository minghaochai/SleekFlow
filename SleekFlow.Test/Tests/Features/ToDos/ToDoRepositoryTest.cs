using Microsoft.EntityFrameworkCore;
using SleekFlow.Domain.Entities;
using SleekFlow.Domain.Enums;
using SleekFlow.Domain.Filters;
using SleekFlow.Infrastructure;
using SleekFlow.Infrastructure.Repositories;
using Xunit;

namespace SleekFlow.Test.Tests.Features.ToDos
{
    public class ToDoRepositoryTest
    {
        private readonly DbContextOptions<SleekFlowDbContext> dbContextOptions;

        public ToDoRepositoryTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<SleekFlowDbContext>()
                .UseInMemoryDatabase("SleekFlow")
                .Options;
        }

        [Fact]
        public async Task GetById_Id1_ReturnRecord()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                var entity = await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                entity.State = EntityState.Detached;

                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    // Act
                    var result = await toDoRepository.GetById(1);
                    // Assert
                    Assert.NotNull(result);
                    Assert.Equal(1, result.Id);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task GetById_Id2_ReturnNull()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    // Act
                    var result = await toDoRepository.GetById(2);
                    // Assert
                    Assert.Null(result);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task Insert_NewToDo_ReturnSuccess()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    var newToDo = new ToDo
                    {
                        Name = "Chores",
                        Description = "House chores",
                        DueAt = DateTime.UtcNow.AddDays(2),
                        Status = Status.NotStarted,
                        AddAt = DateTime.UtcNow,
                        AddBy = "Test"
                    };

                    // Act
                    var result = await toDoRepository.Insert(newToDo);

                    // Assert
                    Assert.Equal(1, result);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task Insert_ExistingToDo_ReturnFail()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                var entity = await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                entity.State = EntityState.Detached;
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    var newToDo = new ToDo
                    {
                        Id = 1,
                        Name = "Chores",
                        Description = "House chores",
                        DueAt = DateTime.UtcNow.AddDays(2),
                        Status = Status.NotStarted,
                        AddAt = DateTime.UtcNow,
                        AddBy = "Test"
                    };

                    // Act
                    var ex = await Assert.ThrowsAsync<ArgumentException>(() => toDoRepository.Insert(newToDo));

                    // Assert
                    Assert.Equal("ArgumentException", ex.GetType().Name);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task Delete_ExistingToDo_ReturnSuccess()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                var entity = await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                entity.State = EntityState.Detached;
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    var toDo = new ToDo
                    {
                        Id = 1
                    };

                    // Act
                    var result = await toDoRepository.Delete(toDo);
                    var record = await toDoRepository.GetById(1);

                    // Assert
                    Assert.Equal(1, result);
                    Assert.Null(record);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task Delete_NonExistentToDo_ReturnFail()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    var toDo = new ToDo
                    {
                        Id = 1
                    };

                    // Act
                    var ex = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => toDoRepository.Delete(toDo));

                    // Assert
                    Assert.Equal("DbUpdateConcurrencyException", ex.GetType().Name);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task Update_ExistingToDo_ReturnSuccess()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                var entity = await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                entity.State = EntityState.Detached;

                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    var existingToDo = new ToDo
                    {
                        Id = 1,
                        Name = "Chores",
                        Description = "House chores",
                        DueAt = DateTime.UtcNow.AddDays(2),
                        Status = Status.NotStarted,
                        AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc),
                        AddBy = "System",
                        EditAt = DateTime.UtcNow,
                        EditBy = "Test"
                    };

                    // Act
                    var result = await toDoRepository.Update(existingToDo);
                    var record = await toDoRepository.GetById(1);

                    // Assert
                    Assert.Equal(1, result);
                    Assert.Equal("Chores", record.Name);
                    Assert.NotNull(record.EditAt);
                    Assert.NotNull(record.EditBy);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task Update_NonExistentToDo_ReturnFail()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    var toDo = new ToDo
                    {
                        Id = 1,
                        Name = "Chores",
                        Description = "House chores",
                        DueAt = DateTime.UtcNow.AddDays(2),
                        Status = Status.NotStarted,
                        AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc),
                        AddBy = "System",
                        EditAt = DateTime.UtcNow,
                        EditBy = "Test"
                    };

                    // Act
                    var ex = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => toDoRepository.Update(toDo));

                    // Assert
                    Assert.Equal("DbUpdateConcurrencyException", ex.GetType().Name);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task GetPageList_DateRangeBetweenParam_ReturnValidRecords()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 2, Name = "Dry", Description = "Hang clothes to dry", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 3, Name = "Iron", Description = "Iron clothes", DueAt = new DateTime(2023, 5, 28, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    // Act
                    var filter = new ToDoPageFilter
                    {
                        Status = Status.NotStarted,
                        DueAtStart = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc),
                        DueAtEnd = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc),
                        PageNumber = 1,
                        ItemsPerPage = 5,
                    };
                    var result = await toDoRepository.GetPageList(filter);

                    // Assert
                    Assert.Equal(2, result.Count);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task GetPageList_StatusInProgress_ReturnValidRecords()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.InProgress, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 2, Name = "Dry", Description = "Hang clothes to dry", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 3, Name = "Iron", Description = "Iron clothes", DueAt = new DateTime(2023, 5, 28, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    // Act
                    var filter = new ToDoPageFilter
                    {
                        Status = Status.InProgress,
                        DueAtStart = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc),
                        DueAtEnd = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc),
                        PageNumber = 1,
                        ItemsPerPage = 5,
                    };
                    var result = await toDoRepository.GetPageList(filter);

                    // Assert
                    Assert.Equal(1, result.Count);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task GetPageList_InvalidDateRangeBetween_ReturnValidRecords()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.InProgress, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 2, Name = "Dry", Description = "Hang clothes to dry", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 3, Name = "Iron", Description = "Iron clothes", DueAt = new DateTime(2023, 5, 28, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    // Act
                    var filter = new ToDoPageFilter
                    {
                        Status = Status.InProgress,
                        DueAtStart = new DateTime(2023, 6, 27, 0, 0, 0, DateTimeKind.Utc),
                        DueAtEnd = new DateTime(2023, 6, 27, 0, 0, 0, DateTimeKind.Utc),
                        PageNumber = 1,
                        ItemsPerPage = 5,
                    };
                    var result = await toDoRepository.GetPageList(filter);

                    // Assert
                    Assert.Equal(0, result.Count);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task GetPageList_InvalidStatus_ReturnValidRecords()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.InProgress, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 2, Name = "Dry", Description = "Hang clothes to dry", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 3, Name = "Iron", Description = "Iron clothes", DueAt = new DateTime(2023, 5, 28, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    // Act
                    var filter = new ToDoPageFilter
                    {
                        Status = Status.Completed,
                        DueAtStart = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc),
                        DueAtEnd = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc),
                        PageNumber = 1,
                        ItemsPerPage = 5,
                    };
                    var result = await toDoRepository.GetPageList(filter);

                    // Assert
                    Assert.Equal(0, result.Count);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task GetPageList_SortByAddAtDateDescending_ReturnValidSort()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 2, Name = "Dry", Description = "Hang clothes to dry", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 6, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    // Act
                    var filter = new ToDoPageFilter
                    {
                        Status = Status.NotStarted,
                        DueAtStart = new DateTime(2023, 5, 24, 0, 0, 0, DateTimeKind.Utc),
                        DueAtEnd = new DateTime(2023, 6, 26, 0, 0, 0, DateTimeKind.Utc),
                        PageNumber = 1,
                        ItemsPerPage = 5,
                    };
                    var result = await toDoRepository.GetPageList(filter);

                    // Assert
                    Assert.Equal(2, result.Count);
                    Assert.Equal("Dry", result.Results.ToList()[0].Name);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task GetPageList_SortByDueAtAscending_ReturnValidSort()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 2, Name = "Dry", Description = "Hang clothes to dry", DueAt = new DateTime(2023, 4, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 6, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    // Act
                    var filter = new ToDoPageFilter
                    {
                        PageNumber = 1,
                        ItemsPerPage = 5,
                        SortColumn = "DueAt",
                        SortDirection = "asc"
                    };
                    var result = await toDoRepository.GetPageList(filter);

                    // Assert
                    Assert.Equal(2, result.Count);
                    Assert.Equal("Dry", result.Results.ToList()[0].Name);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task GetPageList_SortByNameDescending_ReturnValidSort()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 2, Name = "Dry", Description = "Hang clothes to dry", DueAt = new DateTime(2023, 4, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 6, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    // Act
                    var filter = new ToDoPageFilter
                    {
                        PageNumber = 1,
                        ItemsPerPage = 5,
                        SortColumn = "Name",
                        SortDirection = "desc"
                    };
                    var result = await toDoRepository.GetPageList(filter);

                    // Assert
                    Assert.Equal(2, result.Count);
                    Assert.Equal("Dry", result.Results.ToList()[0].Name);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task GetPageList_SortByStatusAscending_ReturnValidSort()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 2, Name = "Dry", Description = "Hang clothes to dry", DueAt = new DateTime(2023, 4, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.Completed, AddAt = new DateTime(2023, 6, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    // Act
                    var filter = new ToDoPageFilter
                    {
                        PageNumber = 1,
                        ItemsPerPage = 5,
                        SortColumn = "Status",
                        SortDirection = "asc"
                    };
                    var result = await toDoRepository.GetPageList(filter);

                    // Assert
                    Assert.Equal(2, result.Count);
                    Assert.Equal("Clean", result.Results.ToList()[0].Name);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task GetPageList_Page2_ReturnValidRecords()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 2, Name = "Dry", Description = "Hang clothes to dry", DueAt = new DateTime(2023, 4, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.Completed, AddAt = new DateTime(2023, 6, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    // Act
                    var filter = new ToDoPageFilter
                    {
                        PageNumber = 2,
                        ItemsPerPage = 1,
                    };
                    var result = await toDoRepository.GetPageList(filter);

                    // Assert
                    Assert.Equal(1, result.Count);
                    Assert.Equal("Clean", result.Results.ToList()[0].Name);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        [Fact]
        public async Task GetPageList_Page0_ReturnAllRecords()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(dbContextOptions))
            {
                ResetDatabase(sleekFlowDbContext);
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 1, Name = "Clean", Description = "Place clothes in washing machine", DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.NotStarted, AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.ToDos.AddAsync(new ToDo { Id = 2, Name = "Dry", Description = "Hang clothes to dry", DueAt = new DateTime(2023, 4, 27, 0, 0, 0, DateTimeKind.Utc), Status = Status.Completed, AddAt = new DateTime(2023, 6, 25, 0, 0, 0, DateTimeKind.Utc), AddBy = "System" });
                await sleekFlowDbContext.SaveChangesAsync();
                var toDoRepository = new ToDoRepository(sleekFlowDbContext);
                if (toDoRepository != null)
                {
                    // Act
                    var filter = new ToDoPageFilter
                    {
                        PageNumber = 0,
                        ItemsPerPage = 1,
                    };
                    var result = await toDoRepository.GetPageList(filter);

                    // Assert
                    Assert.Equal(2, result.Count);
                }
                else
                {
                    Assert.NotNull(toDoRepository);
                }
            }
        }

        private void ResetDatabase(SleekFlowDbContext sleekFlowDbContext)
        {
            sleekFlowDbContext.Database.EnsureDeleted();
            sleekFlowDbContext.Database.EnsureCreated();
        }
    }
}
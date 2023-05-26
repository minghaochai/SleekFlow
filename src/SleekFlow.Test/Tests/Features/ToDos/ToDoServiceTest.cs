using AutoMapper;
using Moq;
using SleekFlow.Application.Common.Exceptions;
using SleekFlow.Application.Features.ToDos;
using SleekFlow.Domain.Entities;
using SleekFlow.Domain.Enums;
using SleekFlow.Domain.Filters;
using SleekFlow.Domain.Interfaces;
using SleekFlow.Infrastructure;
using SleekFlow.Test.Core;
using Xunit;

namespace SleekFlow.Test.Tests.Features.ToDos
{
    public class ToDoServiceTest : TestBase
    {
        private readonly string user = "System";
        private readonly ToDo ToDo1 = new ToDo
        {
            Id = 1,
            Name = "Clean",
            Description = "Place clothes in washing machine",
            DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc),
            Status = Status.NotStarted,
            AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc),
            AddBy = "System"
        };

        private readonly ToDo ToDo2 = new ToDo
        {
            Id = 2,
            Name = "Dry",
            Description = "Hang clothes to dry",
            DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc),
            Status = Status.NotStarted,
            AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc),
            AddBy = "System"
        };

        private readonly ToDoResponse ToDoResponse1 = new ToDoResponse
        {
            Id = 1,
            Name = "Clean",
            Description = "Place clothes in washing machine",
            DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc),
            Status = Status.NotStarted,
            AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc),
            AddBy = "System"
        };

        private readonly ToDoResponse ToDoResponse2 = new ToDoResponse
        {
            Id = 2,
            Name = "Dry",
            Description = "Hang clothes to dry",
            DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc),
            Status = Status.NotStarted,
            AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc),
            AddBy = "System"
        };

        [Fact]
        public async Task GetToDoById_Id1_ReturnRecord()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(DbContextOptions))
            {
                var mockToDoRepository = new Mock<IBaseRepository<ToDo>>();
                mockToDoRepository.Setup(x => x.GetById(1))
                    .ReturnsAsync(() => ToDo1);
                var mockMapper = new Mock<IMapper>();
                mockMapper.Setup(x => x.Map<ToDoResponse>(It.IsAny<ToDo>()))
                    .Returns(() => ToDoResponse1);
                var toDoService = new ToDoService(mockToDoRepository.Object, mockMapper.Object);
                if (toDoService != null)
                {
                    // Act
                    var result = await toDoService.GetToDoById(1);
                    // Assert
                    Assert.NotNull(result);
                    Assert.Equal(1, result.Id);
                    Assert.Equal("Clean", result.Name);
                }
                else
                {
                    Assert.NotNull(toDoService);
                }
            }
        }

        [Fact]
        public async Task GetToDoById_Id1_ThrowException()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(DbContextOptions))
            {
                var mockToDoRepository = new Mock<IBaseRepository<ToDo>>();
                mockToDoRepository.Setup(x => x.GetById(1))
                    .ReturnsAsync(() =>
                        null
                    );
                var mockMapper = new Mock<IMapper>();
                var toDoService = new ToDoService(mockToDoRepository.Object, mockMapper.Object);
                if (toDoService != null)
                {
                    // Act
                    var ex = await Assert.ThrowsAsync<NotFoundException>(() => toDoService.GetToDoById(1));

                    // Assert
                    Assert.Equal("NotFoundException", ex.GetType().Name);
                    Assert.Equal("To do not found", ex.Message);
                }
                else
                {
                    Assert.NotNull(toDoService);
                }
            }
        }

        [Fact]
        public async Task GetPageList_DateRangeBetweenParam_ReturnValidRecords()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(DbContextOptions))
            {
                var filter = new ToDoPageFilter();
                var mockToDoRepository = new Mock<IBaseRepository<ToDo>>();
                mockToDoRepository.Setup(x => x.GetPageList(filter))
                    .ReturnsAsync(() =>
                        {
                            return (new List<ToDo>()
                            {
                                ToDo1,
                                ToDo2
                            }
                            , 2);
                        }
                    );
                var mockMapper = new Mock<IMapper>();
                mockMapper.Setup(x => x.Map<ToDoPageFilter>(It.IsAny<ToDoPageFilterRequest>()))
                    .Returns(() => filter);
                mockMapper.Setup(x => x.Map<IEnumerable<ToDoResponse>>(It.IsAny<IEnumerable<ToDo>>()))
                    .Returns(() =>
                        new List<ToDoResponse>
                        {
                            ToDoResponse1,
                            ToDoResponse2
                        }
                    );
                var toDoService = new ToDoService(mockToDoRepository.Object, mockMapper.Object);
                if (toDoService != null)
                {
                    // Act
                    var result = await toDoService.GetToDoPageList(new ToDoPageFilterRequest());

                    // Assert
                    Assert.Equal(2, result.Count);
                    Assert.Equal(2, result.Results.Count());
                }
                else
                {
                    Assert.NotNull(toDoService);
                }
            }
        }

        [Fact]
        public async Task InsertToDo_NewToDo_ReturnSuccess()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(DbContextOptions))
            {
                var toDo = new ToDo();
                var mockToDoRepository = new Mock<IBaseRepository<ToDo>>();
                mockToDoRepository.Setup(x => x.Insert(toDo))
                    .ReturnsAsync(() => 1);
                var mockMapper = new Mock<IMapper>();
                mockMapper.Setup(x => x.Map<ToDo>(It.IsAny<ToDoRequest>()))
                    .Returns(() => toDo);
                mockMapper.Setup(x => x.Map<ToDoResponse>(It.IsAny<ToDo>()))
                    .Returns(() => ToDoResponse1);
                var toDoService = new ToDoService(mockToDoRepository.Object, mockMapper.Object);
                if (toDoService != null)
                {
                    // Act
                    var result = await toDoService.InsertToDo(user, new ToDoRequest());

                    // Assert
                    Assert.NotNull(result);
                    Assert.Equal("Clean", result.Name);
                }
                else
                {
                    Assert.NotNull(toDoService);
                }
            }
        }

        [Fact]
        public async Task InsertToDo_ExistingToDo_ThrowException()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(DbContextOptions))
            {
                var toDo = new ToDo();
                var mockToDoRepository = new Mock<IBaseRepository<ToDo>>();
                mockToDoRepository.Setup(x => x.Insert(toDo))
                    .ReturnsAsync(() => throw new ArgumentException());
                var mockMapper = new Mock<IMapper>();
                mockMapper.Setup(x => x.Map<ToDo>(It.IsAny<ToDoRequest>()))
                    .Returns(() => toDo);
                mockMapper.Setup(x => x.Map<ToDoResponse>(It.IsAny<ToDo>()))
                    .Returns(() => ToDoResponse1);
                var toDoService = new ToDoService(mockToDoRepository.Object, mockMapper.Object);
                if (toDoService != null)
                {
                    // Act
                    var ex = await Assert.ThrowsAsync<ArgumentException>(() => toDoService.InsertToDo(user, new ToDoRequest()));

                    // Assert
                    Assert.Equal("ArgumentException", ex.GetType().Name);
                }
                else
                {
                    Assert.NotNull(toDoService);
                }
            }
        }

        [Fact]
        public async Task UpdateToDo_ToDoExists_ReturnSuccess()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(DbContextOptions))
            {
                var toDo = new ToDo();
                var mockToDoRepository = new Mock<IBaseRepository<ToDo>>();
                mockToDoRepository.Setup(x => x.GetById(1))
                   .ReturnsAsync(() => ToDo1);
                mockToDoRepository.Setup(x => x.Update(toDo))
                    .ReturnsAsync(() => 1);
                var mockMapper = new Mock<IMapper>();
                mockMapper.Setup(x => x.Map<ToDo>(It.IsAny<ToDoRequest>()))
                    .Returns(() => toDo);
                mockMapper.Setup(x => x.Map<ToDoResponse>(It.IsAny<ToDo>()))
                    .Returns(() => ToDoResponse1);
                var toDoService = new ToDoService(mockToDoRepository.Object, mockMapper.Object);
                if (toDoService != null)
                {
                    // Act
                    var result = await toDoService.UpdateToDo(user, 1, new ToDoRequest());

                    // Assert
                    Assert.NotNull(result);
                    Assert.Equal("Clean", result.Name);
                }
                else
                {
                    Assert.NotNull(toDoService);
                }
            }
        }

        [Fact]
        public async Task UpdateToDo_ToDoDoesNotExist_ThrowException()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(DbContextOptions))
            {
                var toDo = new ToDo();
                var mockToDoRepository = new Mock<IBaseRepository<ToDo>>();
                mockToDoRepository.Setup(x => x.GetById(1))
                   .ReturnsAsync(() => null);
                var mockMapper = new Mock<IMapper>();
                mockMapper.Setup(x => x.Map<ToDo>(It.IsAny<ToDoRequest>()))
                    .Returns(() => toDo);
                var toDoService = new ToDoService(mockToDoRepository.Object, mockMapper.Object);
                if (toDoService != null)
                {
                    // Act
                    var ex = await Assert.ThrowsAsync<NotFoundException>(() => toDoService.UpdateToDo(user, 1, new ToDoRequest()));

                    // Assert
                    Assert.Equal("NotFoundException", ex.GetType().Name);
                    Assert.Equal("To do not found", ex.Message);
                }
                else
                {
                    Assert.NotNull(toDoService);
                }
            }
        }

        [Fact]
        public async Task DeleteToDo_ToDoExists_ReturnTrue()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(DbContextOptions))
            {
                var toDo = new ToDo();
                var mockToDoRepository = new Mock<IBaseRepository<ToDo>>();
                mockToDoRepository.Setup(x => x.GetById(1))
                    .ReturnsAsync(() => ToDo1);
                mockToDoRepository.Setup(x => x.Delete(toDo))
                    .ReturnsAsync(() => 1);
                var mockMapper = new Mock<IMapper>();
                var toDoService = new ToDoService(mockToDoRepository.Object, mockMapper.Object);
                if (toDoService != null)
                {
                    // Act
                    var result = await toDoService.DeleteToDo(1);

                    // Assert
                    Assert.True(result);
                }
                else
                {
                    Assert.NotNull(toDoService);
                }
            }
        }

        [Fact]
        public async Task DeleteToDo_ToDoDoesNotExist_ThrowException()
        {
            // Arrange
            using (var sleekFlowDbContext = new SleekFlowDbContext(DbContextOptions))
            {
                var toDo = new ToDo();
                var mockToDoRepository = new Mock<IBaseRepository<ToDo>>();
                mockToDoRepository.Setup(x => x.GetById(1))
                    .ReturnsAsync(() => null);
                mockToDoRepository.Setup(x => x.Delete(toDo))
                    .ReturnsAsync(() => 1);
                var mockMapper = new Mock<IMapper>();
                var toDoService = new ToDoService(mockToDoRepository.Object, mockMapper.Object);
                if (toDoService != null)
                {
                    // Act
                    var ex = await Assert.ThrowsAsync<NotFoundException>(() => toDoService.DeleteToDo(1));

                    // Assert
                    Assert.Equal("NotFoundException", ex.GetType().Name);
                    Assert.Equal("To do not found", ex.Message);
                }
                else
                {
                    Assert.NotNull(toDoService);
                }
            }
        }
    }
}

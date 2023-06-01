using Helper.Http;
using Microsoft.Extensions.DependencyInjection;
using SleekFlow.Application.Common.Dtos;
using SleekFlow.Application.Features.ToDos;
using SleekFlow.Domain.Entities;
using SleekFlow.Domain.Enums;
using SleekFlow.Infrastructure;
using SleekFlow.IntegrationTest.Helpers;
using System.Net;
using Xunit;

namespace SleekFlow.IntegrationTest.Tests.Features.ToDos
{
    public class ToDoControllerTest
    : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly string _tableName = "[dbo].[ToDos]";
        private readonly List<ToDo> _seedingData = new List<ToDo>()
        {
            new ToDo {
                Id = 1,
                Name = "Clean",
                Description = "Place clothes in washing machine",
                DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc),
                Status = Status.InProgress,
                AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc),
                AddBy = "System"
            },
            new ToDo {
                Id = 2,
                Name = "Dry",
                Description = "Hang clothes to dry",
                DueAt = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc),
                Status = Status.NotStarted, 
                AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc),
                AddBy = "System"
            },
            new ToDo {
                Id = 3,
                Name = "Iron",
                Description = "Iron clothes",
                DueAt = new DateTime(2023, 5, 28, 0, 0, 0, DateTimeKind.Utc),
                Status = Status.NotStarted,
                AddAt = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc),
                AddBy = "System"
            },
        };

        public ToDoControllerTest(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/todos/1", 1)]
        [InlineData("/todos/2", 2)]
        [InlineData("/todos/3", 3)]
        public async Task GetById_ValidIds_EndpointsReturnSuccessAndCorrectRecords(string url, int id)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.ReinitializeDb(db, _seedingData, _tableName);
            }
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);
            var result = response.CastToModel<ToDoResponse>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(id, result.Id);
        }

        [Theory]
        [InlineData("/todos/1")]
        [InlineData("/todos/2")]
        [InlineData("/todos/3")]
        public async Task GetById_InvalidIds_EndpointsReturnNotFoundAndErrorResponse(string url)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.DropAndRecreateDb(db);
            }
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);
            var result = response.CastToModel<ErrorResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("To do not found", result.Errors![0].Message);
        }

        [Theory]
        [InlineData("/todos", 0)]
        [InlineData("/todos", 1)]
        [InlineData("/todos", 2)]
        public async Task Insert_ValidToDo_EndpointsReturnCreatedAndCreatedToDo(string url, int seedingDataIndex)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.DropAndRecreateDb(db);
            }
            var client = _factory.CreateClient();
            var urlParam = "?user=Test";
            // Act
            var response = await client.PostAsync($"{url}{urlParam}", _seedingData[seedingDataIndex].CastToHttpContent());
            var result = response.CastToModel<ToDoResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(_seedingData[seedingDataIndex].Name, result.Name);
        }

        [Theory]
        [InlineData("/todos")]
        public async Task Insert_InvalidToDo_EndpointsReturnBadRequest(string url)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.DropAndRecreateDb(db);
            }
            var client = _factory.CreateClient();
            var urlParam = "?user=Test";
            // Act
            var response = await client.PostAsync($"{url}{urlParam}", new ToDo() { }.CastToHttpContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("/todos/1", 0)]
        [InlineData("/todos/2", 1)]
        [InlineData("/todos/3", 2)]
        public async Task Update_ExistingToDo_EndpointsReturnSuccessAndUpdatedRecord(string url, int seedingDataIndex)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.ReinitializeDb(db, _seedingData, _tableName);
            }
            var client = _factory.CreateClient();
            var urlParam = "?user=Test";
            var updatedToDo = _seedingData[seedingDataIndex];
            updatedToDo.Name = $"Updated {updatedToDo.Name}";

            // Act
            var response = await client.PutAsync($"{url}{urlParam}", updatedToDo.CastToHttpContent());
            var result = response.CastToModel<ToDoResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updatedToDo.Name, result.Name);
            Assert.Equal("Test", result.EditBy);
        }

        [Theory]
        [InlineData("/todos/1", 0)]
        public async Task Update_ToDoDoesNotExist_EndpointsReturnSuccessAndUpdatedRecord(string url, int seedingDataIndex)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.DropAndRecreateDb(db);
            }
            var client = _factory.CreateClient();
            var urlParam = "?user=Test";
            var updatedToDo = _seedingData[seedingDataIndex];
            updatedToDo.Name = $"Updated {updatedToDo.Name}";

            // Act
            var response = await client.PutAsync($"{url}{urlParam}", updatedToDo.CastToHttpContent());
            var result = response.CastToModel<ErrorResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("To do not found", result.Errors![0].Message);
        }

        [Theory]
        [InlineData("/todos/1")]
        public async Task Update_InvalidToDo_EndpointsReturnBadRequest(string url)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.DropAndRecreateDb(db);
            }
            var client = _factory.CreateClient();
            var urlParam = "?user=Test";
            // Act
            var response = await client.PutAsync($"{url}{urlParam}", new ToDo() { }.CastToHttpContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("/todos/1")]
        public async Task Delete_ToDoExists_EndpointsReturnSuccess(string url)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.DropAndRecreateDb(db);
                Utilities<ToDo>.InitializeDb(db, _seedingData, _tableName);
            }
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("/todos/1")]
        public async Task Delete_ToDoDoesNotExist_EndpointsReturnNotFoundAndErrorResponse(string url)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.DropAndRecreateDb(db);
            }
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync(url);
            var result = response.CastToModel<ErrorResponse>();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("To do not found", result.Errors![0].Message);
        }

        [Theory]
        [InlineData("/todos")]
        public async Task Delete_IncorrectParam_EndpointsReturnMethodNotAllowed(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Theory]
        [InlineData("/todos", 1, 1)]
        [InlineData("/todos", 1, 2)]
        [InlineData("/todos", 1, 3)]
        [InlineData("/todos", 2, 1)]
        public async Task Get_PaginationQuery_EndpointsReturnSuccessAndRecord(string url, int pageNumber, int itemsPerPage)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.ReinitializeDb(db, _seedingData, _tableName);
            }
            var client = _factory.CreateClient();
            var urlParam = $"?PageNumber={pageNumber}&ItemsPerPage={itemsPerPage}";

            // Act
            var response = await client.GetAsync($"{url}{urlParam}");
            var result = response.CastToModel<PageResponse<ToDoResponse>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(pageNumber, result.PageInfo.PageNumber);
            Assert.Equal(itemsPerPage, result.PageInfo.ItemsPerPage);
            Assert.Equal(3, result.PageInfo.TotalItems);
            Assert.Equal(itemsPerPage, result.Data.Count);
        }

        [Theory]
        [InlineData("/todos", 0, 2)]
        [InlineData("/todos", 1, 1)]
        public async Task Get_StatusFilterQuery_EndpointsReturnSuccessAndRecord(string url, int status, int expectedCount)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.ReinitializeDb(db, _seedingData, _tableName);
            }
            var client = _factory.CreateClient();
            var urlParam = $"?PageNumber=1&ItemsPerPage=5&Status={status}";

            // Act
            var response = await client.GetAsync($"{url}{urlParam}");
            var result = response.CastToModel<PageResponse<ToDoResponse>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedCount, result.Data.Count);
            Assert.Equal(status, (int)result.Data[0].Status!.Value);
        }

        [Theory]
        [InlineData("/todos", "2023-05-26", "2023-05-27", 2)]
        [InlineData("/todos", "2023-05-28", "2023-05-28", 1)]
        [InlineData("/todos", "2023-10-27", "2023-10-28", 0)]
        public async Task Get_DateRangeFilterQuery_EndpointsReturnSuccessAndRecord(string url, string startDate, string endDate, int expectedCount)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.ReinitializeDb(db, _seedingData, _tableName);
            }
            var client = _factory.CreateClient();
            var urlParam = $"?PageNumber=1&ItemsPerPage=5&DueAtStart={startDate}&DueAtEnd={endDate}";

            // Act
            var response = await client.GetAsync($"{url}{urlParam}");
            var result = response.CastToModel<PageResponse<ToDoResponse>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedCount, result.Data.Count);
        }

        [Theory]
        [InlineData("/todos", "asc", "Clean")]
        [InlineData("/todos", "desc", "Iron")]
        public async Task Get_SortByName_EndpointsReturnSuccessAndCorrectRecordSequence(string url, string sortDirection, string expectedName)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.ReinitializeDb(db, _seedingData, _tableName);
            }
            var client = _factory.CreateClient();
            var urlParam = $"?PageNumber=1&ItemsPerPage=5&SortColumn=Name&SortDirection={sortDirection}";

            // Act
            var response = await client.GetAsync($"{url}{urlParam}");
            var result = response.CastToModel<PageResponse<ToDoResponse>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedName, result.Data[0].Name);
        }

        [Theory]
        [InlineData("/todos", "asc", 0)]
        [InlineData("/todos", "desc", 1)]
        public async Task Get_SortByStatus_EndpointsReturnSuccessAndCorrectRecordSequence(string url, string sortDirection, int expectedStatus)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.ReinitializeDb(db, _seedingData, _tableName);
            }
            var client = _factory.CreateClient();
            var urlParam = $"?PageNumber=1&ItemsPerPage=5&SortColumn=Status&SortDirection={sortDirection}";

            // Act
            var response = await client.GetAsync($"{url}{urlParam}");
            var result = response.CastToModel<PageResponse<ToDoResponse>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedStatus, (int)result.Data[0].Status!.Value);
        }

        [Theory]
        [InlineData("/todos", "asc", "Clean")]
        [InlineData("/todos", "desc", "Iron")]
        public async Task Get_SortByDueAt_EndpointsReturnSuccessAndCorrectRecordSequence(string url, string sortDirection, string expectedName)
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SleekFlowDbContext>();
                Utilities<ToDo>.ReinitializeDb(db, _seedingData, _tableName);
            }
            var client = _factory.CreateClient();
            var urlParam = $"?PageNumber=1&ItemsPerPage=5&SortColumn=DueAt&SortDirection={sortDirection}";

            // Act
            var response = await client.GetAsync($"{url}{urlParam}");
            var result = response.CastToModel<PageResponse<ToDoResponse>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedName, result.Data[0].Name);
        }
    }
}
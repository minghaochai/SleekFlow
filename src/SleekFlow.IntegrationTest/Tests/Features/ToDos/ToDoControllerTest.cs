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
                Status = Status.NotStarted,
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
            var result = response.CastToModel<ToDoResponse>();

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
    }
}

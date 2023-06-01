using Microsoft.AspNetCore.Mvc;
using SleekFlow.Application.Common.Dtos;
using SleekFlow.Application.Features.ToDos;
using System.ComponentModel.DataAnnotations;

namespace SleekFlow.Api.Controllers
{
    [ApiController]
    [Route("todos")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        /// <summary>
        /// Retrieves a To Do record by id
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<ToDoResponse>> GetById(int id)
        {
            var res = await _toDoService.GetToDoById(id);
            return Ok(res);
        }

        /// <summary>
        /// Creates a To Do record
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ToDoResponse>> Insert([FromBody] ToDoRequest request, [FromQuery] [Required] string user)
        {
            var res = await _toDoService.InsertToDo(user, request);
            return CreatedAtAction(nameof(GetById), new { id = res.Id }, res);
        }

        /// <summary>
        /// Updates a To Do record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="user"></param>
        [HttpPut("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<ToDoResponse>> Update(int id, [FromBody] ToDoRequest request, [FromQuery] [Required] string user)
        {
            var result = await _toDoService.UpdateToDo(user, id, request);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a To Do record by id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> Delete(int id)
        {
            await _toDoService.DeleteToDo(id);
            return Ok();
        }

        /// <summary>
        /// Gets the a paged list of To Do records based on the paramaters passed
        /// </summary>
        /// <param name="request"></param>
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<PageResponse<ToDoResponse>>> Get([FromQuery] ToDoPageFilterRequest request)
        {
            var (results, count) = await _toDoService.GetToDoPageList(request);
            var response = new PageResponse<ToDoResponse>
            {
                Data = results.ToList(),
            };
            response.PageInfo.ItemsPerPage = request.ItemsPerPage;
            response.PageInfo.PageNumber = request.PageNumber;
            response.PageInfo.TotalItems = count;

            return Ok(response);
        }
    }
}

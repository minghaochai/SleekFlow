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

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<ToDoResponse>> GetById(int id)
        {
            var res = await _toDoService.GetToDoById(id);
            return Ok(res);
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ToDoResponse>> Insert([FromBody] ToDoRequest request, [FromQuery] [Required] string user)
        {
            var res = await _toDoService.InsertToDo(user, request);
            return CreatedAtAction(nameof(GetById), new { id = res.Id }, res);
        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<ToDoResponse>> Update(int id, [FromBody] ToDoRequest request, [FromQuery] [Required] string user)
        {
            var result = await _toDoService.UpdateToDo(user, id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> Delete(int id)
        {
            await _toDoService.DeleteToDo(id);
            return Ok();
        }

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

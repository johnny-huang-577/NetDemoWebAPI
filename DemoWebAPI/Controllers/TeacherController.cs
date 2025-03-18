using DemoWebAPI.Models.Dto.Response;
using DemoWebAPI.Models.Dto.Teacher;
using DemoWebAPI.Models.Entity;
using DemoWebAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        
        private readonly ITeacherService _teacherService;

        private ResponseDTO _responseDTO;

        private OperationResultDTO _operationResultDTO;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
            _responseDTO = new ResponseDTO();
            _operationResultDTO = new OperationResultDTO();
        }

        // GET: api/<TeacherController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> GetTeachersAsync()
        {

            _operationResultDTO = await _teacherService.GetAllTeachersAsync();

            if (!_operationResultDTO.OperationSuccess)
            {
                _responseDTO.StatusCode = HttpStatusCode.NotFound;
                _responseDTO.Message = "找無資料";
                _responseDTO.RequestSuccess = false;

                return NotFound(_responseDTO);
            }

            _responseDTO.StatusCode = HttpStatusCode.OK;
            _responseDTO.Message = "取得資料成功";
            _responseDTO.RequestSuccess = true;
            _responseDTO.ResponseBody = _operationResultDTO;

            return Ok(_responseDTO);
        }

        // GET api/<TeacherController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> GetTeacherAsync(int id)
        {
            _operationResultDTO = await _teacherService.GetTeacherByIdAsync(id);

            if (!_operationResultDTO.OperationSuccess)
            {
                _responseDTO.StatusCode = HttpStatusCode.NotFound;
                _responseDTO.Message = "找無該筆資料";
                _responseDTO.RequestSuccess = false;
                return NotFound(_responseDTO);
            }

            _responseDTO.StatusCode = HttpStatusCode.OK;
            _responseDTO.Message = "取得資料成功";
            _responseDTO.RequestSuccess = true;
            _responseDTO.ResponseBody = _operationResultDTO;

            return Ok(_responseDTO);
        }

        // POST api/<TeacherController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ResponseDTO>> CreateTeacherAsync([FromBody] TeacherDetailDTO teacherDetailDTO)
        {

            _operationResultDTO =  await _teacherService.CreateTeacherAsync(teacherDetailDTO);
            _responseDTO.StatusCode = HttpStatusCode.Created;
            _responseDTO.Message = "新增成功";
            _responseDTO.RequestSuccess = true;
            _responseDTO.ResponseBody = _operationResultDTO;

            return Ok(_responseDTO);
            //return CreatedAtAction(nameof(GetTeacherAsync), new { id = teacherDetailDTO.Id }, _responseDTO);
        } 

        // PUT api/<TeacherController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> Put(int id, [FromBody] TeacherDetailDTO teacherDetailDTO)
        {
            _operationResultDTO = await _teacherService.UpdateTeacherAsync(teacherDetailDTO);
            if(!_operationResultDTO.OperationSuccess)
            {
                _responseDTO.StatusCode = HttpStatusCode.NotFound;
                _responseDTO.Message = "找無該筆資料，更新失敗";
                _responseDTO.RequestSuccess = false;
                return NotFound(_responseDTO);
            }


            _responseDTO.StatusCode = HttpStatusCode.OK;
            _responseDTO.Message = "更新成功";
            _responseDTO.RequestSuccess = true;
            _responseDTO.ResponseBody = _operationResultDTO;

            return Ok(_responseDTO);
        }

        // DELETE api/<TeacherController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDTO>> Delete(int id)
        {
            _operationResultDTO = await _teacherService.DeleteTeacherAsync(id);
            if (!_operationResultDTO.OperationSuccess)
            {
                _responseDTO.StatusCode = HttpStatusCode.NotFound;
                _responseDTO.Message = "找無該筆資料，刪除失敗";
                _responseDTO.RequestSuccess = false;
                return NotFound(_responseDTO);
            }

            _responseDTO.StatusCode = HttpStatusCode.OK;
            _responseDTO.Message = "刪除成功";
            _responseDTO.RequestSuccess = true;
            _responseDTO.ResponseBody = _operationResultDTO;
            return Ok(_responseDTO);
        }
    }
}

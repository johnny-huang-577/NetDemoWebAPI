using DemoWebAPI.Models.Dto.Response;
using DemoWebAPI.Models.Dto.Teacher;
using DemoWebAPI.Models.Entity;
using DemoWebAPI.Services.IServices;
using Mapster;
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
        /// <summary>
        /// 取得所有老師資料
        /// </summary>
        /// <returns>包含所有老師資料的回應</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> GetTeachersAsync()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();

            if (teachers == null)
            {
                _responseDTO.StatusCode = HttpStatusCode.NotFound;
                _responseDTO.Message = "找無資料";
                _responseDTO.RequestSuccess = false;

                return NotFound(_responseDTO);
            }

            _responseDTO.StatusCode = HttpStatusCode.OK;
            _responseDTO.Message = "取得資料成功";
            _responseDTO.RequestSuccess = true;
            _responseDTO.Data = teachers;

            return Ok(_responseDTO);
        }


        /// <summary>
        /// 取得特定老師資料
        /// </summary>
        /// <param name="id">老師ID</param>
        /// <returns>包含特定老師資料的回應</returns>
        // GET api/<TeacherController>/5
        [HttpGet("{id}", Name = "GetTeacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> GetTeacherAsync(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);

            if (teacher == null)
            {
                _responseDTO.StatusCode = HttpStatusCode.NotFound;
                _responseDTO.Message = "找無該筆資料";
                _responseDTO.RequestSuccess = false;
                return NotFound(_responseDTO);
            }

            _responseDTO.StatusCode = HttpStatusCode.OK;
            _responseDTO.Message = "取得資料成功";
            _responseDTO.RequestSuccess = true;
            _responseDTO.Data = teacher;

            return Ok(_responseDTO);
        }

        /// <summary>
        /// 新增老師資料
        /// </summary>
        /// <param name="teacherDetailDTO">老師詳細資料</param>
        /// <returns>新增結果的回應</returns>
        // POST api/<TeacherController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDTO>> CreateTeacherAsync([FromBody] TeacherDetailDTO teacherDetailDTO)
        {

            _operationResultDTO =  await _teacherService.CreateTeacherAsync(teacherDetailDTO);

            if (!_operationResultDTO.Success)
            {
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                _responseDTO.Message = "新增失敗";
                _responseDTO.RequestSuccess = false;
                _responseDTO.Data = _operationResultDTO.Errors;
                return BadRequest(_responseDTO);
            }

            // 新老師的 id 是在資料庫中自動生成的，從 _operationResultDTO.Data 中獲取新生成的 id
            var newTeacherId = _operationResultDTO.Data.Adapt<TeacherDetailDTO>().Id;

            _responseDTO.StatusCode = HttpStatusCode.Created;
            _responseDTO.Message = "新增成功";
            _responseDTO.RequestSuccess = true;
  
            return CreatedAtRoute("GetTeacher", new { id = newTeacherId }, _responseDTO);
        }


        /// <summary>
        /// 更新老師資料
        /// </summary>
        /// <param name="teacherDetailDTO">老師詳細資料</param>
        /// <returns>更新結果的回應</returns>
        // PUT api/<TeacherController>/5
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> Put([FromBody] TeacherDetailDTO teacherDetailDTO)
        {
            _operationResultDTO = await _teacherService.UpdateTeacherAsync(teacherDetailDTO);
            if(!_operationResultDTO.Success)
            {
                _responseDTO.StatusCode = HttpStatusCode.NotFound;
                _responseDTO.Message = "找無該筆資料，更新失敗";
                _responseDTO.RequestSuccess = false;
                return NotFound(_responseDTO);
            }


            _responseDTO.StatusCode = HttpStatusCode.OK;
            _responseDTO.Message = "更新成功";
            _responseDTO.RequestSuccess = true;

            return Ok(_responseDTO);
        }


        /// <summary>
        /// 刪除老師資料
        /// </summary>
        /// <param name="id">老師ID</param>
        /// <returns>刪除結果的回應</returns>
        // DELETE api/<TeacherController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> Delete(int id)
        {
            _operationResultDTO = await _teacherService.DeleteTeacherAsync(id);
            if (!_operationResultDTO.Success)
            {
                _responseDTO.StatusCode = HttpStatusCode.NotFound;
                _responseDTO.Message = "找無該筆資料，刪除失敗";
                _responseDTO.RequestSuccess = false;
                return NotFound(_responseDTO);
            }

            _responseDTO.StatusCode = HttpStatusCode.OK;
            _responseDTO.Message = "刪除成功";
            _responseDTO.RequestSuccess = true;

            return Ok(_responseDTO);
        }
    }
}

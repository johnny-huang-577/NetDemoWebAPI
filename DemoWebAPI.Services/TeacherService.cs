using DemoWebAPI.DataAccess.Repository.IRepository;
using DemoWebAPI.Models.Dto.Response;
using DemoWebAPI.Models.Dto.Teacher;
using DemoWebAPI.Models.Entity;
using DemoWebAPI.Services.IServices;

using Mapster;

namespace DemoWebAPI.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private OperationResultDTO _operationResultDTO;
        public TeacherService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _operationResultDTO = new OperationResultDTO();
        }

        //取得所有老師
        public async Task<OperationResultDTO> GetAllTeachersAsync()
        {
            var teachers = await _unitOfWork.Teacher.GetAllAsync();

            //如果沒有老師資料，回傳失敗
            if ( !teachers.Any() ) {
                _operationResultDTO.OperationSuccess = false;
                return _operationResultDTO;
            }
            // 有老師資料，回傳成功
            _operationResultDTO.OperationSuccess = true;
            _operationResultDTO.Data = teachers.Adapt<IEnumerable<TeacherDTO>>();
            return _operationResultDTO;

        }
        //取得特定老師
        public async Task<OperationResultDTO> GetTeacherByIdAsync(int id)
        {
            var teacher = await _unitOfWork.Teacher.GetAsync(t => t.Id == id);

            //如果沒有老師資料，回傳失敗
            if (teacher == null)
            {
                _operationResultDTO.OperationSuccess = false;
                return _operationResultDTO;
            }
            // 有老師資料，回傳成功
            _operationResultDTO.OperationSuccess = true;
            _operationResultDTO.Data = teacher.Adapt<TeacherDetailDTO>();


            return _operationResultDTO;
        }

        //新增老師
        public async Task<OperationResultDTO> CreateTeacherAsync(TeacherDetailDTO teacherDetailDTO)
        {

            var newTeacher = teacherDetailDTO.Adapt<Teacher>();
            await _unitOfWork.Teacher.AddAsync(newTeacher);
            await _unitOfWork.SaveAsync();

            _operationResultDTO.OperationSuccess = true;
            return _operationResultDTO;
        }

        //更新老師
        public async Task<OperationResultDTO> UpdateTeacherAsync(TeacherDetailDTO teacherDetailDTO)
        {
            var teacher = await _unitOfWork.Teacher.GetAsync(t => t.Id == teacherDetailDTO.Id);
            //如果沒有老師資料，回傳失敗
            if (teacher == null)
            {
                _operationResultDTO.OperationSuccess = false;
                return _operationResultDTO;
            }
            
            //將 teacherDetailDTO 物件的屬性值複製到 teacher 物件中。
            teacherDetailDTO.Adapt(teacher);

            //更新老師資料
            await _unitOfWork.Teacher.UpdateAsync(teacher);
            await _unitOfWork.SaveAsync();

            _operationResultDTO.OperationSuccess = true;
            return _operationResultDTO;
        }

        //刪除老師
        public async Task<OperationResultDTO> DeleteTeacherAsync(int id)
        {
            var teacher = await _unitOfWork.Teacher.GetAsync(t => t.Id == id);

            //如果沒有老師資料，回傳失敗
            if (teacher == null)
            {
                _operationResultDTO.OperationSuccess = false;
                return _operationResultDTO;
            }

            //刪除老師資料
            await _unitOfWork.Teacher.RemoveAsync(teacher);
            await _unitOfWork.SaveAsync();


            _operationResultDTO.OperationSuccess = true;
            return _operationResultDTO;

        }

    }
}
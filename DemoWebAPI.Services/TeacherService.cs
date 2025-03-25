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
        public async Task<List<TeacherDTO>?> GetAllTeachersAsync()
        {
            var teachers = await _unitOfWork.Teacher.GetAllAsync();

            //如果沒有老師資料，回傳null
            if ( !teachers.Any() ) {

                return null;
            }

            return teachers.Adapt<List<TeacherDTO>>();

        }
        //取得特定老師
        public async Task<TeacherDetailDTO?> GetTeacherByIdAsync(int id)
        {
            var teacher = await _unitOfWork.Teacher.GetAsync(t => t.Id == id);

            //如果沒有指定老師的資料，回傳null
            if (teacher == null)
            {
                return null;
            }
  
            return teacher.Adapt<TeacherDetailDTO>();
        }

        //新增老師
        public async Task<OperationResultDTO> CreateTeacherAsync(TeacherDetailDTO teacherDetailDTO)
        {

            var newTeacher = teacherDetailDTO.Adapt<Teacher>();



            //加入DB前的業務邏輯驗證
            //if 科目不合法
            //   _operationResultDTO.Errors.Add("subject", "無效的科目");
            //.... return _operationResultDTO;

            //加入DB
            await _unitOfWork.Teacher.AddAsync(newTeacher);
            await _unitOfWork.SaveAsync();
            _operationResultDTO.Success = true;
            _operationResultDTO.Data = newTeacher;

            return _operationResultDTO;
        }

        //更新老師
        public async Task<OperationResultDTO> UpdateTeacherAsync(TeacherDetailDTO teacherDetailDTO)
        {
            var teacher = await _unitOfWork.Teacher.GetAsync(t => t.Id == teacherDetailDTO.Id);

            //如果沒有老師資料
            if (teacher == null)
            {
                _operationResultDTO.Errors = new Dictionary<string, string>{{ "msg", "找不到該老師資料" } };
                return _operationResultDTO;
            }
            
            //將 teacherDetailDTO 物件的屬性值複製到 teacher 物件中。
            teacherDetailDTO.Adapt(teacher);

            //更新老師資料
            await _unitOfWork.Teacher.UpdateAsync(teacher);
            await _unitOfWork.SaveAsync();
            _operationResultDTO.Success = true;

            return _operationResultDTO;
        }

        //刪除老師
        public async Task<OperationResultDTO> DeleteTeacherAsync(int id)
        {
            var teacher = await _unitOfWork.Teacher.GetAsync(t => t.Id == id);

            //如果沒有老師資料，回傳失敗
            if (teacher == null)
            {
                _operationResultDTO.Errors = new Dictionary<string, string> { { "msg", "找不到該老師資料" } };
                return _operationResultDTO;
            }

            //刪除老師資料
            await _unitOfWork.Teacher.RemoveAsync(teacher);
            await _unitOfWork.SaveAsync();
            _operationResultDTO.Success = true;

            return _operationResultDTO;

        }

    }
}
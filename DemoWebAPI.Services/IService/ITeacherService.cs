using DemoWebAPI.Models.Dto.Response;
using DemoWebAPI.Models.Dto.Teacher;
using DemoWebAPI.Models.Entity;

namespace DemoWebAPI.Services.IServices
{
    public interface ITeacherService
    {
        Task<OperationResultDTO> GetAllTeachersAsync();
        Task<OperationResultDTO> GetTeacherByIdAsync(int id);
        Task<OperationResultDTO> CreateTeacherAsync(TeacherDetailDTO teacherDatailDTO);
        Task<OperationResultDTO> UpdateTeacherAsync(TeacherDetailDTO teacherDatailDTO);
        Task<OperationResultDTO> DeleteTeacherAsync(int id);
    }
}
using DemoWebAPI.Models.Dto.Response;
using DemoWebAPI.Models.Dto.Teacher;
using DemoWebAPI.Models.Entity;

namespace DemoWebAPI.Services.IServices
{
    public interface ITeacherService
    {
        Task<List<TeacherDTO>?> GetAllTeachersAsync();
        Task<TeacherDetailDTO?> GetTeacherByIdAsync(int id);
        Task<OperationResultDTO> CreateTeacherAsync(TeacherDetailDTO teacherDatailDTO);
        Task<OperationResultDTO> UpdateTeacherAsync(TeacherDetailDTO teacherDatailDTO);
        Task<OperationResultDTO> DeleteTeacherAsync(int id);
    }
}
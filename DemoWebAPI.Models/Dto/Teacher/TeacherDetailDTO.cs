using System.ComponentModel.DataAnnotations;

namespace DemoWebAPI.Models.Dto.Teacher
{
    public class TeacherDetailDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "姓名不能為空")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "請輸入性別")]
        [RegularExpression("^[MF]$", ErrorMessage = "性別必須是 'M' 或 'F'")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "生日日期不能為空")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "電話號碼不能為空")]
        [MaxLength(15)]
        [RegularExpression(@"^\+?[0-9]{8,15}$", ErrorMessage = "請輸入有效的電話號碼")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email不能為空")]
        [EmailAddress(ErrorMessage = "請輸入有效的Email格式")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "教學科目不能為空")]
        [MaxLength(50)]
        public string Subject { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DemoWebAPI.Models.Entity
{
    public class Teacher : BaseEntity
    {
        [Key]
        public int Id { get; set; }                 // Primary key

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }            // 姓名

        [Required]
        public string Gender { get; set; }          // 性別

        [Required]
        public DateTime Birthday { get; set; }      // 生日

        [Required]
        public string Phone { get; set; }           // 聯絡電話

        [Required]
        public string Email { get; set; }           // Email

        [Required]
        public string Subject { get; set; }         // 教學的科目
    }
}

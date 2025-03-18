using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebAPI.Models.Entity
{
    public abstract class BaseEntity
    {
        [Required]
        public DateTime CreateTime { get; set; }   // 建立日期

        public DateTime? UpdateTime { get; set; }  // 修改日期
    }
}

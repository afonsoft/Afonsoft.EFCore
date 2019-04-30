using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConsoleAppTest.Model
{
    public class UserModel
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Nome { get; set; }
    }
}

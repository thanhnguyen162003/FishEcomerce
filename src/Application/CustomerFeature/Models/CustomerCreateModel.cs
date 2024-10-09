using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomerFeature.Models
{
    public class CustomerCreateModel
    {
        public Guid Id { get; set; } // Thêm ID vào model

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateOnly? Birthday { get; set; }
    }
}

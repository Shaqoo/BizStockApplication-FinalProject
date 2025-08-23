using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum Gender
    {
        [Display(Name = "Male")]
        Male = 1,

        [Display(Name = "Female")]
        Female = 2,

        [Display(Name = "Other")]
        Other = 3,

        [Display(Name = "Prefer not to say")]
        PreferNotToSay = 4,

        [Display(Name = "Non Binary")]
        NonBinary = 5,
    }

}

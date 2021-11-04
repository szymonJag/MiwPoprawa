using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoprawaMiW.Models
{
    public class SymbolToChoiceModel
    {
        [Required]
        public int IndexOfColumn { get; set; }
        public int IndexOfRow { get; set; }

    }
}

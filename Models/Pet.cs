using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_12._3._2.Models
{
    public partial class Pet
    {
        
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Type { get; set; }

    }
}

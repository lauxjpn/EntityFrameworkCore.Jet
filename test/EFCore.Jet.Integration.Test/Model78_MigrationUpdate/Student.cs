﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Jet.Integration.Test.Model78_MigrationUpdate
{
    [Table("Students_78")]
    public class Student
    {
        public int StudentId { get; set; }
        
        [Required]
        [MaxLength(50)]
        // Index are supported only with fluent API
        /*[Index]*/
        public string StudentName { get; set; }
        
        public string Notes { get; set; }

        public override string ToString()
        {
            return string.Format("{2}: {0} - {1}", StudentId, StudentName, base.ToString());
        }
    }

}

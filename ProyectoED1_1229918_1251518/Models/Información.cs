using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoED1_1229918_1251518.Models
{
    public class Información:IComparable
    {
        public int num { get; set; }
        [StringLength(100)]
        public string varchar { get; set; }
        public DateTime tiempo { get; set; }

        public int num2 { get; set; }
        [StringLength(100)]
        public string varchar2 { get; set; }
        public DateTime tiempo2 { get; set; }

        public int num3 { get; set; }
        [StringLength(100)]
        public string varchar3 { get; set; }
        public DateTime tiempo3 { get; set; }

        public int CompareTo(object obj)
        {
            Información compareToObj = (Información)obj;
            return this.num.CompareTo(compareToObj.num);
        }
    }
}
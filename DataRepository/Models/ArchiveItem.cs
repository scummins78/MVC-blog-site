using System;
using System.Linq;

namespace DataRepository.Models
{
    public class ArchiveItem
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthName { get; set; }
        public int Instances { get; set; }
    }
}

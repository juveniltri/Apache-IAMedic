using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApacheIAMedicClient.Models
{
    public class ConfigurationModel
    {
        public string Lang { get; set; }
        public int NumOfEMS { get; set; }
        public string EMSJobName { get; set; }
        public string BlipName { get; set; }
        public int BlipColor { get; set; }
    }
}

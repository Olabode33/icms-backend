using System;
using System.Collections.Generic;
using System.Text;

namespace ICMSDemo.Processes.Dtos
{
    public class ExportToExcelDto
    {
        public string processName { get; set; }
        
        public List<Risk> risk { get; set; }
        public List<Control> control { get; set; }

    }

    public class Risk 
    {
        public string RiskName { get; set; }
        public int Impact { get; set; }
        public int Likelyhood { get; set; }

    }

    public class Control
    {
        public string ControlName { get; set; }
        public int Impact { get; set; }
        public int Likelyhood { get; set; }

    }
}

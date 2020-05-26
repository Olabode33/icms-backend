using ICMSDemo.ProcessRisks;
using System.ComponentModel.DataAnnotations.Schema;
using ICMSDemo.Processes;
using ICMSDemo.DepartmentRiskControls;

namespace ICMSDemo.ProcessRiskControls
{
    //[Table("ProcessRiskControls")]
    public class ProcessRiskControl : DepartmentRiskControl
	{
		public virtual int? ProcessRiskId { get; set; }
		
        [ForeignKey("ProcessRiskId")]
		public ProcessRisk ProcessRiskFk { get; set; }
		
		public virtual long? ProcessId { get; set; }
		
        [ForeignKey("ProcessId")]
		public Process ProcessFk { get; set; }

    }
}
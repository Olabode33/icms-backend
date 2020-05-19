using ICMSDemo;
using ICMSDemo.ProcessRisks;
using Abp.Organizations;
using ICMSDemo.Controls;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
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
using Abp.Organizations;
using ICMSDemo.Risks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ICMSDemo.Processes;
using ICMSDemo.DepartmentRisks;

namespace ICMSDemo.ProcessRisks
{
	//[Table("ProcessRisks")]
    public class ProcessRisk : DepartmentRisk
	{

		public virtual long ProcessId { get; set; }
		
        [ForeignKey("ProcessId")]
		public Process ProcessFk { get; set; }


        public int? Likelyhood { get; set; }

        public int? Impact { get; set; }
    }
}
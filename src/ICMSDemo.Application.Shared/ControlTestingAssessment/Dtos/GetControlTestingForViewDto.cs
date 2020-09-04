using System;

namespace ICMSDemo.ControlTestingAssessment.Dtos
{
    public class GetControlTestingForViewDto
    {
		public ControlTestingDto ControlTesting { get; set; }

        public string Name { get; set; }

        public int? TestingTemplateId { get; set; }

        public int? Id { get; set; }


        public DateTime? EndDate { get; set; }

    }
}
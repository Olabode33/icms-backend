﻿namespace ICMSDemo.KeyRiskIndicators.Dtos
{
    public class GetKeyRiskIndicatorForViewDto
    {
		public KeyRiskIndicatorDto KeyRiskIndicator { get; set; }

		public string ExceptionTypeCode { get; set;}

		public string UserName { get; set;}

        public string RiskName { get; set; }
        public string BusinessObjectiveName { get; set; }
    }
}
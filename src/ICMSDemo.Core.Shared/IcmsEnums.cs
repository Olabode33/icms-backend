using System;
using System.Collections.Generic;
using System.Text;

namespace ICMSDemo
{
   public class IcmsEnums
    {
        public enum ProjectOwner
        {
            InternalAudit,
            InternalControl,
            OperationRisk,
            General
            
        }

        public enum LossEventTypeEnums
        {
            InternalFraud,
            ExternalFraud,
            EmploymentPracticesWorkplaceSafety,
            ClientsProductsBusinessPractice,
            DamagePhysicalAssets,
            BusinessDisruptionSystemsFailures,
            ExecutionDeliveryProcessManagement
        }

        public enum LossCategoryEnums
        {
            Actual,
            Potential,
            NearMisses
        }
    }
}

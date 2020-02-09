
using ICMSDemo;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.Risks.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ICMSDemo.Risks
{
	[AbpAuthorize(AppPermissions.Pages_Risks)]
    public class RisksAppService : ICMSDemoAppServiceBase, IRisksAppService
    {
		 private readonly IRepository<Risk> _riskRepository;
		 

		  public RisksAppService(IRepository<Risk> riskRepository ) 
		  {
			_riskRepository = riskRepository;
			
		  }

		 public async Task<PagedResultDto<GetRiskForViewDto>> GetAll(GetAllRisksInput input)
         {
			var severityFilter = (Severity) input.SeverityFilter;
			
			var filteredRisks = _riskRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(input.SeverityFilter > -1, e => e.Severity == severityFilter);

			var pagedAndFilteredRisks = filteredRisks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var risks = from o in pagedAndFilteredRisks
                         select new GetRiskForViewDto() {
							Risk = new RiskDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Description = o.Description,
                                Severity = o.Severity,
                                Id = o.Id
							}
						};

            var totalCount = await filteredRisks.CountAsync();

            return new PagedResultDto<GetRiskForViewDto>(
                totalCount,
                await risks.ToListAsync()
            );
         }
		 
		 public async Task<GetRiskForViewDto> GetRiskForView(int id)
         {
            var risk = await _riskRepository.GetAsync(id);

            var output = new GetRiskForViewDto { Risk = ObjectMapper.Map<RiskDto>(risk) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Risks_Edit)]
		 public async Task<GetRiskForEditOutput> GetRiskForEdit(EntityDto input)
         {
            var risk = await _riskRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRiskForEditOutput {Risk = ObjectMapper.Map<CreateOrEditRiskDto>(risk)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRiskDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Risks_Create)]
		 protected virtual async Task Create(CreateOrEditRiskDto input)
         {
            var risk = ObjectMapper.Map<Risk>(input);

			var prevCount = await _riskRepository.CountAsync();

			prevCount++;

			if (AbpSession.TenantId != null)
			{
				risk.TenantId = (int) AbpSession.TenantId;
			}

			risk.Code = "R-" + prevCount.ToString();

            await _riskRepository.InsertAsync(risk);
         }

		 [AbpAuthorize(AppPermissions.Pages_Risks_Edit)]
		 protected virtual async Task Update(CreateOrEditRiskDto input)
         {
            var risk = await _riskRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, risk);
         }

		 [AbpAuthorize(AppPermissions.Pages_Risks_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _riskRepository.DeleteAsync(input.Id);
         } 
    }
}
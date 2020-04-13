using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.Banks.Dtos;

namespace TepayLink.Sdisco.Banks
{
    public interface IBankAppService : IApplicationService
    {

        Task<List<Dtos.BankDto>> GetBankList();
        Task<List<Dtos.BankDto>> GetBankListByType(BankTypeEnum Type);
        Task<List<Dtos.BankBranchDto>> GetBankBranch(int bakId);
        Task CreateBankAccountInfo(CreateBankAccountInputDto bankAccountDto);
        Task UpdateBankAccountInfo(CreateBankAccountInputDto bankAccountDto);
        Task<List<BankAccountInforDto>> GetBankAccountInfo();

        Task<List<BankAccountInforDto>> GetBankAccountOfHost();

        Task CreateOrUpdatePayPalAccount(CreateOrUpdatePayPalAccount input);

    }
}

using Abp.Authorization;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.Banks.Dtos;
using System.Linq;
using Abp.UI;
using TepayLink.Sdisco.AdminConfig.Dtos;

namespace TepayLink.Sdisco.Banks
{

    [AbpAuthorize]
    public class BankAppService : SdiscoAppServiceBase, IBankAppService
    {

        private readonly IRepository<Bank> _bankRepository;
        
        private readonly IRepository<BankAccountInfo, long> _bankAccountInfoRepository;
        private readonly IRepository<BankBranch> _bankBranchRepository;


        public BankAppService(IRepository<Bank> bankRepository, IRepository<BankAccountInfo, long> bankAccountInfoRepository, IRepository<BankBranch> bankBranchRepository)
        {
            _bankRepository = bankRepository;
            _bankAccountInfoRepository = bankAccountInfoRepository;
            _bankBranchRepository = bankBranchRepository;
        }

        public async Task CreateBankAccountInfo(CreateBankAccountInputDto bankAccountDto)
        {
            var bankinfor = ObjectMapper.Map<BankAccountInfo>(bankAccountDto);
            bankinfor.UserId = AbpSession.UserId ?? 0;
            _bankAccountInfoRepository.Insert(bankinfor);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInput"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<BankAccountInforDto>> GetBankAccountInfo()
        {
            var query = from p in _bankAccountInfoRepository.GetAll()
                        join q in _bankRepository.GetAll() on p.BankId equals q.Id
                        where p.UserId == AbpSession.UserId
                        select new BankAccountInforDto
                        {
                            Id = p.Id,
                            Type = q.Type,
                            BankId = p.BankId,
                            Image = q.CardImage,
                            Logo = q.Logo,
                            AccountName = p.AccountName,
                            BankName = q.BankName,
                            AccountNo = p.AccountNo,




                        };
            return query.ToList();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInput"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<BankAccountInforDto>> GetBankAccountOfHost()
        {
            var query = from p in _bankAccountInfoRepository.GetAll()
                        join q in _bankRepository.GetAll() on p.BankId equals q.Id
                        where p.UserId == AbpSession.UserId
                        select new BankAccountInforDto
                        {
                            Id = p.Id,
                            Type = q.Type,
                            BankId = p.BankId,
                            Image = q.CardImage,
                            Logo = q.Logo,
                            AccountName = p.AccountName,
                            BankName = q.BankName,
                            AccountNo = p.AccountNo,




                        };
            return query.ToList();
        }

        public async Task CreateOrUpdatePayPalAccount(CreateOrUpdatePayPalAccount input)
        {
            var paypalbank = _bankRepository.FirstOrDefault(p => p.Type == BankTypeEnum.Paypal);
            var item = _bankAccountInfoRepository.FirstOrDefault(p => p.BankId == paypalbank.Id);
            if (item != null)
            {
                item.AccountNo = input.UserNameOrEmail;
                _bankAccountInfoRepository.Update(item);
            }
            else
            {
                item = new BankAccountInfo
                {
                    BankId = paypalbank.Id,
                    AccountNo = input.UserNameOrEmail,
                    UserId = AbpSession.UserId ?? 0,

                };
                _bankAccountInfoRepository.Insert(item);
            }
        }


        /// <summary>
        /// Lấy tất cả các chi nhánh của ngân hàng
        /// </summary>
        /// <param name="bakId"> Id ngân hàng </param>
        /// <returns></returns>
        public async Task<List<BankBranchDto>> GetBankBranch(int bakId)
        {
            var listBranch = _bankBranchRepository.GetAll().Where(p => p.BankId == bakId).OrderBy(p => p.Order).ToList();
            return ObjectMapper.Map<List<BankBranchDto>>(listBranch);

        }
        /// <summary>
        /// Danh sách ngân hàng
        /// </summary>
        /// <returns></returns>
        public async Task<List<BankDto>> GetBankList()
        {
            var listBank = _bankRepository.GetAll().Where(p => p.Type == BankTypeEnum.AtmCard).ToList();
            return ObjectMapper.Map<List<BankDto>>(listBank);
        }
        /// <summary>
        /// Danh sách ngân hàng theo loại ngân hàng ( nội địa, thẻ quốc tế, Paypal)
        /// </summary>
        /// <returns></returns>
        public async Task<List<BankDto>> GetBankListByType(BankTypeEnum type)
        {

            var listBank = _bankRepository.GetAll().Where(p => p.Type == type).ToList();
            return ObjectMapper.Map<List<BankDto>>(listBank);
        }

        public async Task UpdateBankAccountInfo(CreateBankAccountInputDto bankAccountDto)
        {
            var existBankInfor = _bankAccountInfoRepository.Get(bankAccountDto.Id);
            if (existBankInfor != null)
            {
                ObjectMapper.Map<BankAccountInfo>(bankAccountDto);
            }
            else
            {
                throw new UserFriendlyException(L("InforNotExist"));
            }

        }
    }
}

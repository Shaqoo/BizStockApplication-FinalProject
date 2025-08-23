using Application.Interfaces.Repository.BaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IDeliveryFeeRuleRepository : IBaseRepository<DeliveryFeeRule>
    {
        Task<DeliveryFeeRule?> GetByZoneAsync(string zone);
        Task<DeliveryFeeRule?> GetActiveRuleByZoneAsync(string zone);
        Task<decimal> CalculateDeliveryFeeAsync(string zone, decimal orderAmount);
        Task<bool> IsFreeDeliveryAsync(string zone, decimal orderAmount);
        Task UpdateDeliveryFeeRule(DeliveryFeeRule deliveryFeeRule);
        Task<bool> DeleteRule(Guid ruleId);
    }

}

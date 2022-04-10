using KadoshDomain.Entities;
using KadoshDomain.Enums;
using System.Linq.Expressions;

namespace KadoshDomain.Queriables
{
    public class InstallmentQueriable
    {
        public static Expression<Func<Installment, bool>> GetBySaleId(int saleId)
        {
            return x => x.SaleId == saleId;
        }

        public static Func<Installment, bool> GetBySituationAndSaleIdExceptFromOne(EInstallmentSituation situation, int saleId, int installmentIdToExcludeFromQuery)
        {
            return x => x.SaleId == saleId && x.Situation == situation && x.Id != installmentIdToExcludeFromQuery;
        }
    }
}

﻿using KadoshDomain.Entities;
using System.Linq.Expressions;

namespace KadoshDomain.Queriables
{
    public static class CustomerQueriable
    {
        public static Expression<Func<Customer, bool>> GetCustomerByName(string name)
        {
            return x => x.Name.Contains(name);
        }

        public static Expression<Func<Customer, bool>> GetCustomerByUsername(string username)
        {
            return x => x.Username == username;
        }
    }
}

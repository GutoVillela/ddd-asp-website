﻿namespace KadoshShared.Constants.ErrorCodes
{
    public class ErrorCodes
    {
        public const int UNEXPECTED_EXCEPTION = 0;
        public const int ERROR_INVALID_STORE_CREATE_COMMAND = 1;
        public const int ERROR_INVALID_STORE_UPDATE_COMMAND = 2;
        public const int ERROR_INVALID_STORE_DELETE_COMMAND = 3;
        public const int ERROR_REPEATED_STORE_ADDRESS = 4;
        public const int ERROR_INVALID_CUSTOMER_CREATE_COMMAND = 5;
        public const int ERROR_INVALID_CUSTOMER_UPDATE_COMMAND = 6;
        public const int ERROR_INVALID_CUSTOMER_DELETE_COMMAND = 7;
        public const int ERROR_CUSTOMER_NOT_FOUND = 8;
        public const int ERROR_STORE_NOT_FOUND = 9;
        public const int ERROR_INVALID_USER_CREATE_COMMAND = 10;
        public const int ERROR_INVALID_USER_UPDATE_COMMAND = 11;
        public const int ERROR_INVALID_USER_DELETE_COMMAND = 12;
        public const int ERROR_USERNAME_ALREADY_TAKEN = 13;
        public const int ERROR_USERNAME_NOT_FOUND = 14;
        public const int ERROR_INVALID_USER_AUTHENTICATE_COMMAND = 15;
        public const int ERROR_AUTHENTICATION_FAILED = 16;
        public const int ERROR_INVALID_BRAND_CREATE_COMMAND = 17;
        public const int ERROR_INVALID_BRAND_UPDATE_COMMAND = 18;
        public const int ERROR_INVALID_BRAND_DELETE_COMMAND = 19;
        public const int ERROR_BRAND_NOT_FOUND = 20;
        public const int ERROR_INVALID_CATEGORY_CREATE_COMMAND = 21;
        public const int ERROR_INVALID_CATEGORY_UPDATE_COMMAND = 22;
        public const int ERROR_INVALID_CATEGORY_DELETE_COMMAND = 23;
        public const int ERROR_CATEGORY_NOT_FOUND = 24;
        public const int ERROR_INVALID_PRODUCT_CREATE_COMMAND = 25;
        public const int ERROR_INVALID_PRODUCT_UPDATE_COMMAND = 26;
        public const int ERROR_INVALID_PRODUCT_DELETE_COMMAND = 27;
        public const int ERROR_PRODUCT_NOT_FOUND = 28;
        public const int ERROR_INVALID_SALE_IN_CASH_CREATE_COMMAND = 29;
        public const int ERROR_COULD_NOT_FIND_SALE_CUSTOMER = 30;
        public const int ERROR_COULD_NOT_FIND_SALE_SELLER = 31;
        public const int ERROR_COULD_NOT_FIND_SALE_PRODUCT = 32;
        public const int ERROR_INVALID_SALE_ITEM = 33;
        public const int ERROR_COULD_NOT_CREATE_SALE_IN_CASH_POSTING = 34;
        public const int ERROR_INVALID_SALE_IN_INSTALLMENTS_CREATE_COMMAND = 35;
        public const int ERROR_COULD_NOT_CREATE_DOWN_PAYMENT_POSTING = 36;
        public const int ERROR_INVALID_SALE_INSTALLMENT = 35;
        public const int ERROR_INVALID_SALE_ON_CREDIT_CREATE_COMMAND = 36;
        public const int ERROR_COULD_NOT_FIND_SALE_STORE = 37;
        public const int ERROR_INVALID_PAYOFF_SALE_COMMAND = 38;
        public const int ERROR_SALE_NOT_FOUND = 39;
        public const int ERROR_COULD_NOT_CREATE_SALE_PAYOFF_POSTING = 40;
        public const int ERROR_INVALID_CUSTOMER_INFORM_PAYMENT_COMMAND = 41;
        public const int ERROR_NO_OPEN_SALES_FOUND_TO_CUSTOMER = 42;
        public const int ERROR_ONE_OR_MORE_INVALID_POSTINGS_TO_INFORMING_PAYMENT = 43;
        public const int ERROR_INVALID_GET_BRAND_BY_ID_QUERY = 44;
        public const int ERROR_INVALID_GET_CATEGORY_BY_ID_QUERY = 45;
        public const int ERROR_INVALID_GET_CUSTOMER_BY_ID_QUERY = 46;
        public const int ERROR_INVALID_GET_CUSTOMER_TOTAL_DEBT_QUERY = 47;
        public const int ERROR_INVALID_GET_ALL_POSTINGS_FROM_CUSTOMER_QUERY = 48;
        public const int ERROR_INVALID_GET_PRODUCT_BY_ID_QUERY = 49;
        public const int ERROR_INVALID_GET_ALL_SALES_BY_CUSTOMER_ID_QUERY = 50;
        public const int ERROR_INVALID_GET_STORE_BY_ID_QUERY_RESULT = 51;
        public const int ERROR_INVALID_GET_USER_BY_ID_QUERY = 52;
    }
}
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
    }
}
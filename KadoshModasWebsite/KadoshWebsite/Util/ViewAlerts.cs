using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace KadoshWebsite.Util
{
    public static class ViewAlerts
    {
        public static void SuccessAlert(ITempDataDictionary tempData, string messageToAlert)
        {
            tempData[ViewConstants.VIEW_TEMP_MESSAGE] = messageToAlert;
        }

        public static void ErrorAlert(ITempDataDictionary tempData, string messageToAlert)
        {
            tempData[ViewConstants.VIEW_ERROR_MESSAGE] = messageToAlert;
        }
    }
}

namespace KeepFitStore.Services.CustomExceptions.Messsages
{
    internal static class ExceptionMessages
    {
        internal const string UserLookupFailed = "User lookup for Id = {0} failed";

        internal const string InvalidProductType = "{0} is not valid {1} type";

        internal const string InvalidAminoId = "Amino with id = {0} can not be found";

        internal const string InvalidJobPosition = "{0} job position does not exist";

        internal const string InvalidCreatine = "Creatine with id = {0} can not be found";

        internal const string BasketItemNotFound = "Basket item can not be found for basket id = {0} and product id {1}";

        internal const string NegativeQuantity = "Quantity can not be negative";

        internal const string ProductNotFound = "Product with id = {0} can not be found";

        internal const string UserDoesNotExist = "User with username = {0} does not exist";

        internal const string OrderNotFound = "Order with id = {0} can not be found";

        internal const string InvalidBasketContent = "User with id = {0} has no products in basket";

        internal const string NotAuthorized = "User with username = {0} is not authorized to see this content";
    }
}
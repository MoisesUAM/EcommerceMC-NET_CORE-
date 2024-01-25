namespace Ecommerce.BLL.Notifications
{
    public static class DS
    {
        public const string Success = "Success";
        public const string Error = "Error";
        public const string ImagesRootPaht = @"\images\products\";

        public const string AdminRole = "Administrator";
        public const string ClientRole = "Client";
        public const string EmployeeRole = "Employee";
        public const string SesionShoppingCart = "Sesion Compras";

        //Estados de la orden
        public const string PendingOrder = "Orden pendiente";
        public const string ApprovedOrder = "Orden aprobada";
        public const string OrderProcessing = "Orden en proceso";
        public const string OrderShipped = "Orden enviada";
        public const string OrderCanceled = "Orden cancelada";
        public const string OrderReturned = "Orden devuelta";

        //estados de pago
        public const string PendingPayment = "Pago pendiente";
        public const string ApprovedPayment = "Pago aprobado";
        public const string DelayedPayment = "Pago retrasado";
        public const string PaymentDeclined = "Pago rechazado";


    }
}

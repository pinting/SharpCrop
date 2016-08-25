namespace SharpCrop.Provider.Models
{
    public enum ProviderState
    {
         NewToken,
         RefreshToken,
         UserError,
         ServiceError,
         PermissionError
    }
}

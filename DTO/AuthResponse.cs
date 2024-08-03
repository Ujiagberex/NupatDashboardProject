namespace NupatDashboardProject.DTO
{
	public class AuthResponse
	{
		public string AccessToken { get; init; } = default;
		public string RefreshToken { get; init; } = default;
		public string Username { get; init; } = default;
		public string PhoneNumber { get; init; } = default;
		public string Email { get; init; } = default;
		public string UserId { get; init; } = default;
		public string FirstName { get; init; } = default;
		public string LastName { get; init; } = default;
		public string MiddleName { get; init; } = default;
		public string FullName { get; init; } = default;
		public DateTime ExpiryTime { get; init; } = default;
		public string[] Roles { get; set; } = default;
	}
}

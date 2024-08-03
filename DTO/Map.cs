namespace NupatDashboardProject.DTO
{
	public class Map
	{
		public static Result<T> GetModelResult<T>(List<T> entity, bool status, string message)
		{
			var result = new Result<T>()
			{
				Data = entity,
				Succeeded = status,
				Message = message,
			};

			return result;
		}
	}
}

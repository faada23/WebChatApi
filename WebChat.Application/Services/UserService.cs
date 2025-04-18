using System.Linq.Expressions;

public class UserService : IUserService {
    public IRepository<User> UserRepository {get;set;}

    public UserService(IRepository<User> userRepository){
        UserRepository = userRepository;
    }


    public async Task<PagedResponse<GetUserResponse>> GetUsers(string? filter, PaginationParameters pagParams){

        Expression<Func<User, bool>>? filterExpression = null;
    
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var searchTerm = filter.Trim().ToLower();
            filterExpression = x => x.Username.ToLower().StartsWith(searchTerm);
        }

        var users = await UserRepository.GetAll(
            filter: filterExpression,
            pagParams: pagParams,
            orderBy: x => x.OrderByDescending(x => x.Username)
        );

        var usersDto = users.Select(user => new GetUserResponse
            (   
               user.Id,
               user.Username
        )).ToList();

        return new PagedResponse<GetUserResponse>(
            usersDto,
            users.CurrentPage,
            users.PageSize,
            users.TotalItems,
            users.TotalPages
        );
    }
}
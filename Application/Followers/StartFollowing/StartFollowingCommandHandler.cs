using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Followers;
using Domain.Users;
using SharedKernel;

namespace Application.Followers.StartFollowing
{
    internal sealed class StartFollowingCommandHandler : ICommandHandler<StartFollowingCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IFollowerService followerService;
        private readonly IUnitOfWork unitOfWork;

        public StartFollowingCommandHandler(
            IUserRepository userRepository,
            IFollowerService followerService,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.followerService = followerService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(StartFollowingCommand command, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);
            if (user == null)
            {
                return Result.Failure(UserErrors.NotFound(command.UserId));
            }

            User? followed = await userRepository.GetByIdAsync(command.FollowedId, cancellationToken);
            if (followed == null)
            {
                return Result.Failure(UserErrors.NotFound(command.FollowedId));
            }

            Result result = await followerService.StartFollowingAsync(user, followed, cancellationToken);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

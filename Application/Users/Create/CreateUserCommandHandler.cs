using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

namespace Application.Users.Create
{
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            Result<Email> emailResult = Email.Create(command.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<Guid>(emailResult.Error);
            }

            Email email = emailResult.Value;

            if (!await userRepository.IsEmailUniqueAsync(email, cancellationToken))
            {
                return Result.Failure<Guid>(UserErrors.EmailNotUnique(email.Value));
            }

            Name name = new(command.Name);

            User user = User.Create(name, emailResult.Value, command.HasPublicProfile);

            userRepository.Insert(user);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id);
        }
    }
}

using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace ETicaretAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
           IdentityResult result =  await _userManager.CreateAsync(new()
            {
               Id=Guid.NewGuid().ToString(),
                UserName=request.Username,
                Email=request.Email,
                NameSurname=request.NameSurname

            },request.Password);

            if(result.Succeeded)
            {
                return new()
                {
                    Succeded = true,
                    Message = "Kullanıcı başarıyla oluşturulmuştur"
                };

            }
   
            throw new UserCreateFailedException();
            

            
        }
    }
}

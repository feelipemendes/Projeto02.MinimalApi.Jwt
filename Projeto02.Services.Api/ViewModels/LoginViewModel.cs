using Flunt.Notifications;
using Flunt.Validations;
using Projeto02.Services.Api.Entities;
using Projeto02.Services.Api.Helpers;

namespace Projeto02.Services.Api.ViewModels
{
    public class LoginViewModel : Notifiable<Notification>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

        public Usuario MapTo()
        {
            AddNotifications(new Contract<Notification>()
                    .Requires()
                    .IsNullOrEmpty(Email, "Informe um endereço de e-mail válido.")
                    .IsNotNullOrEmpty(Password, "Informe a senha do usuário.")
                    .IsGreaterOrEqualsThan(Password, 6, "Senha do usuário deve ter no mínimo 6 caracteres."));

            return new Usuario()
            {
                Id = Guid.NewGuid(),
                Email = Email,
                Password = MD5Helper.Get(Password),
                CreatedAt = DateTime.Now
            };
        }
    }
}

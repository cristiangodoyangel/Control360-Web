using System.Web.Security;

public static class UsuarioInitializer
{
    public static void Initialize()
    {
        string[] roles = { "Administrador", "Gerencia", "Proyectos", "Supervisor", "Usuario" };

        foreach (string rol in roles)
        {
            if (!Roles.RoleExists(rol))
            {
                Roles.CreateRole(rol);
            }
        }

        string adminEmail = "admin@inventario360.com";

        if (Membership.GetUser(adminEmail) == null)
        {
            Membership.CreateUser(adminEmail, "Admin123!");
            Roles.AddUserToRole(adminEmail, "Administrador");
        }
    }
}

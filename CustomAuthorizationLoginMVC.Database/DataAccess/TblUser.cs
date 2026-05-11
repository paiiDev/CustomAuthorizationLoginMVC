using System;
using System.Collections.Generic;

namespace CustomAuthorizationLoginMVC.Database.DataAccess;

public partial class TblUser
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Password { get; set; } = null!;
}

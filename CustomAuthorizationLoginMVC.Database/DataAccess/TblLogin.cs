using System;
using System.Collections.Generic;

namespace CustomAuthorizationLoginMVC.Database.DataAccess;

public partial class TblLogin
{
    public int LoginId { get; set; }

    public int UserId { get; set; }

    public string SessionId { get; set; } = null!;

    public DateTime SessionExpired { get; set; }
}

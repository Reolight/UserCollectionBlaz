﻿#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UserCollectionBlaz.Areas.Identity.Data;

public class AppUser : IdentityUser
{
    public string AvatarSrc { get; set; }
    public int PostedTimes { get; set; }
    public int Level { get; set; } = 1;
}


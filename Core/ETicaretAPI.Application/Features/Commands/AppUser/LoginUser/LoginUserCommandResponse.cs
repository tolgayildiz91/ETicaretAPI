﻿using ETicaretAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandResponse
    {
        public Token Token { get; set; }

    }

    public class LoginUserSuccessCommandResponse: LoginUserCommandResponse
    {

    }

    public class LoginUserErrorCommandRespose : LoginUserCommandResponse
    {
        public string Message { get; set; }
    }

}
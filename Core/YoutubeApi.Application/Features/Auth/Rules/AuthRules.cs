﻿using YoutubeApi.Application.Bases;
using YoutubeApi.Application.Features.Auth.Exceptions;
using YoutubeApi.Domain.Entities;

namespace YoutubeApi.Application.Features.Auth.Rules
{
    public class AuthRules : BaseRules
    {
        public Task UserShouldNotBeExist(User? user)
        {
            if (user is not null) throw new UserAlreadyExistException();
            return Task.CompletedTask;

        }
    }
}

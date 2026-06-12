using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Results
{
    public static class AuthErrors
    {
        public static readonly Error InvalidCredentials =
            new Error("Auth.InvalidCredentials", "Email or password is incorrect.", ErrorType.Validation);

        public static readonly Error InvalidRefreshToken =
            new Error("Auth.InvalidRefreshToken", "Refresh token is invalid or expired.", ErrorType.Validation);

        public static readonly Error Unauthorized =
            new Error("Auth.Unauthorized", "User is not authorized.", ErrorType.Validation);

        public static readonly Error FirstNameRequired =
            new Error("Auth.FirstNameRequired", "First name is required.", ErrorType.Validation);

        public static readonly Error LastNameRequired =
            new Error("Auth.LastNameRequired", "Last name is required.", ErrorType.Validation);

        public static readonly Error EmailRequired =
            new Error("Auth.EmailRequired", "Email is required.", ErrorType.Validation);

        public static readonly Error PasswordRequired =
            new Error("Auth.PasswordRequired", "Password is required.", ErrorType.Validation);

        public static readonly Error PasswordTooShort =
            new Error("Auth.PasswordTooShort", "Password must be at least 6 characters long.", ErrorType.Validation);

        public static readonly Error EmailAlreadyExists =
            new Error("Auth.EmailAlreadyExists", "Email is already registered.", ErrorType.Conflict);
    }
}

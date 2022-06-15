﻿namespace AppTracker.Models
{
    public class BuildSettingsOptions
    {
        public string Environment { get; set; } = String.Empty;
        public string SqlConnectionString { get; set; } = String.Empty;
        public string SendGridAPIKey { get; set; } = String.Empty;
        public string JWTTokenKey { get; set; } = String.Empty;
        public string JWTHeaderName { get; set; } = String.Empty;
        public string AccessControlHeaderName { get; set; } = String.Empty;
        public string RoleIdentityIdentifier1 { get; set; } = String.Empty;
        public string RoleIdentityIdentifier2 { get; set; } = String.Empty;
        public string User { get; set; } = String.Empty;
        public string Admin { get; set; } = String.Empty;
        public string JwtIssuer { get; set; } = String.Empty;
        public string JwtAudience { get; set; } = String.Empty;
        public string JwtHashAlgorithm { get; set; } = String.Empty;
        public string GuestName { get; set; } = String.Empty;
        public string GuestRole { get; set; } = String.Empty;
        public string GuestHash { get; set; } = String.Empty;
    }
}
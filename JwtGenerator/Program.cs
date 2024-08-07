using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

class Program
{
    static void Main()
    {
        try
        {
            // Variables
            string issuer = "YourIssuer";
            string audience = "YourAudience";
            string certificatePath = @".\TestCert.pfx";
            string certificatePassword = "Test"; // Use environment variables or secure storage for this

            // Load the certificate
            var certificate = LoadCertificate(certificatePath, certificatePassword);

            // Create Security Key from Certificate
            var securityKey = new X509SecurityKey(certificate);

            // Create Signing Credentials
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            // Create Claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "client-id"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Create JWT
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: signingCredentials);

            // Generate Token
            var tokenHandler = new JwtSecurityTokenHandler();
            string jwt = tokenHandler.WriteToken(token);

            // Output the JWT
            Console.WriteLine("Generated JWT: ");
            Console.WriteLine(jwt);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static X509Certificate2 LoadCertificate(string path, string password)
    {
        try
        {
            return new X509Certificate2(path, password, X509KeyStorageFlags.MachineKeySet);
        }
        catch (System.IO.FileNotFoundException)
        {
            throw new Exception("Certificate file not found. Please check the path and try again.");
        }
        catch (CryptographicException)
        {
            throw new Exception("The certificate could not be loaded. Please check the password and ensure the certificate is valid.");
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred while loading the certificate: {ex.Message}");
        }
    }
}

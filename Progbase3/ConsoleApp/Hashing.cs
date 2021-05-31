using System;
using System.Security.Cryptography;
using System.Text;

public static class Hashing
{

    public static User SignUp(string username, string fullname, string password, UserRepository userRepository)
    {
        User user = new User();

        password = HashCode(password);
        user = new User(fullname, username, password);

        return user;
    }

    public static User SignIn(string username, string password, UserRepository userRepository)
    {
        User user = new User();
        if (userRepository.UserExistsByUsername(username))
        {
            SHA256 sha256Hash = SHA256.Create();
            string hash = GetHash(sha256Hash, password);
            user.username = "u";
            User dbUser = userRepository.GetUserByUsername(username);
            if (VerifyHash(sha256Hash, dbUser.password, hash))
            {
                user = userRepository.GetUserByUsername(username);
            }
        }
        return user;
    }
    private static string HashCode(string code)
    {
        SHA256 sha256Hash = SHA256.Create();
        string hash = GetHash(sha256Hash, code);
        sha256Hash.Dispose();
        return hash;
    }
    private static string GetHash(HashAlgorithm hashAlgorithm, string input)
    {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        var sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    // Verify a hash against a string.
    private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
    {

        // Create a StringComparer an compare the hashes.
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        return comparer.Compare(input, hash) == 0;
    }
}


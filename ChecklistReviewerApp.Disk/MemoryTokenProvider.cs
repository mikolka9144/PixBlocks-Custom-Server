using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Pix_API.ChecklistReviewerApp.Interfaces;

namespace Pix_API.ChecklistReviewerApp.Disk
{
    public class MemoryTokenProvider : ITokenProvider
    {
        private List<Token> tokens;
        private SHA256 hasher;

        public MemoryTokenProvider()
        {
            tokens = new List<Token>();
            hasher = SHA256.Create();
        }

        public string GetTokenForUser(int UserId)
        {
            if (tokens.Any(s => s.userId == UserId)) return tokens.First(s => s.userId == UserId).token;

            var buffer = new byte[256];
            new Random().NextBytes(buffer);
            var token_value = ComputeSha256Hash(buffer);
            tokens.Add(new Token(UserId, token_value));
            return token_value;

        }

        public int GetUserForToken(string token)
        {
            return tokens.First(s => s.token == token).userId;
        }

        private string ComputeSha256Hash(byte[] rawData)
        {
            byte[] bytes = hasher.ComputeHash(rawData);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();

        }
        class Token
        {
            public int userId;
            public string token;

            public Token(int UserId, string token)
            {
                userId = UserId;
                this.token = token;
            }
        }
    }
}

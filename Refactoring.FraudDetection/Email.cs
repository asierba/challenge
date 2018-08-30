using System;

namespace Payvision.CodeChallenge.Refactoring.FraudDetection
{
    public class Email
    {
        public Email(string value)
        {
            Value = value;
        }

        private string Value { get; }

        public bool IsSimilarTo(Email otherEmail)
        {
            return NormalizeEmail(Value) == NormalizeEmail(otherEmail.Value);
        }

        private static string NormalizeEmail(string email)
        {
            var fullEmail = email.ToLower();
            var emailParts = fullEmail.Split(new[] {'@'}, StringSplitOptions.RemoveEmptyEntries);
            var emailName = RemoveDots(RemoveAfterPlus(emailParts[0]));
            var emailDomain = emailParts[1];
            return $"{emailName}@{emailDomain}";
        }

        private static string RemoveAfterPlus(string emailName)
        {
            var atIndex = emailName.IndexOf("+", StringComparison.Ordinal);
            return atIndex < 0 ? emailName : emailName.Remove(atIndex);
        }

        private static string RemoveDots(string emailName)
        {
            return emailName.Replace(".", "");
        }
    }
}
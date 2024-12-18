using Npgsql;
using System.Data;
using AuthentificationExample.Server.Extensions;
using AuthentificationExample.Server.Abstractions;
using AuthentificationExample.Server.Models;

namespace AuthentificationExample.Server.Services
{
    public class UserRepository(NpgsqlDataSource npgsqlDataSource) : IUserRepository
    {
        public async Task<AuthClientRecord?> FindUserAsync(string login)
        {
            const string query =
                """
                SELECT auths.user_id, password, salt, role_type, is_deleted
                FROM auths, users
                WHERE login = :login
                AND users.user_id = auths.user_id
                """;

            await using var connection = await npgsqlDataSource.OpenConnectionAsync();
            await using var sqlCommand = new NpgsqlCommand(query, connection);
            sqlCommand.Parameters.Add(new("login", login));

            using var reader = await sqlCommand.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                return null;
            }

            return new AuthClientRecord(
                UserId: reader.GetFieldValue<int>("user_id"),
                HashedPassword: reader.GetFieldValue<string>("password"),
                Salt: reader.GetFieldValue<string>("salt"),
                RoleType: (RoleEnum)reader.GetFieldValue<int>("role_type"),
                IsDeleted: reader.GetFieldValue<bool>("is_deleted")
            );
        }
        public async IAsyncEnumerable<StudentDTO> GetUsersByIdAsync(int userId)
        {
            const string query =
                """
                SELECT
                    students.user_id,
                    students.first_name,
                    students.last_name,
                    students.birthday,
                    students.alias_first_name,
                    students.alias_last_name,
                	student_contacts.vk AS student_vk,
                	student_contacts.whats_app AS student_whats_app,
                	student_contacts.tg AS student_tg,
                	student_contacts.phone_number AS student_phone_number,
                    parents.first_name AS parent_first_name,
                	parents.middle_name AS parent_middle_name,
                    parents.last_name AS parent_last_name,
                    parent_contacts.vk AS parent_vk,
                	parent_contacts.whats_app AS parent_whats_app,
                	parent_contacts.tg AS parent_tg,
                	parent_contacts.phone_number AS parent_phone_number
                FROM teachers_students ts
                JOIN students ON ts.student_user_id = students.user_id
                JOIN contacts AS student_contacts ON student_contacts.user_id = students.user_id
                JOIN parents ON parents.user_id = students.parent_user_id
                JOIN contacts AS parent_contacts ON parent_contacts.user_id = parents.user_id
                WHERE teacher_user_id = :teacher_id
                ORDER BY students.user_id;
                """;

            await using var connection = await npgsqlDataSource.OpenConnectionAsync();
            await using var sqlCommand = new NpgsqlCommand(query, connection);
            sqlCommand.Parameters.Add(new("teacher_id", userId));

            using var reader = await sqlCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var parentDTO = new ParentDTO(
                    FirstName: reader.GetFieldValue<string>("parent_first_name"),
                    MiddleName: reader.GetFieldValue<string>("parent_middle_name"),
                    LastName: reader.GetFieldValue<string>("parent_last_name"),
                    Vk: reader.GetNullableFieldValue<string>("parent_vk"),
                    WhatsApp: reader.GetNullableFieldValue<string>("parent_whats_app"),
                    Tg: reader.GetNullableFieldValue<string>("parent_tg"),
                    PhoneNumber: reader.GetFieldValue<string>("parent_phone_number")
                );
                yield return new StudentDTO(
                    UserId: reader.GetFieldValue<int>("user_id"),
                    FirstName: reader.GetFieldValue<string>("first_name"),
                    LastName: reader.GetFieldValue<string>("last_name"),
                    Birthday: reader.GetFieldValue<DateOnly>("birthday"),
                    AliasFirstName: reader.GetFieldValue<string>("alias_first_name"),
                    AliasLastName: reader.GetFieldValue<string>("alias_last_name"),
                    Vk: reader.GetNullableFieldValue<string>("student_vk"),
                    WhatsApp: reader.GetNullableFieldValue<string>("student_whats_app"),
                    Tg: reader.GetNullableFieldValue<string>("student_tg"),
                    PhoneNumber: reader.GetFieldValue<string>("student_phone_number"),
                    ParentDTO: parentDTO
                );
            }
        }

        public async Task UpdateUserNameAsync(int userId, string aliasFirstName, string aliasLastName)
        {
            const string query =
                """
                UPDATE students 
                SET alias_first_name = :alias_first_name, 
                alias_last_name = :alias_last_name 
                WHERE user_id = :user_id;
                """;
            await using var connection = await npgsqlDataSource.OpenConnectionAsync();
            await using var sqlCommand = new NpgsqlCommand(query, connection);

            sqlCommand.Parameters.Add(new("user_id", userId));
            sqlCommand.Parameters.Add(new("alias_first_name", aliasFirstName));
            sqlCommand.Parameters.Add(new("alias_last_name", aliasLastName));

            using var reader = await sqlCommand.ExecuteReaderAsync();
        }
    }
}

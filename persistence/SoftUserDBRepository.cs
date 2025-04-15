using System.Collections.Generic;
using System.Data;
using log4net;
using model;
using persistence;
using persistence.interfaces;

namespace Agentie_turism_transport_csharp.persistence
{
    public class SoftUserDBRepository : ISoftUserRepository
    {
        private readonly IDictionary<string, string> props;
        private static readonly ILog log = LogManager.GetLogger(typeof(SoftUserDBRepository));

        public SoftUserDBRepository(IDictionary<string, string> props)
        {
            log.DebugFormat("Initializing SoftUserDBRepository with properties: {0}", props);
            this.props = props;
        }

        public SoftUser FindOne(long id)
        {
            log.DebugFormat("Finding user with ID {0}", id);
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM soft_users WHERE id = @id";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var user = new SoftUser(
                            (string)reader["username"], 
                            (string)reader["password"])
                        {
                            Id = (long)reader["id"]
                        };
                        return user;
                    }
                }
            }
            return null;
        }

        public IEnumerable<SoftUser> FindAll()
        {
            log.Debug("Finding all users");
            var users = new List<SoftUser>();
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM soft_users";
                
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new SoftUser(
                            (string)reader["username"], 
                            (string)reader["password"])
                        {
                            Id = (long)reader["id"]
                        };
                        users.Add(user);
                    }
                }
            }
            return users;
        }

        public bool Save(SoftUser user)
        {
            log.DebugFormat("Saving user {0}", user);
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = @"INSERT INTO soft_users (username, password) 
                                    VALUES (@username, @password);
                                    SELECT last_insert_rowid();";

                var paramUsername = comm.CreateParameter();
                paramUsername.ParameterName = "@username";
                paramUsername.Value = user.username;
                comm.Parameters.Add(paramUsername);

                var paramPassword = comm.CreateParameter();
                paramPassword.ParameterName = "@password";
                paramPassword.Value = user.password;
                comm.Parameters.Add(paramPassword);

                var generatedId = comm.ExecuteScalar();
                if (generatedId != null)
                {
                    user.Id = (long)generatedId;
                    return true;
                }
            }
            return false;
        }

        public bool Delete(long id)
        {
            log.DebugFormat("Deleting user with id {0}", id);
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "DELETE FROM soft_users WHERE id = @id";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                int rowsDeleted = comm.ExecuteNonQuery();
                return rowsDeleted > 0;
            }
        }

        public bool Update(SoftUser user)
        {
            log.DebugFormat("Updating user {0}", user);
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = @"UPDATE soft_users SET 
                                    username = @username, 
                                    password = @password 
                                    WHERE id = @id";

                var paramUsername = comm.CreateParameter();
                paramUsername.ParameterName = "@username";
                paramUsername.Value = user.username;
                comm.Parameters.Add(paramUsername);

                var paramPassword = comm.CreateParameter();
                paramPassword.ParameterName = "@password";
                paramPassword.Value = user.password;
                comm.Parameters.Add(paramPassword);

                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = user.Id;
                comm.Parameters.Add(paramId);

                int rowsUpdated = comm.ExecuteNonQuery();
                return rowsUpdated > 0;
            }
        }

        public SoftUser FindByUsernameAndPassword(string username, string password)
        {
            log.DebugFormat("Finding user by username {0}", username);
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM soft_users WHERE username = @username AND password = @password";
                
                var paramUsername = comm.CreateParameter();
                paramUsername.ParameterName = "@username";
                paramUsername.Value = username;
                comm.Parameters.Add(paramUsername);

                var paramPassword = comm.CreateParameter();
                paramPassword.ParameterName = "@password";
                paramPassword.Value = password;
                comm.Parameters.Add(paramPassword);

                using (var reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var user = new SoftUser(
                            (string)reader["username"], 
                            (string)reader["password"])
                        {
                            Id = (long)reader["id"]
                        };
                        return user;
                    }
                }
            }
            return null;
        }
    }
}